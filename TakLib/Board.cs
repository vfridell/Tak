using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TakLib
{
    public class Board
    {
        private readonly int _size;
        public int Size => _size;
        public bool WhiteToPlay => _whiteToPlay;
        public PieceColor ColorToPlay => (PieceColor) PlayerIndex;

        protected int PlayerIndex => _whiteToPlay ? _w : _b;

        private Stack<Piece>[,] _grid;
        private int _w = 0;
        private int _b = 1;
        private int[] _capStonesInHand;
        private int[] _stonesInHand;
        private bool _whiteToPlay = true;

        protected Board(GameSetup gameSetup)
        {
            _size = gameSetup.BoardSize;
            _grid = new Stack<Piece>[_size,_size];
            for(int r=0; r<_size;r++)
                for(int c=0; c<_size;c++)
                    _grid[r,c] = new Stack<Piece>();
            _capStonesInHand = new []{gameSetup.NumCapstones, gameSetup.NumCapstones};
            _stonesInHand = new []{gameSetup.NumStonesPerSide, gameSetup.NumStonesPerSide};
        }

        public static Board GetInitialBoard(GameSetup gameSetup)
        {
            Board newBoard = new Board(gameSetup);
            return newBoard;
        }

        public void PlacePiece(int r, int c, Piece piece)
        {
            CheckGridRange(r,c);
            if(piece.Color != ColorToPlay) throw new Exception($"Piece {piece.Color} does not match color to play in game {ColorToPlay}");
            if(_grid[r,c].Count != 0) throw new Exception("Cannot place on non-empty space");
            _grid[r, c].Push(piece);
            if (piece is CapStone) _capStonesInHand[PlayerIndex]--;
            else _stonesInHand[PlayerIndex]--;
            if(_capStonesInHand[PlayerIndex] < 0 || _stonesInHand[PlayerIndex] < 0) throw new Exception($"Cannot place: no pieces in hand for {ColorToPlay}");
        }

        public Stack<Piece> PickStack(int r, int c, int count)
        {
            CheckGridRange(r,c);
            if(count > Size) throw new Exception($"{count} is greater than the carry limit of {Size}");
            if(_grid[r,c].Count < count) throw new Exception($"There are fewer than {count} stones at {r},{c}");
            Stack<Piece> tempStack = new Stack<Piece>();
            int leftToPick = count;
            while (leftToPick > 0)
            {
                tempStack.Push(_grid[r, c].Pop());
                leftToPick--;
            }
            return tempStack;
        }

        public void PlaceStack(int r, int c, Stack<Piece> heldStack, int count)
        {
            CheckGridRange(r,c);
            if(count > heldStack.Count) throw new Exception($"There are fewer than {count} stones in the held stack ({heldStack.Count})");
            int leftToPush = count;
            while (leftToPush > 0)
            {
                _grid[r,c].Push(heldStack.Pop());
                leftToPush--;
            }
        }

        private void CheckGridRange(int r, int c)
        {
            if (Math.Max(r, c) > Size) throw new Exception($"{r},{c} out of range for grid size {Size}");
        }

        public IEnumerable<Move> GetAllMoves()
        {
            throw new NotImplementedException();
        }

        public Board CloneBoard()
        {
            throw new NotImplementedException();
        }

        public void EndTurn()
        {
            _whiteToPlay = !_whiteToPlay;
            throw new NotImplementedException();
        }
    }
}
