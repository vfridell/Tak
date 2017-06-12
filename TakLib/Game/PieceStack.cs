using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TakLib
{
    public enum StackType
    {
        Empty = 0,
        WhiteSoft = 1,
        WhiteHard = 2,
        BlackSoft = -1,
        BlackHard = -2
    };

    [Serializable]
    public class PieceStack : Stack<Piece>
    {
        private int[] _pieces = {0, 0};

        public PieceStack Clone()
        {
            PieceStack clone = new PieceStack();
            foreach(Piece p in this.Reverse())
            {
                clone.Push(new Piece(p.Color, p.Type));
            }
            return clone;
        }

        public new void Push(Piece p)
        {
            base.Push(p);
            _pieces[(int) p.Color]++;
        }

        public new Piece Pop()
        {
            Piece p = base.Pop();
            _pieces[(int) p.Color]--;
            return p;
        }

        public int OwnerPieceCount => Count > 0 ? _pieces[(int) Peek().Color] : 0;
        public int CapturedPieceCount => Count > 0 ? (Peek().Color == PieceColor.White? _pieces[(int)PieceColor.Black] : _pieces[(int)PieceColor.White]) : 0;

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
