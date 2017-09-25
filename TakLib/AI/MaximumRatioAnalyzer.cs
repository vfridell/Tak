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

        public MaximumRatioAnalyzer(int boardSize)
        {
            _factorsTemplate = GetBestAnalysisFactorsTemplate();
            _boardSize = boardSize;
        }

        public static AnalysisFactors GetBestAnalysisFactorsTemplate()
        {
            Dictionary<string, Tuple<double, double>> bestWeightsGrowth = new Dictionary<string, Tuple<double, double>>()
            {
                {"capStoneDiff", new Tuple<double, double>(1, -.1)},
                {"flatScore", new Tuple<double, double>(86, -.1)},
                {"possibleMovesDiff", new Tuple<double, double>(0, 0)},
                {"wallCountDiff", new Tuple<double, double>(-5, -.1)},
                {"averageSubGraphDiff", new Tuple<double, double>(15, -.1)},
                {"longestSubGraphDiff", new Tuple<double, double>(15, -.1)},
                {"numberOfSubGraphsDiff", new Tuple<double, double>(1, -.1)},
                {"stacksAdvantageDiff", new Tuple<double, double>(10, -.1)},
                {"cornerOwnershipDiff", new Tuple<double, double>(1, -.2)},
                {"sideOwnershipDiff", new Tuple<double, double>(2, -.2)},
                {"centerOwnershipDiff", new Tuple<double, double>(3, -.2)},
            };
            return new AnalysisFactors(bestWeightsGrowth);
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

            GetRoadFinderFactors(board);

            d.whiteAdvantage = _factorsTemplate.CalculateAdvantage(board.Turn);
            return d;
        }

        private double GetWallCountDiff(Board board)
        {
            double wallCountDiff = 0;
            foreach (Coordinate c in new CoordinateEnumerable(board.Size))
            {
                Space space = board.GetSpace(c);
                if (space.Piece?.Type == PieceType.Wall)
                {
                    if (space.Piece?.Color == PieceColor.White)
                        wallCountDiff += 1;
                    else
                        wallCountDiff -= 1;
                }
            }
            return wallCountDiff;
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

        private void GetRoadFinderFactors(Board board)
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

            double edgeDiff = 0;
            double cornerDiff = 0;
            double centerDiff = 0;
            foreach (Coordinate c in new CoordinateEnumerable(board.Size))
            {
                Space space = board.GetSpace(c);
                if (roadFinder.IsCorner(c))
                {
                    if (space.ColorEquals(PieceColor.Black)) cornerDiff--;
                    if (space.ColorEquals(PieceColor.White)) cornerDiff++;
                }
                else if (roadFinder.IsSide(c))
                {
                    if (space.ColorEquals(PieceColor.Black)) edgeDiff--;
                    if (space.ColorEquals(PieceColor.White)) edgeDiff++;
                }
                else
                {
                    if (space.ColorEquals(PieceColor.Black)) centerDiff--;
                    if (space.ColorEquals(PieceColor.White)) centerDiff++;
                }
            }
            _factorsTemplate["cornerOwnershipDiff"].Value = cornerDiff;
            _factorsTemplate["sideOwnershipDiff"].Value = edgeDiff;
            _factorsTemplate["centerOwnershipDiff"].Value = centerDiff;
        }

        public SOMWeightsVector GetSomWeightsVector(Board board)
        {
            throw new NotImplementedException();

        }
    }

    
}
