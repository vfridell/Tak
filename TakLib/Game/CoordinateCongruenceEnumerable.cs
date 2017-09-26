using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TakLib
{
    public class CoordinateCongruenceEnumerable : IEnumerable<CongruencePair>
    {
        public int BoardSize { get; }
        public int IndexLimit { get; }

        public CoordinateCongruenceEnumerable(int boardSize)
        {
            BoardSize = boardSize;
            IndexLimit = (int)Math.Floor((BoardSize - 1d) / 2d);
        }

        public IEnumerator<CongruencePair> GetEnumerator()
        {
            for (int r = 0; r < BoardSize; r++)
            {
                for (int c = 0; c < BoardSize; c++)
                {
                    // (1) mirror 1 horizontal line
                    if(c < IndexLimit) yield return new CongruencePair(new Coordinate(r,c), new Coordinate(r, BoardSize - (c + 1)), 1);
                    // (2) mirror 2 vertical line
                    if(r < IndexLimit) yield return new CongruencePair(new Coordinate(r,c), new Coordinate(BoardSize - (r + 1), c), 2); 
                    // (3) mirror 3 line bottom left to upper right
                    if(r > c) yield return new CongruencePair(new Coordinate(r,c), new Coordinate(c,r), 3);
                    // (4) mirror 4 line upper left to bottom right
                    if(c + r + 1 < BoardSize) yield return new CongruencePair(new Coordinate(r,c), new Coordinate((BoardSize - (r + c + 1)) + r, (BoardSize - (r + c + 1)) + c), 4);
                    // (5) rotate 90 degrees
                    // (6) rotate 180 degrees
                    if (r + c < BoardSize - 1 && c <= r)
                    {
                        yield return new CongruencePair(new Coordinate(r,c), new Coordinate((BoardSize-1)-c, r), 5);
                        yield return new CongruencePair(new Coordinate(r,c), new Coordinate((BoardSize-1)-r, (BoardSize-1)-c), 6);
                    }
                }
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }


    }

    public struct CongruencePair
    {
        public CongruencePair(Coordinate c1, Coordinate c2, int group)
        {
            C1 = c1;
            C2 = c2;
            Group = group;
        }

        public Coordinate C1 { get; }
        public Coordinate C2 { get; }
        public int Group { get; }

    }
}
