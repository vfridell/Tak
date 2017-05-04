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
        public int Size => _size;
        public bool WhiteToPlay => _whiteToPlay;
        public PieceColor ColorToPlay => (PieceColor) StonePileIndex;

        public int Turn => _turn;
        public int Round => _round;

        protected int StonePileIndex
        {
            get
            {
                if (_turn > 1) return _whiteToPlay ? 0 : 1;
                return _whiteToPlay ? 1 : 0;
            }
        }

        private PieceStack[,] _grid;
        private int[] _capStonesInHand;
        private int[] _stonesInHand;
        private bool _whiteToPlay = true;
        private int _turn = 1;
        private int _size;
        private int _round = 1;

        public int StonesInHand(PieceColor color) => _stonesInHand[(int)color];
        public int CapStonesInHand(PieceColor color) => _capStonesInHand[(int)color];

        protected Board() { }

        protected Board(GameSetup gameSetup)
        {
            _size = gameSetup.BoardSize;
            _grid = new PieceStack[_size,_size];
            for(int r=0; r<_size;r++)
                for(int c=0; c<_size;c++)
                    _grid[r,c] = new PieceStack();
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
            if(StackSize(r,c) != 0) throw new Exception("Cannot place on non-empty space");
            _grid[r, c].Push(piece);
            if (piece.Type == PieceType.CapStone) _capStonesInHand[StonePileIndex]--;
            else _stonesInHand[StonePileIndex]--;
            if(_capStonesInHand[StonePileIndex] < 0 || _stonesInHand[StonePileIndex] < 0) throw new Exception($"Cannot place: no pieces in hand for {ColorToPlay}");
        }

        public int StackSize(int r, int c) => _grid[r, c].Count;

        public bool StackOwned(int r, int c, PieceColor color)
        {
            CheckGridRange(r, c);
            if (StackSize(r, c) == 0) return false;
            return color == _grid[r,c].Peek().Color;
        }

        public PieceStack PickStack(int r, int c, int count)
        {
            CheckGridRange(r,c);
            if(count > Size) throw new Exception($"{count} is greater than the carry limit of {Size}");
            if(StackSize(r, c) < count) throw new Exception($"There are fewer than {count} stones at {r},{c}");
            PieceStack tempStack = new PieceStack();
            int leftToPick = count;
            while (leftToPick > 0)
            {
                tempStack.Push(_grid[r, c].Pop());
                leftToPick--;
            }
            return tempStack;
        }

        public void PlaceStack(int r, int c, Stack<Piece> heldStack)
        {
            CheckGridRange(r,c);
            if(heldStack.Count == 0) throw new Exception($"There are no stones in the held stack");
            while (heldStack.Count > 0)
                _grid[r,c].Push(heldStack.Pop());
        }

        public bool OnTheBoard(int r, int c) => Math.Max(r, c) < Size && r >= 0 && c >= 0;

        private void CheckGridRange(int r, int c)
        {
            if (!OnTheBoard(r,c)) throw new Exception($"{r},{c} out of range for grid size {Size}");
        }

        public int DistanceAvailable(int r, int c, Direction direction)
        {
            CheckGridRange(r,c);
            Coordinate next = new Coordinate(r, c).GetNeighbor(direction);
            int dist = 0;
            while (OnTheBoard(next.Row, next.Column) && !WallOrCap(next.Row, next.Column))
            {
                dist++;
                next = new Coordinate(r, c).GetNeighbor(direction);
            }
            return dist;
        }

        public bool WallOrCap(int r, int c)
            => _grid[r, c].Count > 0 && 
               (_grid[r, c].Peek().Type == PieceType.Wall || _grid[r, c].Peek().Type == PieceType.CapStone);

        public IEnumerable<Move> GetAllMoves()
        {
            throw new NotImplementedException();
        }

        public Board Clone()
        {
            Board clone = new Board();
            clone._round = _round;
            clone._turn = _turn;
            clone._size = _size;
            clone._whiteToPlay = _whiteToPlay;
            _capStonesInHand.CopyTo(clone._capStonesInHand,0);
            _stonesInHand.CopyTo(clone._stonesInHand, 0);
            for (int r = 0; r < _size; r++)
                for (int c = 0; c < _size; c++)
                    clone._grid[r,c] = _grid[r,c].Clone();
            return clone;
        }

        public void EndPlayerMove()
        {
            _round++;
            _whiteToPlay = !_whiteToPlay;
        }

        public void EndTurn()
        {
            _turn++;
        }
    }
}
