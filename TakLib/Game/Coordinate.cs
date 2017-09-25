using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TakLib
{
    [Serializable]
    public struct Coordinate
    {
        public Coordinate(int row, int column)
        {
            Row = row;
            Column = column;
        }
        public Coordinate(char row, char column)
        {
            Row = NotationParser.ToRow(row);
            Column = NotationParser.ToColumn(column);
        }
        public int Row;
        public int Column;

        public Coordinate GetNeighbor(Direction direction) => this + NotationParser.ToCoordinateDirection(direction);
        public char RowChar => NotationParser.ToCharRow(this);
        public char ColumnChar => NotationParser.ToCharColumn(this);
        public override string ToString() => $"{ColumnChar}{RowChar} ({Row},{Column})";

        public static Coordinate operator +(Coordinate c1, Coordinate c2) => new Coordinate(c1.Row + c2.Row, c1.Column + c2.Column);
        public static bool operator ==(Coordinate c1, Coordinate c2) => c1.Row == c2.Row && c1.Column == c2.Column;
        public static bool operator !=(Coordinate c1, Coordinate c2) => c1.Row != c2.Row || c1.Column != c2.Column;


        public static Coordinate MirrorCenter(Coordinate c, int mirrorLine)
        {
            switch (mirrorLine)
            {
                case 1:
                    return new Hex(-hex.x, -hex.z, -hex.y);
                case 2:
                    return new Hex(hex.y, hex.x, hex.z);
                case 3:
                    return new Hex(-hex.z, -hex.y, -hex.x);
                case 4:
                    return new Hex(hex.x, hex.z, hex.y);
                default:
                    throw new Exception($"Bad mirror line: {mirrorLine}");
            }
        }

        public static Hex Rotate60DegreesClockwiseHex(Hex hex) => new Hex(-hex.z, -hex.x, -hex.y);
        public static Hex Rotate60DegreesClockwiseHex(Hex hex, int times)
        {
            Hex returnHex = hex;
            for (int i = 0; i < times; i++) returnHex = new Hex(-returnHex.z, -returnHex.x, -returnHex.y);
            return returnHex;
        }
        public static Hex Rotate60DegreesCounterClockwiseHex(Hex hex) => new Hex(-hex.y, -hex.z, -hex.x);

    }
}
}
