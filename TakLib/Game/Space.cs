using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TakLib
{
    public struct Space
    {
        public Space(PieceStack stack, Coordinate coordinate, bool onBoard)
        {
            OnTheBoard = onBoard;
            Piece = null;
            IsEmpty = stack.Count == 0;
            if (!IsEmpty) Piece = stack.Peek();
            Coordinate = coordinate;
        }

        public bool OnTheBoard;
        public bool IsEmpty;
        public Piece? Piece;
        public Coordinate Coordinate;

        public bool ColorEquals(PieceColor color) =>  Piece.HasValue && Piece?.Color == color;
        public bool ColorEquals(Space other) => Piece?.Color == other.Piece?.Color;
        public bool TypeEquals(Space other) => Piece?.Type == other.Piece?.Type;

        public override bool Equals(object obj)
        {
            if (!(obj is Space)) return false;
            Space other = (Space)obj;
            return Equals(other);
        }

        public bool Equals(Space other) => ColorEquals(other) && TypeEquals(other) && Coordinate.Equals(other.Coordinate);

        public override int GetHashCode()
        {
            return Coordinate.GetHashCode() * 397 + Piece.GetHashCode() * 17;
        }

        public override string ToString()
        {
            return (!OnTheBoard ? "Invalid Space: " : "Space: ") +
                ($"{Coordinate} ") +
                (IsEmpty ? "Empty" : $"{Piece?.Color} {Piece?.Type}");
        }
    }
}
