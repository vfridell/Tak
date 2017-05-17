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
        public double unplayedPiecesDiffWeight;
        public double queenPlacementDiffWeight;
        public double movementPlacementDiffWeight;
        public double ownedBeetleStacksWeight;

        public static BoardAnalysisWeights startingWeights = new BoardAnalysisWeights()
        {
            capStoneDiffWeight = 1.5,
            flatScoreWeight = 0.5,
            possibleMovesDiffWeight = 1.0,
            wallCountDiffWeight = 2.0,
            unplayedPiecesDiffWeight = 1.0,
            queenPlacementDiffWeight = 100.0,
            movementPlacementDiffWeight = 0,
            ownedBeetleStacksWeight = 0,
        };

        public override string ToString()
        {
            return string.Format("capStoneDiffWeight: {0} \n", capStoneDiffWeight) +
                    string.Format("flatScoreWeight: {0} \n", flatScoreWeight) +
                    string.Format("possibleMovesDiffWeight: {0} \n", possibleMovesDiffWeight) +
                    string.Format("wallCountDiffWeight: {0} \n", wallCountDiffWeight) +
                    string.Format("unplayedPiecesDiffWeight: {0} \n", unplayedPiecesDiffWeight) +
                    string.Format("queenPlacementDiffWeight: {0} \n", queenPlacementDiffWeight) +
                    string.Format("ownedBeetleStacksDiffWeight: {0} \n", ownedBeetleStacksWeight) +
                    string.Format("movementPlacementAdvantageDiffWeight: {0} \n", movementPlacementDiffWeight);
        }
    }
}
