using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TakLib
{
    public static class MoveGenerator
    {
        public static IEnumerable<Move> GetAllMoves(Board board)
        {
            IList<Move> moves = new List<Move>();
            for (int r = 0; r < board.Size; r++)
            {
                for (int c = 0; c < board.Size; c++)
                {
                    Coordinate location = new Coordinate(r,c);
                    // placement moves
                    if (board.StackSize(r, c) == 0)
                    {
                        if (board.StonesInHand(board.ColorToPlay) > 0)
                        {
                            moves.Add(new PlaceStone(location));
                            if(board.Turn > 1) moves.Add(new PlaceWall(location));
                        }

                        if (board.Turn > 1 && board.CapStonesInHand(board.ColorToPlay) > 0)
                        {
                            moves.Add(new PlaceCapstone(location));
                        }
                    }
                    else if(board.StackOwned(r, c, board.ColorToPlay))
                    {
                        // movement moves
                        int maxDist = board.DistanceAvailable(r, c, Direction.Up);
                        int possibleCarry = Math.Min(board.StackSize(r, c), board.Size);

                        for (int p1 = possibleCarry; p1 > 0; p1--)
                        {

                            new MoveStack(p1, location, new List<int>(), Direction.Up);
                        }
                    }


                }
            }
            return moves;

        }
    }
}
