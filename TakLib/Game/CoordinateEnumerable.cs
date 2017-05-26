using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TakLib
{
    public class CoordinateEnumerable : IEnumerable<Coordinate>
    {
        public int BoardSize { get; }

        public CoordinateEnumerable(int boardSize)
        {
            BoardSize = boardSize;
        }

        public IEnumerator<Coordinate> GetEnumerator()
        {
            for (int r = 0; r < BoardSize; r++)
                for (int c = 0; c < BoardSize; c++)
                    yield return new Coordinate(r,c);
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
