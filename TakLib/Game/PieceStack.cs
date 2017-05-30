using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TakLib
{
    [Serializable]
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

        public override bool Equals(object obj)
        {
            PieceStack other = obj as PieceStack;
            if (null == other) return false;
            return Equals(other);
        }

        public bool Equals(PieceStack other)
        {
            if (other.Count != Count) return false;
            return this.SequenceEqual(other);
        }

        public override int GetHashCode()
        {
            if (Count == 0) return 0;
            return Peek().GetHashCode() ^ Count * 397;
        }
    }
}
