using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace TakLib
{
    public class RandomAI : Player, ITakAI
    {
        private Random _rand = new Random();
        private bool _playingWhite;
        public bool playingWhite { get { return _playingWhite; } }

        public Move MakeBestMove(Game game)
        {
            Board board = game.CurrentBoard;
            Move move;
            if ((_playingWhite && board.WhiteToPlay) || (!_playingWhite && !board.WhiteToPlay))
            {
                move = PickBestMove(board);
                game.ApplyMove(move);
            }
            else
            {
                throw new Exception("It is not my move :(");
            }
            return move;
        }

        public void BeginNewGame(bool playingWhite)
        {
            _playingWhite = playingWhite;
        }

        Move GetRandomMove(IList<Move> moves)
        {
            return moves[_rand.Next(0, moves.Count - 1)];
        }

        public string Name
        {
            get { return "RandomAI"; }
        }

        public Move PickBestMove(Board board)
        {
            IEnumerable<Move> moves = board.GetAllMoves();
            return GetRandomMove(moves.ToList());
        }

        public Task<Move> PickBestMoveAsync(Board board, CancellationToken aiCancelToken)
        {
            IEnumerable<Move> moves = board.GetAllMoves();
            return Task.FromResult(GetRandomMove(moves.ToList()));
        }


    }
}
