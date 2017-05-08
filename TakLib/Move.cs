using System;
using System.Collections.Generic;
using System.Linq;
using System.Management.Instrumentation;
using System.Net;

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
        public PlaceCapstone(Coordinate location) : base(location) { }
        public override void Apply(Board board)
        {
            board.PlacePiece(Location.Row, Location.Column, new Piece(board.ColorToPlay, PieceType.CapStone));
        }

        public override string ToString()
        {
            return $"C{Column}{Row}";
        }
    }

    public class PlaceWall : Move
    {
        public PlaceWall(char r, char c) : base(r, c) { }
        public PlaceWall(Coordinate location) : base(location) { }
        public override void Apply(Board board)
        {
            board.PlacePiece(Location.Row, Location.Column, new Piece(board.ColorToPlay, PieceType.Wall));
        }

        public override string ToString()
        {
            return $"S{Column}{Row}";
        }
    }

    public class PlaceStone : Move
    {
        public PlaceStone(char r, char c) : base(r, c) { }
        public PlaceStone(Coordinate location) : base(location) { }
        public override void Apply(Board board)
        {
            board.PlacePiece(Location.Row, Location.Column, new Piece(board.ColorToPlay, PieceType.Stone));
        }
        public override string ToString()
        {
            return $"{Column}{Row}";
        }
    }

    public class MoveStack : Move
    {
        public readonly char Carry;
        private readonly List<char> _drops = new List<char>();
        public IReadOnlyList<char> Drops => _drops.AsReadOnly();
        public readonly char Direction;

        public readonly int CarryInt;
        public readonly Direction DirectionEnum;
        private readonly List<int> _dropInts = new List<int>();
        public IReadOnlyList<int> DropInts => _dropInts.AsReadOnly();

        private readonly List<MoveOneSquare> SubMoves = new List<MoveOneSquare>();
        public MoveStack(char carry, char r, char c, char[] drops, char direction) 
            : base(r,c)
        {
            Carry = carry;
            _drops.AddRange(drops);
            Direction = direction;
            CarryInt = int.Parse(Carry.ToString());
            DirectionEnum = (Direction) Direction;
            _dropInts = drops.Select(d => int.Parse(d.ToString())).ToList();

            AddSubMoves();
        }

        public MoveStack(int carry, Coordinate location, List<int> drops, Direction direction)
            : base(location)
        {
            CarryInt = carry;
            Carry = carry.ToString()[0];
            DirectionEnum = direction;
            Direction = (char) direction;
            _dropInts.AddRange(drops);
            _drops = new List<char>(drops.Select(c => c.ToString()[0]));

            AddSubMoves();
        }

        private void AddSubMoves()
        {
            int currentCarry = CarryInt;
            Coordinate nextCoordinate = Location;
            foreach (int currentDrop in DropInts)
            {
                SubMoves.Add(new MoveOneSquare(currentCarry, nextCoordinate, currentDrop, DirectionEnum));
                nextCoordinate = nextCoordinate.GetNeighbor(DirectionEnum);
                currentCarry = currentCarry - currentDrop;
            }
        }

        public override void Apply(Board board)
        {
            foreach (MoveOneSquare move in SubMoves)
            {
                move.Apply(board);
            }
        }
        public override string ToString()
        {
            return $"{Carry}{Column}{Row}{Direction}{new string(_drops.ToArray())}";
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