using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace TakLib.AI
{
    public class MoveSorter
    {
        public static IDictionary<double, HashSet<NegamaxContext>> GetSortedAnalysisDictionary(Board board, IBoardAnalyzer analyzer)
        {
            var advantageDict = new SortedDictionary<double, HashSet<NegamaxContext>>();
            var allMoves = board.GetAllMoves(true);
            foreach (Move move in allMoves)
            {
                var futureBoard = Board.ComputeFutureBoard(board, move);
                double currentAdvantage = analyzer.Analyze(futureBoard).whiteAdvantage;
                if (!advantageDict.ContainsKey(currentAdvantage))
                    advantageDict.Add(currentAdvantage, new HashSet<NegamaxContext>());
                advantageDict[currentAdvantage].Add(new NegamaxContext(move, futureBoard, currentAdvantage, true));
            }
            return advantageDict;
        }

        public static IDictionary<double, HashSet<AnalysisFactors>> GetSortedAnalysisFactorsDictionary(Board board, MaximumRatioAnalyzer analyzer)
        {
            var advantageDict = new SortedDictionary<double, HashSet<AnalysisFactors>>();
            var allMoves = board.GetAllMoves(true);
            foreach (Move move in allMoves)
            {
                var futureBoard = Board.ComputeFutureBoard(board, move);
                double currentAdvantage = analyzer.Analyze(futureBoard).whiteAdvantage;
                if (!advantageDict.ContainsKey(currentAdvantage))
                    advantageDict.Add(currentAdvantage, new HashSet<AnalysisFactors>());
                advantageDict[currentAdvantage].Add(analyzer.GetCurrentAnalysisFactors);
            }
            return advantageDict;
        }
    }
}
