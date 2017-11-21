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
    public class BoardStacksAnalyzer : IBoardAnalyzer
    {
        protected readonly int _boardSize;
        protected BoardAnalysisWeights _weights;

        public BoardStacksAnalyzer(int boardSize, BoardAnalysisWeights weights)
        {
            _weights = weights;
            _boardSize = boardSize;
        }

        public IAnalysisResult Analyze (Board board)
        {
            BoardStacksAnalysis d = new BoardStacksAnalysis();
            d.weights = _weights;
            d.gameResult = board.GameResult;
            d.gameResult = GameResultService.GetGameResult(board);
            if (d.winningResultDiff != 0)
            {
                d.whiteAdvantage = d.winningResultDiff;
                return d;
            }

            if (d.weights.capStoneDiffWeight != 0)
            {
                d.blackCapStonesInHand = board.CapStonesInHand(PieceColor.Black);
                d.whiteCapStonesInHand = board.CapStonesInHand(PieceColor.White);
            }

            if (d.weights.possibleMovesDiffWeight != 0)
            {
                IEnumerable<Move> whiteMoves = MoveGenerator.GetAllMoves(board, PieceColor.White);
                IEnumerable<Move> blackMoves = MoveGenerator.GetAllMoves(board, PieceColor.Black);
                d.whitePossibleMovementMoves = whiteMoves.Count(m => m is MoveStack);
                d.blackPossibleMovementMoves = blackMoves.Count(m => m is MoveStack);
            }

            d.blackUnplayedPieces = board.StonesInHand(PieceColor.Black);
            d.whiteUnplayedPieces = board.StonesInHand(PieceColor.White);
            d.flatScore = board.FlatScore;
            d.emptySpaces = board.EmptySpaces;

            d.whiteToPlay = board.WhiteToPlay;
            d.turnNumber = board.Turn;
            if(d.weights.wallCountDiffWeight != 0) CountWalls(board, d);
            if(d.weights.stacksAdvantageDiffWeight != 0) AnalyzeStacks(board, d);

            if (d.weights.numberOfSubGraphsDiffWeight != 0 || d.weights.averageSubGraphDiffWeight != 0 ||
                d.weights.longestSubGraphDiffWeight != 0)
            {
                RoadFinder roadFinder = new RoadFinder(_boardSize);
                roadFinder.Analyze(board, PieceColor.White);
                d.whiteLongestSubgraph = roadFinder.LongestSubGraphLength;
                d.whiteAverageSubgraph = (int)Math.Ceiling(roadFinder.AverageSubGraphLength);
                d.whiteNumberOfSubgraphs = roadFinder.SubGraphCount;

                roadFinder.Analyze(board, PieceColor.Black);
                d.blackLongestSubgraph = roadFinder.LongestSubGraphLength;
                d.blackAverageSubgraph = (int)Math.Ceiling(roadFinder.AverageSubGraphLength);
                d.blackNumberOfSubgraphs = roadFinder.SubGraphCount;
            }

            d.whiteAdvantage = (d.capStoneDiff * d.weights.capStoneDiffWeight) +
                        (d.flatScore * d.weights.flatScoreWeight) +
                        (d.possibleMovesDiff * d.weights.possibleMovesDiffWeight) +
                        (d.wallCountDiff * d.weights.wallCountDiffWeight) +
                        (d.averageSubGraphDiff * d.weights.averageSubGraphDiffWeight) +
                        (d.longestSubGraphDiff * d.weights.longestSubGraphDiffWeight) +
                        (d.numberOfSubGraphsDiff * d.weights.numberOfSubGraphsDiffWeight) +
                        (d.stacksAdvantageDiff * d.weights.stacksAdvantageDiffWeight);

            return d;
        }



        private void CountWalls(Board board, BoardStacksAnalysis data)
        {
            foreach (Coordinate c in new CoordinateEnumerable(board.Size))
            {
                Space space = board.GetSpace(c);
                if (space.Piece?.Type == PieceType.Wall)
                {
                    if (space.Piece?.Color == PieceColor.White)
                        data.whiteWallCount++;
                    else
                        data.blackWallCount++;
                }
            }
        }

        private void AnalyzeStacks(Board board, BoardStacksAnalysis data)
        {

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
                    data.whiteStackAdvantage += stackAdvantage;
                else
                    data.blackStackAdvantage += stackAdvantage;
            }
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
