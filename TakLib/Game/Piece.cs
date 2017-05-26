using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TakLib
{
    public enum PieceColor { White = 0, Black = 1 };
    public enum PieceType { Stone = 0, Wall = 1, CapStone = 2 };
    public struct Piece
    {
        public Piece(PieceColor color, PieceType type)
        {
            Color = color;
            Type = type;
        }

        public readonly PieceType Type;

        public readonly PieceColor Color;
    }

}
