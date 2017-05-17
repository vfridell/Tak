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
            return GetAllMoves(board, board.ColorToPlay);
        }

        public static IEnumerable<Move> GetAllMovesNonCurrentPlayer(Board board)
        {
            return GetAllMoves(board, board.ColorToPlay == PieceColor.White ? PieceColor.Black : PieceColor.White);
        }

        public static IEnumerable<Move> GetAllMoves(Board board, PieceColor colorToCheck)
        {
            IList<Move> moves = new List<Move>();
            for (int r = 0; r < board.Size; r++)
            {
                for (int c = 0; c < board.Size; c++)
                {
                    Coordinate location = new Coordinate(r, c);
                    // placement moves
                    if (board.StackSize(r, c) == 0)
                    {
                        if (board.StonesInHand(colorToCheck) > 0)
                        {
                            moves.Add(new PlaceStone(location));
                            if (board.Turn > 1) moves.Add(new PlaceWall(location));
                        }

                        if (board.Turn > 1 && board.CapStonesInHand(colorToCheck) > 0)
                        {
                            moves.Add(new PlaceCapstone(location));
                        }
                    }
                    else if (board.StackOwned(r, c, colorToCheck))
                    {
                        AddMovementMoves(board, moves, location, Direction.Up);
                        AddMovementMoves(board, moves, location, Direction.Down);
                        AddMovementMoves(board, moves, location, Direction.Left);
                        AddMovementMoves(board, moves, location, Direction.Right);
                    }
                }
            }
            return moves;

        }

        private static void AddMovementMoves(Board board, IList<Move> moves, Coordinate location, Direction dir)
        {
            // movement moves
            DistanceAvailable maxDist = board.GetDistanceAvailable(location.Row, location.Column, dir);
            int possibleCarry = Math.Min(board.StackSize(location.Row, location.Column), board.Size);
            if (maxDist.Distance == 0 || possibleCarry == 0) return;

            List<List<int>> dropLists = GetAllDropLists(possibleCarry, maxDist);
            foreach (List<int> dropList in dropLists)
            {
                moves.Add(new MoveStack(dropList.Sum(), location, dropList, dir));
            }
        }


        public static List<List<int>> GetAllDropLists(int maxPicked, DistanceAvailable maxDistance)
        {
            List<List<int>> returnList = new List<List<int>>();
            for (int i = 1; i <= maxPicked; i++)
                returnList.AddRange(GetAllDropListsRecursive(i, maxDistance, new List<int>()));
            return returnList;
        }


        public static List<List<int>> GetAllDropListsRecursive(int maxPicked, DistanceAvailable maxDistance, List<int> baseList)
        {

            List<List<int>> returnList = new List<List<int>>();
            if (baseList.Count > maxDistance.Distance)
            {
                if(maxDistance.EndsWithWall && maxDistance.CapStoneTop && baseList.Last() == 1) returnList.Add(baseList);
                return returnList;
            }

            int sum = baseList.Sum();
            if (sum == maxPicked)
            {
                returnList.Add(baseList);
                return returnList;
            }

            int picked = 1;
            while (picked + sum <= maxPicked)
            {
                List<int> newBase = new List<int>(baseList) { picked };
                returnList.AddRange(GetAllDropListsRecursive(maxPicked, maxDistance, newBase));
                picked++;
            }

            return returnList;
        }
    }
}
