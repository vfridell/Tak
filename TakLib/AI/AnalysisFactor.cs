using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TakLib
{
    public class AnalysisFactor
    {
        public string Name { get; set; }
        public double Value { get; set;}
        public double Weight { get; set; }
        public double GrowthRate { get; set; }
        public double CalculatedResult { get; set; }
    }
}
