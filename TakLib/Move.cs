using System.Collections.Generic;
using System.Linq;

namespace TakLib
{
    public abstract class Move
    {
        public readonly char Row;
        public readonly char Column;
        public readonly Coordinate Location;

        protected Move(Coordinate location)
        {
            Location = location;
            Row = location.RowChar;
            Column = location.ColumnChar;
        }

        protected Move(char r, char c)
        {
            Row = r;
            Column = c;
            Location = new Coordinate(r,c);
        }

        public abstract void Apply(Board board);
    }

    public class PlaceCapstone : Move
    {
        public PlaceCapstone(char r, char c) : base(r, c) { }
        public override void Apply(Board board)
        {
            board.PlacePiece(Location.Row, Location.Column, new CapStone(board.ColorToPlay));
        }
    }

    public class PlaceWall : Move
    {
        public PlaceWall(char r, char c) : base(r, c) { }
        public override void Apply(Board board)
        {
            board.PlacePiece(Location.Row, Location.Column, new Wall(board.ColorToPlay));
        }
    }

    public class PlaceStone : Move
    {
        public PlaceStone(char r, char c) : base(r, c) { }
        public override void Apply(Board board)
        {
            board.PlacePiece(Location.Row, Location.Column, new Wall(board.ColorToPlay));
        }
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
            CarryInt = int.Parse(Carry.ToString());
            DirectionEnum = (Direction) Direction;
            DropInts = drops.Select(d => int.Parse(d.ToString())).ToList();

            int currentCarry = CarryInt;
            Coordinate nextCoordinate = Location;
            foreach (int currentDrop in DropInts)
            {
                SubMoves.Add(new MoveOneSquare(currentCarry, nextCoordinate, currentDrop, DirectionEnum));
                nextCoordinate = nextCoordinate.GetNeighbor((Direction)direction);
                currentCarry = currentCarry - currentDrop;
            }
        }

        public override void Apply(Board board)
        {
            foreach (Move move in SubMoves)
            {
                move.Apply(board);
            }
        }
    }

    public class MoveOneSquare : Move
    {
        public readonly int Carry;
        public readonly int Drop;
        public readonly Direction Direction;
        public MoveOneSquare(int carry, Coordinate start, int drop, Direction direction)
            : base(start)
        {
            Carry = carry;
            Drop = drop;
            Direction = direction;
        }

        public override void Apply(Board board)
        {
            PieceStack stack = board.PickStack(Location.Row, Location.Column, Carry);
            Coordinate nextCoordinate = Location.GetNeighbor(Direction);
            board.PlaceStack(nextCoordinate.Row, nextCoordinate.Column, stack);
        }
    }
}