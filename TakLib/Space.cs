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
    }
}
