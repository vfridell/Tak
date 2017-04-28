using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TakLib
{
    public class Game
    {
        private IList<Board> _boards;
        private Player _whitePlayer;
        private Player _blackPlayer;

        public Player WhitePlayer => _whitePlayer;
        public Player BlackPlayer => _blackPlayer;
        public Board CurrentBoard => _boards.Last();
        public bool WhiteToPlay = true;
        public Player CurrentPlayer => WhiteToPlay ? _whitePlayer : _blackPlayer;

        public Game(GameSetup gameSetup)
        {
            _boards = new List<Board>();
            _boards.Add(Board.GetInitialBoard(gameSetup.BoardSize));
        }

        public IEnumerable<Move> GetAllMoves()
        {
            throw new NotImplementedException();
        }

        public void ApplyMove(Move move)
        {
            throw new NotImplementedException();
        }

        public void EndTurn()
        {
            throw new NotImplementedException();
        }
    }
}
