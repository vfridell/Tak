using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TakLib
{
    public class MaximumRatioAnalysisData : BoardAnalysisData
    {
        public int BlackCornerSpacesOwned { get; set; }
        public int WhiteCornerSpacesOwned { get; set; }
        public int BlackEdgeSpacesOwned { get; set; }
        public int WhiteEdgeSpacesOwned { get; set; }
        public int CenterTerritoryBlack { get; set; }
        public int CenterTerritoryWhite { get; set; }

        //public double 
    }
}
