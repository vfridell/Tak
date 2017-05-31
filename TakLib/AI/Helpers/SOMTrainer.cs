using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace TakLib.AI.Helpers
{
    public class SOMTrainer
    {
        private readonly double _startLearningRate = 0.07;
        private readonly int _numIterations = 500;

        private double _latticeRadius;
        private double _timeConstant;

        public SOMTrainer() { }

        public SOMTrainer(double learnRate, int iterations)
        {
            _startLearningRate = learnRate;
            _numIterations = iterations;
        }

        private double GetNeighborhoodRadius(double iteration)
        {
            return _latticeRadius * Math.Exp(-iteration / _timeConstant);
        }

        private double GetDistanceFallOff(double distSq, double radius)
        {
            double radiusSq = radius * radius;
            return Math.Exp(-(distSq) / (2 * radiusSq));
        }

        // Train the given lattice based on a vector of input vectors
        public void Train(SOMLattice lattice, List<SOMWeightsVector> inputVectorsList, IProgress<int> progressReport, CancellationToken token)
        {
            _latticeRadius = Math.Max(lattice.Height, lattice.Width) / 2;
            _timeConstant = _numIterations / Math.Log(_latticeRadius);
            int iteration = 0;
            double distanceFallOff;
            double learningRate = _startLearningRate;

            while (iteration < _numIterations)
            {
                double neighborhoodRadius = GetNeighborhoodRadius(iteration);
                foreach (SOMWeightsVector currentVector in inputVectorsList)
                {
                    SOMNode bmuNode = lattice.GetBestMatchingUnitNode(currentVector);

                    for (int x = (int)-neighborhoodRadius + bmuNode.X; x <= neighborhoodRadius + bmuNode.X; x++)
                    {
                        int minY = (int)Math.Max(-neighborhoodRadius + bmuNode.Y, -x - neighborhoodRadius);
                        int maxY = (int)Math.Min(neighborhoodRadius + bmuNode.Y, -x + neighborhoodRadius);
                        for (int y = minY; y <= maxY; y++)
                        {
                            if (x < 0 || y < 0) continue;
                            double distance = bmuNode.Distance(lattice.GetNode(x, y));
                            if (distance <= neighborhoodRadius * neighborhoodRadius)
                            {
                                distanceFallOff = GetDistanceFallOff(distance, neighborhoodRadius);
                                lattice.GetNode(x, y).AdjustWeights(currentVector, learningRate, distanceFallOff);
                            }
                            token.ThrowIfCancellationRequested();
                        }
                    }
                }
                iteration++;
                learningRate = _startLearningRate * Math.Exp(-(double)iteration / _numIterations);
                progressReport.Report(iteration / (_numIterations / 100));
            }
        }

    }
}
