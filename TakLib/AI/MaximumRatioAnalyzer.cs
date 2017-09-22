using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TakLib.AI.Helpers;

namespace TakLib
{
    // this class must be thread safe
    [Serializable]
    public class MaximumRatioAnalyzer : IBoardAnalyzer
    {
        protected readonly int _boardSize;
        protected AnalysisFactors _factorsTemplate;

        public MaximumRatioAnalyzer(int boardSize, AnalysisFactors factorsTemplate)
        {
            _factorsTemplate = factorsTemplate;
            _boardSize = boardSize;
        }


        public IAnalysisResult Analyze (Board board)
        {
            MaximumRatioAnalysisData d = new MaximumRatioAnalysisData();
            d.gameResult = GameResultService.GetGameResult(board);
            if (d.winningResultDiff != 0)
            {
                d.whiteAdvantage = d.winningResultDiff;
                return d;
            }

            if(_factorsTemplate["capStoneDiff"].Weight != 0)
            {
                _factorsTemplate["capStoneDiff"].Value = (board.CapStonesInHand(PieceColor.Black) - board.CapStonesInHand(PieceColor.White));
            }

            if(_factorsTemplate["possibleMovesDiff"].Weight != 0)
            {
                IEnumerable<Move> whiteMoves = MoveGenerator.GetAllMoves(board, PieceColor.White);
                IEnumerable<Move> blackMoves = MoveGenerator.GetAllMoves(board, PieceColor.Black);
                _factorsTemplate["possibleMovesDiff"].Value = whiteMoves.Count(m => m is MoveStack) - blackMoves.Count(m => m is MoveStack);
            }

            _factorsTemplate["flatScore"].Value = board.FlatScore;

            if (_factorsTemplate["wallCountDiff"].Weight != 0)
            {
                _factorsTemplate["wallCountDiff"].Value = GetWallCountDiff(board);
            }

            if (_factorsTemplate["stacksAdvantageDiff"].Weight != 0)
            {
                _factorsTemplate["stacksAdvantageDiff"].Value = GetStackAdvantageDiff(board);
            }

            if (_factorsTemplate["averageSubGraphDiff"].Weight != 0 || _factorsTemplate["longestSubGraphDiff"].Weight != 0 ||
                _factorsTemplate["numberOfSubGraphsDiff"].Weight != 0)
            {
                RoadFinder roadFinder = new RoadFinder(_boardSize);
                roadFinder.Analyze(board, PieceColor.White);
                double whiteLongestSubgraph = roadFinder.LongestSubGraphLength;
                double whiteAverageSubgraph = roadFinder.AverageSubGraphLength;
                double whiteNumberOfSubgraphs = roadFinder.SubGraphCount;

                roadFinder.Analyze(board, PieceColor.Black);
                double blackLongestSubgraph = roadFinder.LongestSubGraphLength;
                double blackAverageSubgraph = roadFinder.AverageSubGraphLength;
                double blackNumberOfSubgraphs = roadFinder.SubGraphCount;

                _factorsTemplate["averageSubGraphDiff"].Value = whiteAverageSubgraph - blackAverageSubgraph;
                _factorsTemplate["longestSubGraphDiff"].Value = whiteLongestSubgraph - blackLongestSubgraph;
                _factorsTemplate["numberOfSubGraphsDiff"].Value = whiteNumberOfSubgraphs - blackNumberOfSubgraphs;
            }

            d.whiteAdvantage = _factorsTemplate.CalculateAdvantage(board.Turn);
            return d;
        }

        private double GetWallCountDiff(Board board)
        {
            double result = 0;
            foreach (Coordinate c in new CoordinateEnumerable(board.Size))
            {
                Space space = board.GetSpace(c);
                if (space.Piece?.Type == PieceType.Wall)
                {
                    if (space.Piece?.Color == PieceColor.White)
                        result += 1;
                    else
                        result -= 1;
                }
            }
            return result;
        }

        private double GetStackAdvantageDiff(Board board)
        {
            double result = 0;
            foreach (Coordinate c in new CoordinateEnumerable(board.Size))
            {
                Space space = board.GetSpace(c);
                if (space.IsEmpty) continue;
                PieceStack stack = board.GetPieceStack(c);
                if(stack.Count == 1) continue;
                DistanceAvailable up = board.GetDistanceAvailable(c, Direction.Up);
                DistanceAvailable down = board.GetDistanceAvailable(c, Direction.Down);
                DistanceAvailable left = board.GetDistanceAvailable(c, Direction.Left);
                DistanceAvailable right = board.GetDistanceAvailable(c, Direction.Right);
                int stackAdvantage = (stack.OwnerPieceCount * up.Distance) +
                    (stack.OwnerPieceCount * down.Distance) +
                    (stack.OwnerPieceCount * left.Distance) +
                    (stack.OwnerPieceCount * right.Distance);
                stackAdvantage -= stack.CapturedPieceCount;
                if (space.ColorEquals(PieceColor.White))
                    result += stackAdvantage;
                else
                    result -= stackAdvantage;
            }
            return result;
        }

        public SOMWeightsVector GetSomWeightsVector(Board board)
        {
            SOMWeightsVector vector = new SOMWeightsVector();
            BoardStacksAnalysis analysis = (BoardStacksAnalysis)Analyze(board);
            vector.Add(analysis.flatScore);
            vector.Add(analysis.averageSubGraphDiff);
            vector.Add(analysis.longestSubGraphDiff);
            vector.Add(analysis.capStoneDiff);
            vector.Add(analysis.numberOfSubGraphsDiff);
            vector.Add(analysis.wallCountDiff);
            //vector.Add(analysis.possibleMovesDiff);
            vector.Add(analysis.winningResultDiff);
            vector.Add(analysis.stacksAdvantageDiff);
            return vector;
        }
    }

    
}
