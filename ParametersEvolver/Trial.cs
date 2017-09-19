using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TakLib;

namespace ParametersEvolver
{
    public class Trial
    {
        public Trial(Board board, Move priorMove, Move correctMove)
        {
            Board = board;
            PriorMove = priorMove;
            CorrectMove = correctMove;
        }

        public readonly Board Board;
        public readonly Move PriorMove;

        public readonly Move CorrectMove;
    }
}
