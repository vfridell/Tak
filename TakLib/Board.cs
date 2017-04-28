using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TakLib
{
    public class Board
    {
        private int _size;
        public int Size => _size;

        protected Board(int size)
        {
            _size = size;
        }

        public static Board GetInitialBoard(int size)
        {
            Board newBoard = new Board(size);

            return newBoard;
        }
    }
}
