using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TakLib.AI.Helpers
{
    [Serializable]
    public class SOMNode
    {
        private static Random _rand = new Random();
        private SOMWeightsVector _weightsVector;
        private int _x, _y;

        public int X => _x;
        public int Y => _y;
        public SOMWeightsVector WeightsVector => _weightsVector;

        public SOMNode(int numWeights, int x, int y)
        {
            _x = x;
            _y = y;
            _weightsVector = new SOMWeightsVector();
            for (int i = 0; i < numWeights; i++)
            {
                _weightsVector.Add(_rand.Next(0, 99));
            }
        }

        public double Distance(SOMNode other) => (X - other.X) * (X - other.X) + (Y - other.Y) * (Y - other.Y);

        public void SetWeight(int n, double value)
        {
            if (n > _weightsVector.Count) throw new ArgumentOutOfRangeException($"Index {n} does not exist in a node with {_weightsVector.Count} weights");
            _weightsVector[n] = value;
        }

        public double GetWeight(int n)
        {
            if (n > _weightsVector.Count) throw new ArgumentOutOfRangeException($"Index {n} does not exist in a node with {_weightsVector.Count} weights");
            return _weightsVector[n];
        }

        public void AdjustWeights(SOMWeightsVector input, double learningRate, double distanceFallOff)
        {
            for (int i = 0; i < _weightsVector.Count; i++)
            {
                _weightsVector[i] = _weightsVector[i] + distanceFallOff * learningRate * (input[i] - _weightsVector[i]);
            }
        }
    }

}
