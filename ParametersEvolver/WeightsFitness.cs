using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GeneticSharp.Domain.Chromosomes;
using GeneticSharp.Domain.Fitnesses;
using TakLib;

namespace ParametersEvolver
{
    public class WeightsFitness : IFitness
    {
        private TrialSet _trialSet;
        private ITakAI _ai;

        public WeightsFitness(TrialSet trialSet, ITakAI ai)
        {
            _trialSet = trialSet;
            _ai = ai;
        }

        public double Evaluate(IChromosome chromosome)
        {
            var weightsChromosome = chromosome as WeightsChromosome;
            if (null == weightsChromosome) throw new ArgumentException("chromosome must be of type WeightsChromosome");
            BoardAnalysisWeights weights = weightsChromosome.GetWeights();
            return _trialSet.RunTrials(_ai);
        }


    }
}
