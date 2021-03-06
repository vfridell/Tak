﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TakLib.AI.Helpers;

namespace TakLib
{
    public class BoardStacksAnalysis : IAnalysisResult
    {
        public static readonly double MaxValue = int.MaxValue - 10;
        public static readonly double MinValue = int.MinValue + 10;

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

        public int whiteStackAdvantage;
        public int blackStackAdvantage;
        public int stacksAdvantageDiff => whiteStackAdvantage - blackStackAdvantage;

        public GameResult gameResult { get; set; }
        public double winningResultDiff
        {
            get
            {
                switch (gameResult)
                {
                    case GameResult.WhiteFlat:
                    case GameResult.WhiteRoad:
                        return MaxValue;
                    case GameResult.BlackFlat:
                    case GameResult.BlackRoad:
                        return MinValue;
                    default:
                        return 0;
                }
            }
        }

        public BoardAnalysisWeights weights;

        public double whiteAdvantage { get; set; }
        public BoardStacksAnalysis() { }

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


        public SOMWeightsVector GetSomWeightsVector()
        {
            SOMWeightsVector vector = new SOMWeightsVector();
            vector.Add(flatScore);
            vector.Add(averageSubGraphDiff);
            vector.Add(longestSubGraphDiff);
            vector.Add(capStoneDiff);
            vector.Add(numberOfSubGraphsDiff);
            vector.Add(wallCountDiff);
            //vector.Add(possibleMovesDiff);
            vector.Add(winningResultDiff);
            return vector;
        }
    }
}
