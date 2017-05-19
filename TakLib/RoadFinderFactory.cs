using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TakLib
{
    public class RoadFinderFactory
    {
        private static Dictionary<int, RoadFinder> _finders = new Dictionary<int, RoadFinder>();

        public static RoadFinder GetRoadFinder(int boardSize)
        {
            RoadFinder finder;
            _finders.TryGetValue(boardSize, out finder);
            if (finder == null)
            {
                finder = new RoadFinder(boardSize);
                _finders.Add(boardSize, finder);
            }
            return finder;
        }
    }
}
