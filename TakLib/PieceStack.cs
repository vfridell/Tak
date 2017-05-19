using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TakLib
{
    public class PieceStack : Stack<Piece>
    {
        public PieceStack Clone()
        {
            PieceStack clone = new PieceStack();
            foreach(Piece p in this.Reverse())
            {
                clone.Push(new Piece(p.Color, p.Type));
            }
            return clone;
        }
    }
}
