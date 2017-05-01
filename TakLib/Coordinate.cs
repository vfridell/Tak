using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TakLib
{
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

        public Coordinate Add(Coordinate other) => new Coordinate(other.Row + Row, other.Column + Column);
        public Coordinate GetNeighbor(Direction direction) => Add(NotationParser.ToCoordinateDirection(direction));
        public char RowChar => NotationParser.ToCharRow(this);
        public char ColumnChar => NotationParser.ToCharColumn(this);
    }
}
