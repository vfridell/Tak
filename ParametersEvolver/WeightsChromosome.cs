using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GeneticSharp.Domain.Chromosomes;
using GeneticSharp.Domain.Fitnesses;
using GeneticSharp.Domain.Randomizations;
using TakLib;

namespace ParametersEvolver
{
    public class WeightsChromosome : ChromosomeBase
    {
        public WeightsChromosome() : base(8)
        {
            for (int i = 0; i < Length; i++)
            {
                ReplaceGene(i, GenerateGene(i));
            }
        }

        public override IChromosome CreateNew()
        {
            return new WeightsChromosome();
        }

        public override Gene GenerateGene(int geneIndex)
        {
            return new Gene(RandomizationProvider.Current.GetDouble(-100, 100));
        }

        public BoardAnalysisWeights GetWeights()
        {
            return new BoardAnalysisWeights()
            {
                capStoneDiffWeight = (double)GetGene(0).Value,
                flatScoreWeight = (double)GetGene(1).Value,
                possibleMovesDiffWeight = (double)GetGene(2).Value,
                wallCountDiffWeight = (double)GetGene(3).Value,
                averageSubGraphDiffWeight = (double)GetGene(4).Value,
                longestSubGraphDiffWeight = (double)GetGene(5).Value,
                numberOfSubGraphsDiffWeight = (double)GetGene(6).Value,
                stacksAdvantageDiffWeight = (double)GetGene(7).Value,
            };
        }
    }


}
