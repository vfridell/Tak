using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
                        return MaxValue;
                    case GameResult.BlackFlat:
                    case GameResult.BlackRoad:
                        return MinValue;
                    default:
                        return 0;
                }
            }
        }

        public int whiteAdvantage => winningResultDiff;

        public SimpleAnalysisData() { }

        public override string ToString()
        {
            return $"White Advantage: {whiteAdvantage}";
        }

    }
}
