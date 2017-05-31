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
        private int _height;
        private int _width;
        private int _numWeights;
        private double[] _maxWeights;
        private double[] _minWeights;

        public int Height => _height;
        public int Width => _width;
        public int NumWeights => _numWeights;

        public double MaxWeight(int i) => _maxWeights[i];
        public double MinWeight(int i) => _minWeights[i];

        public SOMLattice(int height, int width, int numWeights)
        {
            _nodes = new SOMNode[width, height];
            _height = height;
            _width = width;
            _numWeights = numWeights;
            _maxWeights = new double[_numWeights];
            _minWeights = new double[_numWeights];
        }

        public void Initialize()
        {
            for (int x = 0; x < _height; x++)
            {
                for (int y = 0; y < _width; y++)
                {
                    _nodes[x, y] = new SOMNode(_numWeights, x, y);
                    AdjustMaxMinWeights(_nodes[x, y]);
                }
            }
        }

        public void InitializeTest()
        {
            double xstep = .5f / (float)_width;
            double ystep = .5f / (float)_height;
            for (int x = 0; x < _height; x++)
            {
                for (int y = 0; y < _width; y++)
                {
                    _nodes[x, y] = new SOMNode(_numWeights, x, y);
                    _nodes[x, y].SetWeight(0, (xstep * x) + (ystep * y));
                    _nodes[x, y].SetWeight(1, (xstep * x) + (ystep * y));
                    _nodes[x, y].SetWeight(2, (xstep * x) + (ystep * y));

                    AdjustMaxMinWeights(_nodes[x, y]);
                }
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
            return _nodes[x, y];
        }

        public SOMNode GetBestMatchingUnitNode(SOMWeightsVector input)
        {
            SOMNode bmuNode = _nodes[0, 0];
            double bestDistance = input.EuclideanDistance(bmuNode.WeightsVector);
            double currentDistance;

            for (int x = 0; x < _height; x++)
            {
                for (int y = 0; y < _width; y++)
                {
                    currentDistance = input.EuclideanDistance(_nodes[x, y].WeightsVector);
                    if (currentDistance < bestDistance)
                    {
                        bmuNode = _nodes[x, y];
                        bestDistance = currentDistance;
                    }
                }
            }
            return bmuNode;
        }

        public void AdjustWeights(int x, int y, SOMWeightsVector input, double learningRate, double distanceFallOff)
        {
            GetNode(x, y).AdjustWeights(input, learningRate, distanceFallOff);
            AdjustMaxMinWeights(GetNode(x, y));
        }

        public static string WriteLatticeData(SOMLattice lattice)
        {
            string filename = $"lattice_{lattice.Height}X{lattice.Width}_{DateTime.Now.ToString("yyyy.MM.dd.HHmmss")}";
            BinaryFormatter formatter = new BinaryFormatter();
            using (Stream stream = new FileStream(filename + ".bin", FileMode.Create, FileAccess.Write, FileShare.None))
            {
                formatter.Serialize(stream, lattice);
            }
            return filename;
        }
    }
}
