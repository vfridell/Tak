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
            Move bestMove;
            var cancelSource = new CancellationTokenSource();
            double score;
            var initialContext = new NegamaxContext(null, board, 0, false);
            score = Negamax(initialContext, MinValue, MaxValue, _depth, playingWhite ? 1 : -1, cancelSource.Token, out bestMove);
            return bestMove;
        }

        public Task<Move> PickBestMoveAsync(Board board, CancellationToken aiCancelToken)
        {
            return Task.Run(() => {
                Move bestMove;
                var initialContext = new NegamaxContext(null, board, 0, false);
                double score = Negamax(initialContext, MinValue, MaxValue, _depth, playingWhite ? 1 : -1, aiCancelToken, out bestMove);
                return bestMove;
            });
        }

        public void BeginNewGame(bool playingWhite, int boardSize)
        {
            _playingWhite = playingWhite;
        }

        private double Negamax(NegamaxContext context, double alpha, double beta, int depth, int color, CancellationToken aiCancelToken, out Move bestMove)
        {
            aiCancelToken.ThrowIfCancellationRequested();
            if (depth == 0 || context.Board.GameResult != GameResult.Incomplete)
            {
                bestMove = null;
                if(context.ScoreCalculated) return context.Score;
                else
                {
                    return _analyzer.Analyze(context.Board).whiteAdvantage * color;
                }
            }

            IEnumerable<NegamaxContext> orderedAnalysis = GetSortedMoves(context.Board, color, depth);
            double bestScore = MinValue;
            Move localBestMove = orderedAnalysis.First().Move;

            foreach (var nextContext in orderedAnalysis)
            {
                Move subBestMove;
                double score = -Negamax(nextContext, -beta, -alpha, depth - 1, -color, aiCancelToken, out subBestMove);
                if (score >= bestScore) localBestMove = nextContext.Move;
                bestScore = Math.Max(bestScore, score);
                alpha = Math.Max(alpha, score);
                if (alpha >= beta)
                    break;
            };

            bestMove = localBestMove;
            return bestScore;
        }

        private IEnumerable<NegamaxContext> GetSortedMoves(Board board, int color, int depth)
        {
            var advantageDict = new SortedDictionary<double, HashSet<NegamaxContext>>();
            var allMoves = board.GetAllMoves();
            if (depth == _depth)
            {
                foreach (Move move in allMoves)
                {
                    var futureBoard = ComputeFutureBoard(board, move);
                    double currentAdvantage = _analyzer.Analyze(futureBoard).whiteAdvantage * color;
                    if (!advantageDict.ContainsKey(currentAdvantage))
                        advantageDict.Add(currentAdvantage, new HashSet<NegamaxContext>());
                    advantageDict[currentAdvantage].Add(new NegamaxContext(move, futureBoard, currentAdvantage, true));
                }

                foreach (var kvp in (color == 1 ? advantageDict.Reverse() : advantageDict))
                {
                    foreach (var pair in kvp.Value) yield return pair;
                }
            }
            else
            {
                foreach (var move in allMoves)
                {
                    var futureBoard = ComputeFutureBoard(board, move);
                    yield return new NegamaxContext(move, futureBoard, 0, false);
                }
            }
        }

        private Board ComputeFutureBoard(Board currentBoard, Move move)
        {
            Board futureBoard = currentBoard.Clone();
            move.Apply(futureBoard);
            futureBoard.EndPlayerMove();
            if (futureBoard.Round % 2 != 0) futureBoard.EndTurn();
            return futureBoard;
        }

        private string _name;
        public override string Name
        {
            get { return string.IsNullOrEmpty(_name) ? string.Format("Optimus{0}Deep", _depth) : _name; }
        }
    }

    public struct NegamaxContext
    {
        public NegamaxContext(Move move, Board board, double score, bool calculated)
        {
            Move = move;
            Board = board;
            Score = score;
            ScoreCalculated = calculated;
        }

        public Move Move;
        public Board Board;
        public double Score;
        public bool ScoreCalculated;
    }
}
