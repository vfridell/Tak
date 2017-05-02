using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TakLib
{
    public enum PieceColor { White = 0, Black = 1 };
    public class Piece
    {
        public Piece(PieceColor color)
        {
            Color = color;
        }

        public readonly PieceColor Color;
    }

    public class Stone : Piece {
        public Stone(PieceColor color) : base(color)
        {
        }
    }

    public class Wall : Piece {
        public Wall(PieceColor color) : base(color)
        {
        }
    }

    public class CapStone : Piece {
        public CapStone(PieceColor color) : base(color)
        {
        }
    }
}
