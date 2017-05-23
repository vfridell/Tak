using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TakLib
{
    public class BoardAnalysisData : IAnalysisResult
    {
        public static readonly int MaxValue = int.MaxValue;
        public static readonly int MinValue = int.MinValue;

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

        public int flatScore;
        // positive better for white, negative better for black

        public bool whiteToPlay;

        public int turnNumber;
        public int blackTurnNumber { get { return (int)Math.Floor((double)turnNumber / 2); } }
        public int whiteTurnNumber { get { return (int)Math.Ceiling((double)turnNumber / 2); } }

        public int blackWallCount;
        public int whiteWallCount;
        // fewer is better
        public int wallCountDiff { get { return blackWallCount - whiteWallCount; } }

        public int blackLongestSubgraph;
        public int whiteLongestSubgraph;
        // more is better
        public int longestSubGraphDiff { get { return whiteLongestSubgraph - blackLongestSubgraph; } }

        public int blackAverageSubgraph;
        public int whiteAverageSubgraph;
        public int averageSubGraphDiff  { get { return whiteAverageSubgraph - blackAverageSubgraph; } }
        // more is better

        public int whiteNumberOfSubgraphs;
        public int blackNumberOfSubgraphs;
        public int numberOfSubGraphsDiff  { get { return whiteNumberOfSubgraphs - blackNumberOfSubgraphs; } }
        // more is better

        public GameResult gameResult { get; set; }
        public int winningResultDiff
        {
            get
            {
                switch (gameResult)
                {
                    case GameResult.WhiteFlat:
                    case GameResult.WhiteRoad:
                        return MaxValue - 2;
                    case GameResult.BlackFlat:
                    case GameResult.BlackRoad:
                        return MinValue + 2;
                    default:
                        return 0;
                }
            }
        }

        public BoardAnalysisWeights weights;

        public int whiteAdvantage
        {
            get
            {
                if(winningResultDiff != 0) return winningResultDiff;
                return (capStoneDiff * weights.capStoneDiffWeight) +
                        (flatScore * weights.flatScoreWeight) +
                        (possibleMovesDiff * weights.possibleMovesDiffWeight) +
                        (wallCountDiff * weights.wallCountDiffWeight) +
                        (averageSubGraphDiff * weights.averageSubGraphDiffWeight) +
                        (longestSubGraphDiff * weights.longestSubGraphDiffWeight) +
                        (numberOfSubGraphsDiff * weights.numberOfSubGraphsDiffWeight);
            }
        }

        public BoardAnalysisData() { }

        public override string ToString()
        {
            return $"White Advantage: {whiteAdvantage}";
        }

        public string ToStringDetailed()
        {
            return string.Format("capStoneDiff: {0} * {1} = {2}\n", capStoneDiff, weights.capStoneDiffWeight, capStoneDiff * weights.capStoneDiffWeight) +
                    string.Format("flatScore (Weighted): {0} * {1} = {2}\n", flatScore, weights.flatScoreWeight, flatScore * weights.flatScoreWeight) +
                    string.Format("possibleMovesDiff: {0} * {1} = {2}\n", possibleMovesDiff, weights.possibleMovesDiffWeight, possibleMovesDiff * weights.possibleMovesDiffWeight) +
                    string.Format("wallCountDiff: {0} * {1} = {2}\n", wallCountDiff, weights.wallCountDiffWeight, wallCountDiff * weights.wallCountDiffWeight) +
                    string.Format("averageSubGraphDiff: {0} * {1} = {2}\n", averageSubGraphDiff, weights.averageSubGraphDiffWeight, averageSubGraphDiff * weights.averageSubGraphDiffWeight) +
                    string.Format("winningResultDiff: {0}\n", winningResultDiff) +
                    string.Format("longestSubGraphDiff: {0} * {1} = {2}\n", longestSubGraphDiff, weights.longestSubGraphDiffWeight, longestSubGraphDiff * weights.longestSubGraphDiffWeight);
        }



    }
}
