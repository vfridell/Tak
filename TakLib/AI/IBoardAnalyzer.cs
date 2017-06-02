using TakLib.AI.Helpers;

namespace TakLib
{
    public interface IBoardAnalyzer
    {
        IAnalysisResult Analyze(Board boards);
        SOMWeightsVector GetSomWeightsVector(Board board);
    }
}