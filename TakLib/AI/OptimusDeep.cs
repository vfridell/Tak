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
    public class OptimusDeep : Player, ITakAI
    {
        private bool _playingWhite;
        private int _depth;
        private IBoardAnalyzer _analyzer;
        public readonly double MinValue = double.MinValue;
        public readonly double MaxValue = double.MaxValue;

        public OptimusDeep(int depth, IBoardAnalyzer boardAnalyzer)
        {
            _analyzer = boardAnalyzer;
            _depth = depth;
        }

        public OptimusDeep(int depth, IBoardAnalyzer boardAnalyzer, string name)
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
                if(context.ScoreCalculated) return context.Score * color;
                else
                {
                    return _analyzer.Analyze(context.Board).whiteAdvantage * color;
                }
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
            //var advantageDict = new SortedDictionary<double, HashSet<NegamaxContext>>();
            //var allMoves = board.GetAllMoves();
            //foreach (Move move in allMoves)
            //{
            //    var futureBoard = Board.ComputeFutureBoard(board, move);
            //    double currentAdvantage = _analyzer.Analyze(futureBoard).whiteAdvantage;
            //    if (!advantageDict.ContainsKey(currentAdvantage))
            //        advantageDict.Add(currentAdvantage, new HashSet<NegamaxContext>());
            //    advantageDict[currentAdvantage].Add(new NegamaxContext(move, futureBoard, currentAdvantage, true));
            //}
            //foreach (var kvp in color == 1 ? advantageDict.Reverse() : advantageDict)
            //{
            //    foreach (var pair in kvp.Value) yield return pair;
            //}

            var advantageBag = new ConcurrentBag<NegamaxContext>();
            var allMoves = board.GetAllMoves(true);
            Parallel.ForEach(allMoves, (move, parallelLoopState) =>
            {
                if (parallelLoopState.ShouldExitCurrentIteration) parallelLoopState.Stop();
                if (aiCancelToken.IsCancellationRequested) parallelLoopState.Stop();
                var futureBoard = Board.ComputeFutureBoard(board, move);
                double currentAdvantage = _analyzer.Analyze(futureBoard).whiteAdvantage;
                advantageBag.Add(new NegamaxContext(move, futureBoard, currentAdvantage, true));
            });

            IEnumerable<NegamaxContext> sortedContexts = color == 1 ? advantageBag.ToList().OrderByDescending(c => c.Score) : advantageBag.ToList().OrderBy(c => c.Score);

            foreach (var context in sortedContexts)
            {
                yield return context;
            }
        }



        private string _name;
        public override string Name
        {
            get { return string.IsNullOrEmpty(_name) ? string.Format("Optimus{0}Deep", _depth) : _name; }
        }
    }
}
