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
    public class BoardAnalyzer : IBoardAnalyzer
    {
        protected readonly int _boardSize;
        protected BoardAnalysisWeights _weights;

        public BoardAnalyzer(int boardSize, BoardAnalysisWeights weights)
        {
            _weights = weights;
            _boardSize = boardSize;
        }

        public virtual IAnalysisResult Analyze (Board board)
        {
            BoardAnalysisData d = new BoardAnalysisData();
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
            if (d.winningResultDiff != 0)
            {
                d.whiteAdvantage = d.winningResultDiff;
                return d;
            }
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

            d.whiteAdvantage = (d.capStoneDiff * d.weights.capStoneDiffWeight) +
                               (d.flatScore * d.weights.flatScoreWeight) +
                               (d.possibleMovesDiff * d.weights.possibleMovesDiffWeight) +
                               (d.wallCountDiff * d.weights.wallCountDiffWeight) +
                               (d.averageSubGraphDiff * d.weights.averageSubGraphDiffWeight) +
                               (d.longestSubGraphDiff * d.weights.longestSubGraphDiffWeight) +
                               (d.numberOfSubGraphsDiff * d.weights.numberOfSubGraphsDiffWeight);

            return d;
        }

        protected virtual void CountWalls(Board board, BoardAnalysisData data)
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

        public virtual SOMWeightsVector GetSomWeightsVector(Board board)
        {
            SOMWeightsVector vector = new SOMWeightsVector();
            BoardAnalysisData analysis = (BoardAnalysisData)Analyze(board);
            vector.Add(analysis.flatScore);
            vector.Add(analysis.averageSubGraphDiff);
            vector.Add(analysis.longestSubGraphDiff);
            vector.Add(analysis.capStoneDiff);
            vector.Add(analysis.numberOfSubGraphsDiff);
            vector.Add(analysis.wallCountDiff);
            //vector.Add(analysis.possibleMovesDiff);
            vector.Add(analysis.winningResultDiff);
            return vector;
        }
    }

    
}
