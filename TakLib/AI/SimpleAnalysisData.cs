using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TakLib.AI.Helpers;

namespace TakLib
{
    public class SimpleAnalysisData : IAnalysisResult
    {
        public static readonly double MaxValue = double.MaxValue;
        public static readonly double MinValue = double.MinValue;

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

        public double whiteAdvantage
        {
            get { return winningResultDiff; }
            set { }
        }

        public SimpleAnalysisData() { }

        public override string ToString()
        {
            return $"White Advantage: {whiteAdvantage}";
        }

        public SOMWeightsVector GetSomWeightsVector()
        {
            throw new NotImplementedException("SimpleAnalyzer not suitable for SOM analysis");
        }
    }
}
