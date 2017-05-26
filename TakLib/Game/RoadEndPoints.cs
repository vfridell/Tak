using System;
using System.Collections.Generic;

namespace TakLib
{
    public class RoadEndPoints : Dictionary<Coordinate, HashSet<Coordinate>>
    {
        private static List<RoadEndPoints> _allEndPoints = new List<RoadEndPoints>();

        static RoadEndPoints()
        {
            _allEndPoints.Add(null);
            _allEndPoints.Add(null);
            _allEndPoints.Add(null);
            for(int i=3; i<=8; i++)
                _allEndPoints.Add(new RoadEndPoints(i));
        }

        public static RoadEndPoints GetRoadEndPoints(int boardSize)
        {
            if (boardSize < 3 || boardSize > 8) throw new ArgumentException($"Invalid boardSize {boardSize}");
            return _allEndPoints[boardSize];
        }

        public int BoardSize { get; protected set; }
        public int MaxCoord => BoardSize - 1;

        public RoadEndPoints(int boardSize)
        {
            BoardSize = boardSize;
            for (int i = 0; i < MaxCoord; i++)
            {
                Coordinate leftSide = new Coordinate(i, 0);
                Coordinate rightSide = new Coordinate(MaxCoord - i, MaxCoord);
                Coordinate topSide = new Coordinate(MaxCoord, i);
                Coordinate bottomSide = new Coordinate(0, MaxCoord - i);

                Add(leftSide, new HashSet<Coordinate>());
                Add(rightSide, new HashSet<Coordinate>());
                Add(topSide, new HashSet<Coordinate>());
                Add(bottomSide, new HashSet<Coordinate>());

                for (int j = 0; j < BoardSize; j++)
                {
                    this[leftSide].Add(new Coordinate(MaxCoord - j, MaxCoord));
                    if (IsCorner(leftSide)) this[leftSide].Add(new Coordinate(MaxCoord, j));

                    this[rightSide].Add(new Coordinate(j, 0));
                    if (IsCorner(rightSide)) this[rightSide].Add(new Coordinate(0, MaxCoord - j));

                    this[topSide].Add(new Coordinate(0, MaxCoord - j));
                    if (IsCorner(topSide)) this[topSide].Add(new Coordinate(MaxCoord - j, MaxCoord));

                    this[bottomSide].Add(new Coordinate(MaxCoord, j));
                    if (IsCorner(bottomSide)) this[bottomSide].Add(new Coordinate(j, 0));
                }
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
