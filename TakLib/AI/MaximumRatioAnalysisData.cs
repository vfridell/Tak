using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TakLib.AI.Helpers;

namespace TakLib
{
    public class MaximumRatioAnalysisData : IAnalysisResult
    {
        public static readonly double MaxValue = double.MaxValue;
        public static readonly double MinValue = double.MinValue;


        public double whiteAdvantage { get; set; }
        public GameResult gameResult { get; set; }
        public SOMWeightsVector GetSomWeightsVector()
        {
            throw new NotImplementedException();
        }

        public virtual double winningResultDiff
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
    }
}
