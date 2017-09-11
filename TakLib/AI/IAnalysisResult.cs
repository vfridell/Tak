using TakLib.AI.Helpers;

namespace TakLib
{
    public interface IAnalysisResult
    {
        int whiteAdvantage { get; set; }
        GameResult gameResult { get; }
        SOMWeightsVector GetSomWeightsVector();
    }
}