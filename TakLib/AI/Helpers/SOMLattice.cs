using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;

namespace TakLib.AI.Helpers
{
    [Serializable]
    public class SOMLattice
    {
        private SOMNode[,] _nodes;
        private int _size;
        private int _numWeights;
        private double[] _maxWeights;
        private double[] _minWeights;

        public int Size => _size;
        public int NumWeights => _numWeights;
        public readonly IBoardAnalyzer Analyzer;

        public double MaxWeight(int i) => _maxWeights[i];
        public double MinWeight(int i) => _minWeights[i];

        public SOMLattice(int size, int numWeights, IBoardAnalyzer analyzer)
        {
            _nodes = new SOMNode[size, size];
            _size = size;

            _numWeights = numWeights;
            _maxWeights = new double[_numWeights];
            _minWeights = new double[_numWeights];
            Analyzer = analyzer;
        }

        public void Initialize()
        {
            foreach(Coordinate c in new CoordinateEnumerable(_size))
            {
                _nodes[c.Row, c.Column] = new SOMNode(_numWeights, c);
                AdjustMaxMinWeights(_nodes[c.Row, c.Column]);
            }
        }

        public void InitializeTest()
        {
            double step = .5f / (float)_size;
            foreach (Coordinate c in new CoordinateEnumerable(_size))
            {
                _nodes[c.Row, c.Column] = new SOMNode(_numWeights, c);
                _nodes[c.Row, c.Column].SetWeight(0, (step * c.Row) + (step * c.Column));
                _nodes[c.Row, c.Column].SetWeight(1, (step * c.Row) + (step * c.Column));
                _nodes[c.Row, c.Column].SetWeight(2, (step * c.Row) + (step * c.Column));

                AdjustMaxMinWeights(_nodes[c.Row, c.Column]);
            }
        }

        private void AdjustMaxMinWeights(SOMNode node)
        {
            for (int i = 0; i < _numWeights; i++)
            {
                _minWeights[i] = Math.Min(_minWeights[i], node.GetWeight(i));
                _maxWeights[i] = Math.Max(_maxWeights[i], node.GetWeight(i));
            }
        }

        public SOMNode GetNode(int x, int y)
        {
            //Coordinate c = new Coordinate(y, x);
            return _nodes[y, x];
        }

        public SOMNode GetNode(Coordinate c)
        {
            return _nodes[c.Row, c.Column];
        }

        public SOMNode GetBestMatchingUnitNode(SOMWeightsVector input)
        {
            SOMNode bmuNode = _nodes[0, 0];
            double bestDistance = input.EuclideanDistance(bmuNode.WeightsVector);
            double currentDistance;

            foreach (Coordinate c in new CoordinateEnumerable(_size))
            {
                currentDistance = input.EuclideanDistance(_nodes[c.Row, c.Column].WeightsVector);
                if (currentDistance < bestDistance)
                {
                    bmuNode = _nodes[c.Row, c.Column];
                    bestDistance = currentDistance;
                }
            }
            return bmuNode;
        }

        public void AdjustWeights(Coordinate c, SOMWeightsVector input, double learningRate, double distanceFallOff)
        {
            GetNode(c).AdjustWeights(input, learningRate, distanceFallOff);
            AdjustMaxMinWeights(GetNode(c));
        }

        public static string WriteLatticeData(SOMLattice lattice)
        {
            string filename = $"lattice_{lattice.Size}X{lattice.Size}_{DateTime.Now.ToString("yyyy.MM.dd.HHmmss")}";
            BinaryFormatter formatter = new BinaryFormatter();
            using (Stream stream = new FileStream(filename + ".bin", FileMode.Create, FileAccess.Write, FileShare.None))
            {
                formatter.Serialize(stream, lattice);
            }
            return filename;
        }
    }
}
