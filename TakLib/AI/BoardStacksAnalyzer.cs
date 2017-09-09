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
            d.blackCapStonesInHand = board.CapStonesInHand(PieceColor.Black);
            d.whiteCapStonesInHand = board.CapStonesInHand(PieceColor.White);

            IEnumerable<Move> whiteMoves = MoveGenerator.GetAllMoves(board, PieceColor.White);
            IEnumerable<Move> blackMoves = MoveGenerator.GetAllMoves(board, PieceColor.Black);
            d.whitePossibleMovementMoves = whiteMoves.Count(m => m is MoveStack);
            d.blackPossibleMovementMoves = blackMoves.Count(m => m is MoveStack);

            d.blackUnplayedPieces = board.StonesInHand(PieceColor.Black);
            d.whiteUnplayedPieces = board.StonesInHand(PieceColor.White);
            d.flatScore = board.FlatScore;
            d.emptySpaces = board.EmptySpaces;
            d.gameResult = GameResultService.GetGameResult(board);
            d.whiteToPlay = board.WhiteToPlay;
            d.turnNumber = board.Turn;
            CountWalls(board, d);

            RoadFinder roadFinder = new RoadFinder(_boardSize);
            roadFinder.Analyze(board, PieceColor.White);
            d.whiteLongestSubgraph = roadFinder.LongestSubGraphLength;
            d.whiteAverageSubgraph = roadFinder.AverageSubGraphLength;
            d.whiteNumberOfSubgraphs = roadFinder.SubGraphCount;

            roadFinder.Analyze(board, PieceColor.Black);
            d.blackLongestSubgraph = roadFinder.LongestSubGraphLength;
            d.blackAverageSubgraph = roadFinder.AverageSubGraphLength;
            d.blackNumberOfSubgraphs = roadFinder.SubGraphCount;

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
                DistanceAvailable up = board.GetDistanceAvailable(c, Direction.Up);
                DistanceAvailable down = board.GetDistanceAvailable(c, Direction.Down);
                DistanceAvailable left = board.GetDistanceAvailable(c, Direction.Left);
                DistanceAvailable right = board.GetDistanceAvailable(c, Direction.Right);
                int stackAdvantage = (stack.OwnerPieceCount * up.Distance) +
                    (stack.OwnerPieceCount * down.Distance) +
                    (stack.OwnerPieceCount * left.Distance) +
                    (stack.OwnerPieceCount * right.Distance);
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
