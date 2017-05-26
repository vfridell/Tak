using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TakLib
{
    // this class must be thread safe
    public class SimpleAnalyzer : IBoardAnalyzer
    {
        private readonly int _boardSize;
        public SimpleAnalyzer(int boardSize)
        {
            _boardSize = boardSize;
        }

        public IAnalysisResult Analyze (Board board)
        {
            return new SimpleAnalysisData() { gameResult = board.GameResult};
        }
    }
}
