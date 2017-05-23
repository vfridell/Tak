﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TakLib
{
    public struct BoardAnalysisWeights
    {
        public int capStoneDiffWeight;
        public int flatScoreWeight;
        public int possibleMovesDiffWeight;
        public int wallCountDiffWeight;
        public int averageSubGraphDiffWeight;
        public int movementPlacementDiffWeight;
        public int longestSubGraphDiffWeight;
        public int numberOfSubGraphsDiffWeight;

        public static BoardAnalysisWeights bestWeights = new BoardAnalysisWeights()
        {
            capStoneDiffWeight = 1,
            flatScoreWeight = 100,
            possibleMovesDiffWeight = 1,
            wallCountDiffWeight = 2,
            averageSubGraphDiffWeight = 10,
            longestSubGraphDiffWeight = 10,
            numberOfSubGraphsDiffWeight = 1,
        };

        public static BoardAnalysisWeights testingWeights = new BoardAnalysisWeights()
        {
            capStoneDiffWeight = 1,
            flatScoreWeight = 50,
            possibleMovesDiffWeight = 1,
            wallCountDiffWeight = 2,
            averageSubGraphDiffWeight = 10,
            longestSubGraphDiffWeight = 50,
            numberOfSubGraphsDiffWeight = 1,
        };

        public static BoardAnalysisWeights zeroWeights = new BoardAnalysisWeights()
        {
            capStoneDiffWeight = 0,
            flatScoreWeight = 0,
            possibleMovesDiffWeight = 0,
            wallCountDiffWeight = 0,
            averageSubGraphDiffWeight = 0,
            longestSubGraphDiffWeight = 0,
            numberOfSubGraphsDiffWeight = 0,
        };

        public override string ToString()
        {
            return string.Format("capStoneDiffWeight: {0} \n", capStoneDiffWeight) +
                    string.Format("flatScoreWeight: {0} \n", flatScoreWeight) +
                    string.Format("possibleMovesDiffWeight: {0} \n", possibleMovesDiffWeight) +
                    string.Format("wallCountDiffWeight: {0} \n", wallCountDiffWeight) +
                    string.Format("averageSubGraphDiffWeight: {0} \n", averageSubGraphDiffWeight) +
                    string.Format("numberOfSubGraphsDiffWeight: {0} \n", numberOfSubGraphsDiffWeight) +
                    string.Format("longestSubGraphDiffWeight: {0} \n", longestSubGraphDiffWeight);
        }
    }
}
