using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using TakLib.AI;

namespace TakLib
{
    public class JohnnyDeep : Player, ITakAI
    {
        private bool _playingWhite;
        private int _depth;
        private IBoardAnalyzer _analyzer;
        public readonly int MinValue = SimpleAnalysisData.MinValue;
        public readonly int MaxValue = SimpleAnalysisData.MaxValue;
        private AnalysisMemoryCollection _memories;

        public JohnnyDeep(int depth, IBoardAnalyzer boardAnalyzer)
        {
            _analyzer = boardAnalyzer;
            _depth = depth;
        }

        public JohnnyDeep(int depth, IBoardAnalyzer boardAnalyzer, string name)
            : this(depth, boardAnalyzer)
        {
            _name = name;
        }

        public bool playingWhite { get { return _playingWhite; } }

        public Move MakeBestMove(Game game)
        {
            Board board = game.CurrentBoard;
            Move move;
            if ((_playingWhite && board.WhiteToPlay) || (!_playingWhite && !board.WhiteToPlay))
            {
                move = PickBestMove(board);
                game.ApplyMove(move);
            }
            else
            {
                throw new Exception("It is not my move :(");
            }
            return move;
        }

        public Move PickBestMove(Board board)
        {
            Move bestMove;
            var cancelSource = new CancellationTokenSource();
            int score;

            AnalysisMemory memory;
            if (_memories.TryGetValue(board, out memory))
            {
                bestMove = memory.BestMove;
            }
            else
            {
                score = Negamax(board, null, MinValue, MaxValue, _depth, playingWhite ? 1 : -1, cancelSource.Token, out bestMove);
                _memories.Add(board, new AnalysisMemory()
                {
                    AnalysisDepth = _depth,
                    BestMove = bestMove,
                    AnalysisKey = _memories.Key,
                    Score = score
                });
            }
            return bestMove;
        }

        public Task<Move> PickBestMoveAsync(Board board, CancellationToken aiCancelToken)
        {
            return Task.Run(() => {
                Move bestMove;
                int score = Negamax(board, null, MinValue, MaxValue, _depth, playingWhite ? 1 : -1, aiCancelToken, out bestMove);
                return bestMove;
            });
        }

        public void BeginNewGame(bool playingWhite, int boardSize)
        {
            _playingWhite = playingWhite;
            _memories = AnalysisMemoryCollection.LoadMemories("JohnnyDeep", _depth);
        }

        private int Negamax(Board board, Move fromMove, int alpha, int beta, int depth, int color, CancellationToken aiCancelToken, out Move bestMove)
        {
            AnalysisMemory memory;
            if (_memories.TryGetValue(board, out memory))
            {
                bestMove = memory.BestMove;
                return memory.Score;
            }

            aiCancelToken.ThrowIfCancellationRequested();
            if (depth == 0 || board.GameResult != GameResult.Incomplete)
            {
                bestMove = null;
                return _analyzer.Analyze(board).whiteAdvantage * color;
            }

            IEnumerable<Tuple<Move,Board>> orderedAnalysis = GetSortedMoves(board, aiCancelToken);
            int bestScore = MinValue;
            Move localBestMove = orderedAnalysis.First().Item1;

            foreach (var kvp in orderedAnalysis)
            {
                Move subBestMove;
                int score = -Negamax(kvp.Item2, kvp.Item1, -beta, -alpha, depth - 1, -color, aiCancelToken, out subBestMove);
                if (score > bestScore) localBestMove = kvp.Item1;
                bestScore = Math.Max(bestScore, score);
                alpha = Math.Max(alpha, score);
                if (alpha >= beta)
                    break;
            };

            bestMove = localBestMove;
            return bestScore;
        }

        private IEnumerable<Tuple<Move, Board>> GetSortedMoves(Board board, CancellationToken aiCancelToken)
        {
            var localMovesData = new ConcurrentBag<Tuple<Move, Board>>();

            // I think this parallelism allows for randomness when picking multiple moves that all analyze to the same advantage number
            Parallel.ForEach(board.GetAllMoves(), (nextMove, parallelLoopState) =>
            {
                if (parallelLoopState.ShouldExitCurrentIteration) parallelLoopState.Stop();
                if (aiCancelToken.IsCancellationRequested) parallelLoopState.Stop();
                Board futureBoard = board.Clone();

                nextMove.Apply(futureBoard);
                futureBoard.EndPlayerMove();
                if (futureBoard.Round % 2 != 0) futureBoard.EndTurn();

                localMovesData.Add(new Tuple<Move, Board>(nextMove, futureBoard));
            });

            aiCancelToken.ThrowIfCancellationRequested();

            if (board.WhiteToPlay)
            {
                return localMovesData.OrderByDescending(m => m.Item2.GameResult == GameResult.Incomplete ? 0 : 1).ThenByDescending(m => m.Item2.FlatScore);
            }
            else
            {
                return localMovesData.OrderByDescending(m => m.Item2.GameResult == GameResult.Incomplete ? 0 : 1).ThenBy(m => m.Item2.FlatScore);
            }
        }

        private string _name;
        public override string Name
        {
            get { return string.IsNullOrEmpty(_name) ? string.Format("Johnny{0}Deep", _depth) : _name; }
        }

        public void WriteMemories()
        {
            _memories.Write();
        }
    }

    

}
