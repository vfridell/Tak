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
        public static int Distance(Coordinate c1, Coordinate c2) => Math.Abs(c1.Row - c2.Row) + Math.Abs(c1.Column - c2.Column);
        public static bool operator ==(Coordinate c1, Coordinate c2) => c1.Row == c2.Row && c1.Column == c2.Column;
        public static bool operator !=(Coordinate c1, Coordinate c2) => c1.Row != c2.Row || c1.Column != c2.Column;

    }
}