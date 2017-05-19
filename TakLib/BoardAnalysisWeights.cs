using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TakLib
{
    public struct BoardAnalysisWeights
    {
        public double capStoneDiffWeight;
        public double flatScoreWeight;
        public double possibleMovesDiffWeight;
        public double wallCountDiffWeight;
        public double averageSubGraphDiffWeight;
        public double movementPlacementDiffWeight;
        public double longestSubGraphDiffWeight;
        public double numberOfSubGraphsDiffWeight;

        public static BoardAnalysisWeights bestWeights = new BoardAnalysisWeights()
        {
            capStoneDiffWeight = .1,
            flatScoreWeight = 100.0,
            possibleMovesDiffWeight = .1,
            wallCountDiffWeight = 2.0,
            averageSubGraphDiffWeight = 10.0,
            longestSubGraphDiffWeight = 10.0,
            numberOfSubGraphsDiffWeight = .1,
        };

        public static BoardAnalysisWeights testingWeights = new BoardAnalysisWeights()
        {
            capStoneDiffWeight = .1,
            flatScoreWeight = 50.0,
            possibleMovesDiffWeight = .1,
            wallCountDiffWeight = 2.0,
            averageSubGraphDiffWeight = 10.0,
            longestSubGraphDiffWeight = 50.0,
            numberOfSubGraphsDiffWeight = .1,
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
