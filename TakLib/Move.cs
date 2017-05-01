using System.Collections.Generic;

namespace TakLib
{
    public class Move
    {
        public readonly char Row;
        public readonly char Column;
        public readonly Coordinate Location;

        public Move(char r, char c)
        {
            Row = r;
            Column = c;
            Location = new Coordinate(r,c);
        }

    }

    public class PlaceCapstone : Move
    {
        public PlaceCapstone(char r, char c) : base(r, c) { }
    }

    public class PlaceWall : Move
    {
        public PlaceWall(char r, char c) : base(r, c) { }
        
    }

    public class PlaceStone : Move
    {
        public PlaceStone(char r, char c) : base(r, c) { }
    }

    public class MoveStack : Move
    {
        public readonly char Carry;
        public readonly IReadOnlyList<char> Drops;
        public readonly char Direction;

        public readonly int CarryInt;
        public readonly Direction DirectionEnum;
        public readonly IReadOnlyList<int> DropInts;

        private List<MoveOneSquare> SubMoves = new List<MoveOneSquare>();
        public MoveStack(char carry, char r, char c, char[] drops, char direction) 
            : base(r,c)
        {
            Carry = carry;
            Drops = new List<char>();
            Drops =  drops;
            Direction = direction;

            char currentRow = r;
            char currentCol = c;
            char currentDrop;
            char currentCarry = carry;
            foreach (char drop in Drops)
            {
                int carryInt = int.Parse(Carry.ToString());
                carryInt
                SubMoves.Add(new MoveOneSquare(carry, currentRow, currentCol, drop, direction));
                Coordinate nextCoordinate = Location.GetNeighbor((Direction)direction);
                currentRow = nextCoordinate.RowChar;
                currentCol = nextCoordinate.ColumnChar;
                currentCarry = int.Parse(carry);
            }
            SubMoves
        }
        
    }

    public class MoveOneSquare : Move
    {
        public readonly char Carry;
        public readonly char Drop;
        public readonly char Direction;
        public MoveOneSquare(char carry, char r, char c, char drop, char direction) 
            : base(r,c)
        {
            Carry = carry;
            Drop = drop;
            Direction = direction;
        }
    }
}