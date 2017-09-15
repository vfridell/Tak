using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GeneticSharp.Domain.Chromosomes;
using TakLib;

namespace ParametersEvolver
{
    public class WeightsChromosome : BinaryChromosomeBase
    {
        public WeightsChromosome(int length, BoardAnalysisWeights weights) : base(length)
        {
            Gene newGene = new Gene(weights.longestSubGraphDiffWeight);
        }

        public override IChromosome CreateNew()
        {
            throw new NotImplementedException();
        }
    }
}
