using TakLib.AI.Helpers;

namespace TakLib
{
    public interface IAnalysisResult
    {
        int whiteAdvantage { get; }
        GameResult gameResult { get; }
        SOMWeightsVector GetSomWeightsVector();
    }
}