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
        public PieceColor Color { get; set; }
    }

    public class Stone : Piece { }

    public class Wall : Piece { }

    public class CapStone : Piece { }
}
