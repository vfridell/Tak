using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TakLib
{
    public class AnalysisFactors : ConcurrentDictionary<string, AnalysisFactor>
    {
        public AnalysisFactors(Dictionary<string, Tuple<double, double>> weightsGrowthDictionary)
        {
            foreach (var kvp in weightsGrowthDictionary)
            {
                GetOrAdd(kvp.Key, new AnalysisFactor() {Name = kvp.Key, Weight = kvp.Value.Item1, GrowthRate = kvp.Value.Item2});
            }
        }

        private static double Activation(double turnNumber, double growthRate)
        {
            if (growthRate >= 0 && growthRate <= .1) return 0;
            if (growthRate < 0 && growthRate >= -.1) return 1;
            double Q = growthRate >= 0 ? 10000d : .0001;
            return 1d / (1d + Math.Exp(-growthRate * turnNumber) * Q);
        }

        public double CalculateAdvantage(int turnNumber)
        {
            double result = 0;
            foreach (var kvp in this)
            {
                if (kvp.Key != kvp.Value.Name) throw new Exception($"Invalid factor: Name = {kvp.Key}, FactorName = {kvp.Value.Name}");
                result += kvp.Value.Value * kvp.Value.Weight * Activation(turnNumber, kvp.Value.GrowthRate);
            }
            return result;
        }

        public void Validate()
        {
            foreach (var kvp in this)
            {
                if(kvp.Key != kvp.Value.Name) throw new Exception($"Invalid factor: Name = {kvp.Key}, FactorName = {kvp.Value.Name}");
            }
        }


        public static AnalysisFactors GetBestAnalysisFactorsTemplate()
        {
            Dictionary<string, Tuple<double, double>> bestWeightsGrowth = new Dictionary<string, Tuple<double, double>>()
            {
                {"capStoneDiff", new Tuple<double, double>(1, 1)},
                {"flatScore", new Tuple<double, double>(86, 1)},
                {"possibleMovesDiff", new Tuple<double, double>(0, 0)},
                {"wallCountDiff", new Tuple<double, double>(-5, 1)},
                {"averageSubGraphDiff", new Tuple<double, double>(15, 1)},
                {"longestSubGraphDiff", new Tuple<double, double>(15, 1)},
                {"numberOfSubGraphsDiff", new Tuple<double, double>(1, 1)},
                {"stacksAdvantageDiff", new Tuple<double, double>(10, 1)},
            };
            return new AnalysisFactors(bestWeightsGrowth);
        }
    }
}
