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
        private AnalysisFactors() { }

        // If growthRate is negative, the weight applies in full on turn 1 and becomes less important toward the last turn
        // If positive, the weight applies hardly at all in the beginning and fully at endgame
        // Greater negative numbers make the weight irrelevant sooner
        // Greater positive numbers make the weight relevant sooner
        // Around +-0.5 there is an S-curve beginning at turn 10 and ending at turn 26
        // Values in excess of +-5 will not produce much weight variance over the length of the game
        private static double Activation(double turnNumber, double growthRate)
        {
            if (growthRate >= 0 && growthRate <= .1) return 0;
            if (growthRate < 0 && growthRate >= -.1) return 1;
            double Q = growthRate >= 0 ? 10000d : .0001;
            return 1d / (1d + Math.Exp(-growthRate * turnNumber) * Q);
        }

        public double CalculateNamedFactor(string name, int turnNumber)
        {
            AnalysisFactor factor;
            if(!TryGetValue(name, out factor)) throw new Exception($"Invalid factor {name}");
            return CalculateNamedFactor(factor, turnNumber);
        }

        public double CalculateNamedFactor(AnalysisFactor factor, int turnNumber)
        {
            factor.CalculatedResult = factor.Value * factor.Weight * Activation(turnNumber, factor.GrowthRate);
            return factor.CalculatedResult;
        }

        public double CalculateAdvantage(int turnNumber)
        {
            double result = 0;
            foreach (var kvp in this)
            {
                if (kvp.Key != kvp.Value.Name) throw new Exception($"Invalid factor: Name = {kvp.Key}, FactorName = {kvp.Value.Name}");
                result += CalculateNamedFactor(kvp.Value, turnNumber);
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

        public AnalysisFactors Clone()
        {
            AnalysisFactors clone = new AnalysisFactors();
            foreach (var kvp in this)
            {
                clone.GetOrAdd(kvp.Key, new AnalysisFactor()
                {
                    Name = kvp.Key,
                    Value = kvp.Value.Value,
                    Weight = kvp.Value.Weight,
                    GrowthRate = kvp.Value.GrowthRate,
                    CalculatedResult = kvp.Value.CalculatedResult,
                });
            }
            return clone;
        }


    }
}
