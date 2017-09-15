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
        public static readonly int MaxValue = int.MaxValue - 10;
        public static readonly int MinValue = int.MinValue + 10;

        public GameResult gameResult { get; set; }
        public int winningResultDiff
        {
            get
            {
                switch (gameResult)
                {
                    case GameResult.WhiteFlat:
                    case GameResult.WhiteRoad:
                    case GameResult.BlackFlat:
                    case GameResult.BlackRoad:
                        return MaxValue;
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
