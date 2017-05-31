using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TakLib.AI.Helpers
{
    [Serializable]
    public class SOMWeightsVector : List<double>
    {
        public double EuclideanDistance(SOMWeightsVector other)
        {
            if (other.Count != Count)
                throw new Exception("Vectors must have the same number of elements");

            double sum = 0;
            for (int i = 0; i < Count; i++)
            {
                sum += (this[i] - other[i]) * (this[i] - other[i]);
            }
            return sum;
        }
    }
}
