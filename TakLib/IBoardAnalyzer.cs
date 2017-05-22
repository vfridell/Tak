namespace TakLib
{
    public interface IBoardAnalyzer
    {
        IAnalysisResult Analyze(Board board, BoardAnalysisWeights weights);
    }
}