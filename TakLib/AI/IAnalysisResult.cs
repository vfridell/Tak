using TakLib.AI.Helpers;

namespace TakLib
{
    public interface IAnalysisResult
    {
        double whiteAdvantage { get; set; }
        GameResult gameResult { get; }
        SOMWeightsVector GetSomWeightsVector();
    }
}