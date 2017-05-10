using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TakLib
{
    public class RoadFinder
    {
        public readonly int BoardSize;
        public int MaxCoord => BoardSize - 1;

        public Dictionary<Coordinate, List<Coordinate>> DestinationsDictionary = new Dictionary <Coordinate, List<Coordinate>>()
            { [new Coordinate(0,0)] = new List<Coordinate>() {new Coordinate(0,4)},  };

        public RoadFinder(int boardSize)
        {
            BoardSize = boardSize;
            DestinationsDictionary = new Dictionary<Coordinate, List<Coordinate>>();
            for (int r = 0; r < BoardSize; r++)
            {

                if(IsCorner())
            }
        }

        public bool IsCorner(Coordinate coordinate)
        {
            return (coordinate.Row == 0 || coordinate.Row == MaxCoord) &&
                    (coordinate.Column == 0 || coordinate.Column == MaxCoord);
        }

        public bool IsSide(Coordinate coordinate)
        {
            return coordinate.Row == 0 || 
                   coordinate.Column == 0 ||
                   coordinate.Row == MaxCoord || 
                   coordinate.Column == MaxCoord;
        }
    }
}
