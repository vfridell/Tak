using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TakLib
{
    [Serializable]
    public struct BoardAnalysisWeights
    {
        public double capStoneDiffWeight;
        public double flatScoreWeight;
        public double possibleMovesDiffWeight;
        public double wallCountDiffWeight;
        public double averageSubGraphDiffWeight;
        //public double movementPlacementDiffWeight;
        public double longestSubGraphDiffWeight;
        public double numberOfSubGraphsDiffWeight;
        public double stacksAdvantageDiffWeight;

        public static BoardAnalysisWeights bestWeights = new BoardAnalysisWeights()
        {
            capStoneDiffWeight = 1,
            flatScoreWeight = 100,
            possibleMovesDiffWeight = 1,
            wallCountDiffWeight = 2,
            averageSubGraphDiffWeight = 10,
            longestSubGraphDiffWeight = 10,
            numberOfSubGraphsDiffWeight = 1,
            stacksAdvantageDiffWeight = 0,
        };

        public static BoardAnalysisWeights bestStackWeights = new BoardAnalysisWeights()
        {
            capStoneDiffWeight = 1,
            flatScoreWeight = 86,
            possibleMovesDiffWeight = 0,
            wallCountDiffWeight = 5,
            averageSubGraphDiffWeight = 15,
            longestSubGraphDiffWeight = 15,
            numberOfSubGraphsDiffWeight = 1,
            stacksAdvantageDiffWeight = 10,
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
            stacksAdvantageDiffWeight = 0,
        };



        public override string ToString()
        {
            return string.Format("capStoneDiffWeight: {0} \n", capStoneDiffWeight) +
                    string.Format("flatScoreWeight: {0} \n", flatScoreWeight) +
                    string.Format("possibleMovesDiffWeight: {0} \n", possibleMovesDiffWeight) +
                    string.Format("wallCountDiffWeight: {0} \n", wallCountDiffWeight) +
                    string.Format("averageSubGraphDiffWeight: {0} \n", averageSubGraphDiffWeight) +
                    string.Format("numberOfSubGraphsDiffWeight: {0} \n", numberOfSubGraphsDiffWeight) +
                    string.Format("longestSubGraphDiffWeight: {0} \n", longestSubGraphDiffWeight) +
                    string.Format("stacksAdvantageDiffWeight: {0} \n", stacksAdvantageDiffWeight);
        }
    }
}
