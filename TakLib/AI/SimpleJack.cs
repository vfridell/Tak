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
        public readonly double MinValue = SimpleAnalysisData.MinValue;
        public readonly double MaxValue = SimpleAnalysisData.MaxValue;

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
            var cancelSource = new CancellationTokenSource();
            var initialContext = new NegamaxContext(null, board, 0, false);
            Move bestMove = NegamaxRoot(initialContext, MinValue, MaxValue, _depth, playingWhite ? 1 : -1, cancelSource.Token);
            return bestMove;
        }

        public Task<Move> PickBestMoveAsync(Board board, CancellationToken aiCancelToken)
        {
            return Task.Run(() => {
                var initialContext = new NegamaxContext(null, board, 0, false);
                Move bestMove = NegamaxRoot(initialContext, MinValue, MaxValue, _depth, playingWhite ? 1 : -1, aiCancelToken);
                return bestMove;
            });
        }

        public void BeginNewGame(bool playingWhite, int boardSize)
        {
            _playingWhite = playingWhite;
        }

        private Move NegamaxRoot(NegamaxContext context, double alpha, double beta, int depth, int color, CancellationToken aiCancelToken)
        {
            aiCancelToken.ThrowIfCancellationRequested();

            IEnumerable<NegamaxContext> orderedAnalysis = GetSortedMoves(context.Board, color, aiCancelToken);
            double bestScore = MinValue;
            Move localBestMove = orderedAnalysis.First().Move;

            foreach (var nextContext in orderedAnalysis)
            {
                double score = -Negamax(nextContext, -beta, -alpha, depth - 1, -color, aiCancelToken);
                if (score > bestScore) localBestMove = nextContext.Move;
                bestScore = Math.Max(bestScore, score);
                alpha = Math.Max(alpha, score);
                if (alpha >= beta)
                    break;
            };

            return localBestMove;
        }


        private double Negamax(NegamaxContext context, double alpha, double beta, int depth, int color, CancellationToken aiCancelToken)
        {
            aiCancelToken.ThrowIfCancellationRequested();
            if (depth == 0 || context.Board.GameResult != GameResult.Incomplete)
            {
                double score = _analyzer.Analyze(context.Board).whiteAdvantage * color;
                return score;
            }

            IEnumerable<NegamaxContext> orderedAnalysis = GetSortedMoves(context.Board, color, aiCancelToken);
            double bestScore = MinValue;

            foreach (var nextContext in orderedAnalysis)
            {
                double score = -Negamax(nextContext, -beta, -alpha, depth - 1, -color, aiCancelToken);
                bestScore = Math.Max(bestScore, score);
                alpha = Math.Max(alpha, score);
                if (alpha >= beta)
                    break;
            };

            return bestScore;
        }

        private IEnumerable<NegamaxContext> GetSortedMoves(Board board, int color, CancellationToken aiCancelToken)
        {
            var localMovesData = new ConcurrentBag<NegamaxContext>();

            // I think this parallelism allows for randomness when picking multiple moves that all analyze to the same advantage number
            Parallel.ForEach(board.GetAllMoves(), (nextMove, parallelLoopState) =>
            {
                if (parallelLoopState.ShouldExitCurrentIteration) parallelLoopState.Stop();
                if (aiCancelToken.IsCancellationRequested) parallelLoopState.Stop();
                Board futureBoard = Board.ComputeFutureBoard(board, nextMove);
                localMovesData.Add(new NegamaxContext(nextMove, futureBoard, 0, false));
            });

            aiCancelToken.ThrowIfCancellationRequested();

            return color == 1
                ? localMovesData.OrderBy( m => m.Board.GameResult == GameResult.WhiteFlat || m.Board.GameResult == GameResult.WhiteRoad ? 1 : 2)
                : localMovesData.OrderBy( m => m.Board.GameResult == GameResult.BlackFlat || m.Board.GameResult == GameResult.BlackRoad ? 1 : 2);
        }

        private string _name;
        public override string Name
        {
            get { return string.IsNullOrEmpty(_name) ? string.Format("SimpleJack{0}", _depth) : _name; }
        }

    }

    

}
