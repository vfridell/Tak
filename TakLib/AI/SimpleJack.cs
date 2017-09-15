using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace TakLib
{
    public class SimpleJack : Player, ITakAI
    {
        private bool _playingWhite;
        private int _depth;
        private IBoardAnalyzer _analyzer;
        public readonly int MinValue = SimpleAnalysisData.MinValue;
        public readonly int MaxValue = SimpleAnalysisData.MaxValue;

        private Dictionary<int, Move> _lastBestMoveAtDepth;

        public SimpleJack(int depth, int boardSize)
        {
            _analyzer = new SimpleAnalyzer(boardSize);
            _depth = depth;
        }

        public SimpleJack(int depth, int boardSize, string name)
            : this(depth, boardSize)
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
            double score;
            score = Negamax(board, null, MinValue, MaxValue, _depth, playingWhite ? 1 : -1, cancelSource.Token, out bestMove);
            return bestMove;
        }

        public Task<Move> PickBestMoveAsync(Board board, CancellationToken aiCancelToken)
        {
            return Task.Run(() => {
                Move bestMove;
                double score = Negamax(board, null, MinValue, MaxValue, _depth, playingWhite ? 1 : -1, aiCancelToken, out bestMove);
                return bestMove;
            });
        }

        public void BeginNewGame(bool playingWhite, int boardSize)
        {
            _playingWhite = playingWhite;
        }

        private double Negamax(Board board, Move fromMove, double alpha, double beta, int depth, int color, CancellationToken aiCancelToken, out Move bestMove)
        {
            aiCancelToken.ThrowIfCancellationRequested();
            if (depth == 0 || board.GameResult != GameResult.Incomplete)
            {
                bestMove = null;
                double score = _analyzer.Analyze(board).whiteAdvantage * color;
                return score;
            }

            IEnumerable<Tuple<Move,Board>> orderedAnalysis = GetSortedMoves(board, aiCancelToken);
            double bestScore = MinValue;
            Move localBestMove = orderedAnalysis.First().Item1;

            foreach (var kvp in orderedAnalysis)
            {
                Move subBestMove;
                double score = -Negamax(kvp.Item2, kvp.Item1, -beta, -alpha, depth - 1, -color, aiCancelToken, out subBestMove);

                if (score >= bestScore) localBestMove = kvp.Item1;
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

            return playingWhite ? localMovesData.OrderByDescending(m => m.Item2.FlatScore) :
                                  localMovesData.OrderBy(m => m.Item2.FlatScore);
        }

        private string _name;
        public override string Name
        {
            get { return string.IsNullOrEmpty(_name) ? string.Format("SimpleJack{0}", _depth) : _name; }
        }

    }

    

}
