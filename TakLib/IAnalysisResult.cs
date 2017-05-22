namespace TakLib
{
    public interface IAnalysisResult
    {
        double whiteAdvantage { get; }
        GameResult gameResult { get; }
    }
}