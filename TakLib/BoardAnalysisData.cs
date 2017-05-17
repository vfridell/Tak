using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TakLib
{
    public class BoardAnalysisData
    {
        public static int[] turnQueenPlayedPts = { 0, 25, 35, 41, 0 };

        public int blackCapStonesInHand;
        public int whiteCapStonesInHand;
        // fewer is better
        public int capStoneDiff { get { return blackCapStonesInHand - whiteCapStonesInHand; } }

        public int blackPossibleMovementMoves;
        public int whitePossibleMovementMoves;
        // more is better
        public int possibleMovesDiff { get { return whitePossibleMovementMoves - blackPossibleMovementMoves; } }

        public int blackUnplayedPieces;
        public int whiteUnplayedPieces;
        public int emptySpaces;
        public int unplayedPiecesDiff
        {
            get
            {
                int bTurn = blackTurnNumber > 16 ? bTurn = 16 : blackTurnNumber;
                int wTurn = whiteTurnNumber > 16 ? wTurn = 16 : whiteTurnNumber;
                int whitePoints = unplayedPointMap[new Tuple<int, int>(wTurn, whiteUnplayedPieces)];
                int blackPoints = unplayedPointMap[new Tuple<int, int>(bTurn, blackUnplayedPieces)];
                return whitePoints - blackPoints;
            }
        }

        public int flatScore;
        // more is better

        public bool whiteToPlay;

        public int turnNumber;
        public int blackTurnNumber { get { return (int)Math.Floor((double)turnNumber / 2); } }
        public int whiteTurnNumber { get { return (int)Math.Ceiling((double)turnNumber / 2); } }

        public int blackWallCount;
        public int whiteWallCount;
        // fewer is better
        public int wallCountDiff { get { return whiteWallCount - blackWallCount; } }

        public bool whiteCanMoveAnt;
        public bool whiteCanMoveQueen;
        public bool blackCanMoveAnt;
        public bool blackCanMoveQueen;

        public int whiteMoveablePieces;
        public int blackMoveablePieces;
        // more is better
        public int movementPlacementAdvantageDiff { get { return whiteMoveablePieces - blackMoveablePieces; } }

        public int blackOwnedBeetleStacks;
        public int whiteOwnedBeetleStacks;
        // more is better
        public int ownedBeetleStacksDiff { get { return whiteOwnedBeetleStacks - blackOwnedBeetleStacks; } }

        public GameResult gameResult;
        public int winningResultDiff
        {
            get
            {
                switch (gameResult)
                {
                    case GameResult.WhiteFlat:
                    case GameResult.WhiteRoad:
                        return int.MaxValue;
                    case GameResult.BlackFlat:
                    case GameResult.BlackRoad:
                        return int.MinValue;
                    default:
                        return 0;
                }
            }
        }

        private BoardAnalysisWeights _weights;
        public double whiteAdvantage
        {
            get
            {
                return (capStoneDiff * _weights.capStoneDiffWeight) +
                        (flatScore * _weights.flatScoreWeight) +
                        (possibleMovesDiff * _weights.possibleMovesDiffWeight) +
                        (wallCountDiff * _weights.wallCountDiffWeight) +
                        (unplayedPiecesDiff * _weights.unplayedPiecesDiffWeight) +
                        (winningResultDiff) +
                        (ownedBeetleStacksDiff * _weights.ownedBeetleStacksWeight) +
                        (movementPlacementAdvantageDiff * _weights.movementPlacementDiffWeight);
            }
        }

        private BoardAnalysisData() { }

        public static BoardAnalysisData GetBoardAnalysisData(Board board, BoardAnalysisWeights weights)
        {
            BoardAnalysisData d = new BoardAnalysisData();
            d._weights = weights;
            d.gameResult = board.GameResult;
            d.blackCapStonesInHand = board.CapStonesInHand(PieceColor.Black);
            d.whiteCapStonesInHand = board.CapStonesInHand(PieceColor.White);

            IEnumerable<Move> whiteMoves = MoveGenerator.GetAllMoves(board, PieceColor.White);
            IEnumerable<Move> blackMoves = MoveGenerator.GetAllMoves(board, PieceColor.Black);
            d.blackPossibleMovementMoves = whiteMoves.Count(m => m is MoveStack);
            d.whitePossibleMovementMoves = blackMoves.Count(m => m is MoveStack);

            d.blackUnplayedPieces = board.StonesInHand(PieceColor.Black);
            d.whiteUnplayedPieces = board.StonesInHand(PieceColor.White);
            d.flatScore = board.FlatScore;
            d.emptySpaces = board.EmptySpaces;
            d.gameResult = board.GameResult;
            d.whiteToPlay = board.WhiteToPlay;
            d.turnNumber = board.Turn;
            CountWalls(board, d);

            d.whiteCanMoveAnt = board.whiteCanMoveAnt;
            d.blackCanMoveAnt = board.blackCanMoveAnt;
            d.whiteCanMoveQueen = board.whiteCanMoveQueen;
            d.blackCanMoveQueen = board.blackCanMoveQueen;
            d.whiteMoveablePieces = board.whiteMoveablePieces;
            d.blackMoveablePieces = board.blackMoveablePieces;
            d.blackOwnedBeetleStacks = board.blackOwnedBeetleStacks;
            d.whiteOwnedBeetleStacks = board.whiteOwnedBeetleStacks;

            return d;
        }

        private static void CountWalls(Board board, BoardAnalysisData data)
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

        public override string ToString()
        {
            return string.Format("capStoneDiff: {0} * {1} = {2}\n", capStoneDiff, _weights.capStoneDiffWeight, capStoneDiff * _weights.capStoneDiffWeight) +
                    string.Format("hivailableSpaceDiff: {0} * {1} = {2}\n", hivailableSpaceDiff, _weights.flatScoreWeight, hivailableSpaceDiff * _weights.flatScoreWeight) +
                    string.Format("possibleMovesDiff: {0} * {1} = {2}\n", possibleMovesDiff, _weights.possibleMovesDiffWeight, possibleMovesDiff * _weights.possibleMovesDiffWeight) +
                    string.Format("wallCountDiff: {0} * {1} = {2}\n", wallCountDiff, _weights.wallCountDiffWeight, wallCountDiff * _weights.wallCountDiffWeight) +
                    string.Format("unplayedPiecesDiff: {0} * {1} = {2}\n", unplayedPiecesDiff, _weights.unplayedPiecesDiffWeight, unplayedPiecesDiff * _weights.unplayedPiecesDiffWeight) +
                    string.Format("queenPlacementDiff: {0} * {1} = {2}\n", queenPlacementDiff, _weights.queenPlacementDiffWeight, queenPlacementDiff * _weights.queenPlacementDiffWeight) +
                    string.Format("winningResultDiff: {0}\n", winningResultDiff) +
                    string.Format("ownedBeetleStacksDiff: {0} * {1} = {2}\n", ownedBeetleStacksDiff, _weights.ownedBeetleStacksWeight, ownedBeetleStacksDiff * _weights.ownedBeetleStacksWeight) +
                    string.Format("movementPlacementAdvantageDiff: {0} * {1} = {2}\n", movementPlacementAdvantageDiff, _weights.movementPlacementDiffWeight, movementPlacementAdvantageDiff * _weights.movementPlacementDiffWeight);
        }

        internal readonly static Dictionary<Tuple<int, int>, int> unplayedPointMap = new Dictionary<Tuple<int, int>, int>
        {
            { new Tuple<int, int>(0, 11), 5 },
            { new Tuple<int, int>(1, 11), 5 },
            { new Tuple<int, int>(1, 10), 5 },
            { new Tuple<int, int>(2, 10), 0 },
            { new Tuple<int, int>(2, 9), 5 },
            { new Tuple<int, int>(3, 9), 0 },
            { new Tuple<int, int>(3, 8), 5 },
            { new Tuple<int, int>(4, 9), -5 },
            { new Tuple<int, int>(4, 8), 3 },
            { new Tuple<int, int>(4, 7), 5 },
            { new Tuple<int, int>(5, 9), -10 },
            { new Tuple<int, int>(5, 8), -5 },
            { new Tuple<int, int>(5, 7), 3 },
            { new Tuple<int, int>(5, 6), 3 },
            { new Tuple<int, int>(6, 9), -10 },
            { new Tuple<int, int>(6, 8), -5 },
            { new Tuple<int, int>(6, 7), 3 },
            { new Tuple<int, int>(6, 6), 5 },
            { new Tuple<int, int>(6, 5), 0 },
            { new Tuple<int, int>(7, 9), -10 },
            { new Tuple<int, int>(7, 8), -10 },
            { new Tuple<int, int>(7, 7), -5 },
            { new Tuple<int, int>(7, 6), 3 },
            { new Tuple<int, int>(7, 5), 3 },
            { new Tuple<int, int>(7, 4), 0 },
            { new Tuple<int, int>(8, 9), -10 },
            { new Tuple<int, int>(8, 8), -10 },
            { new Tuple<int, int>(8, 7), -5 },
            { new Tuple<int, int>(8, 6), 3 },
            { new Tuple<int, int>(8, 5), 3 },
            { new Tuple<int, int>(8, 4), 0 },
            { new Tuple<int, int>(8, 3), -5 },
            { new Tuple<int, int>(9, 9), -10 },
            { new Tuple<int, int>(9, 8), -10 },
            { new Tuple<int, int>(9, 7), -5 },
            { new Tuple<int, int>(9, 6), 0 },
            { new Tuple<int, int>(9, 5), 3 },
            { new Tuple<int, int>(9, 4), 3 },
            { new Tuple<int, int>(9, 3), -5 },
            { new Tuple<int, int>(9, 2), -10 },
            { new Tuple<int, int>(10, 9), -10 },
            { new Tuple<int, int>(10, 8), -10 },
            { new Tuple<int, int>(10, 7), -5 },
            { new Tuple<int, int>(10, 6), 3 },
            { new Tuple<int, int>(10, 5), 3 },
            { new Tuple<int, int>(10, 4), 3 },
            { new Tuple<int, int>(10, 3), -5 },
            { new Tuple<int, int>(10, 2), -10 },
            { new Tuple<int, int>(10, 1), -10 },
            { new Tuple<int, int>(11, 9), -10 },
            { new Tuple<int, int>(11, 8), -10 },
            { new Tuple<int, int>(11, 7), -10 },
            { new Tuple<int, int>(11, 6), -5 },
            { new Tuple<int, int>(11, 5), 3 },
            { new Tuple<int, int>(11, 4), 3 },
            { new Tuple<int, int>(11, 3), 0 },
            { new Tuple<int, int>(11, 2), -5 },
            { new Tuple<int, int>(11, 1), -10 },
            { new Tuple<int, int>(11, 0), -10 },
            { new Tuple<int, int>(12, 9), -10 },
            { new Tuple<int, int>(12, 8), -10 },
            { new Tuple<int, int>(12, 7), -10 },
            { new Tuple<int, int>(12, 6), -5 },
            { new Tuple<int, int>(12, 5), 0 },
            { new Tuple<int, int>(12, 4), 3 },
            { new Tuple<int, int>(12, 3), 3 },
            { new Tuple<int, int>(12, 2), -5 },
            { new Tuple<int, int>(12, 1), -5 },
            { new Tuple<int, int>(12, 0), -10 },
            { new Tuple<int, int>(13, 9), -10 },
            { new Tuple<int, int>(13, 8), -10 },
            { new Tuple<int, int>(13, 7), -5 },
            { new Tuple<int, int>(13, 6), -5 },
            { new Tuple<int, int>(13, 5), 3 },
            { new Tuple<int, int>(13, 4), 3 },
            { new Tuple<int, int>(13, 3), 0 },
            { new Tuple<int, int>(13, 2), 0 },
            { new Tuple<int, int>(13, 1), 0 },
            { new Tuple<int, int>(13, 0), -5 },
            { new Tuple<int, int>(14, 9), -10 },
            { new Tuple<int, int>(14, 8), -10 },
            { new Tuple<int, int>(14, 7), -10 },
            { new Tuple<int, int>(14, 6), -5 },
            { new Tuple<int, int>(14, 5), -5 },
            { new Tuple<int, int>(14, 4), 0 },
            { new Tuple<int, int>(14, 3), 3 },
            { new Tuple<int, int>(14, 2), 3 },
            { new Tuple<int, int>(14, 1), 0 },
            { new Tuple<int, int>(14, 0), -5 },
            { new Tuple<int, int>(15, 9), -10 },
            { new Tuple<int, int>(15, 8), -10 },
            { new Tuple<int, int>(15, 7), -5 },
            { new Tuple<int, int>(15, 6), -5 },
            { new Tuple<int, int>(15, 5), 0 },
            { new Tuple<int, int>(15, 4), 3 },
            { new Tuple<int, int>(15, 3), 3 },
            { new Tuple<int, int>(15, 2), 3 },
            { new Tuple<int, int>(15, 1), 0 },
            { new Tuple<int, int>(15, 0), 0 },
            { new Tuple<int, int>(16, 9), -10 },
            { new Tuple<int, int>(16, 8), -10 },
            { new Tuple<int, int>(16, 7), -10 },
            { new Tuple<int, int>(16, 6), -5 },
            { new Tuple<int, int>(16, 5), -5 },
            { new Tuple<int, int>(16, 4), -5 },
            { new Tuple<int, int>(16, 3), 3 },
            { new Tuple<int, int>(16, 2), 3 },
            { new Tuple<int, int>(16, 1), 0 },
            { new Tuple<int, int>(16, 0), 0 },
        };

    }
}
