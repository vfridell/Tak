using System;
using System.Collections.Generic;

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
        private int _flatScore;
        private int _emptySpaces;
        private bool _flatScoreDirty = true;
        private IEnumerable<Move> _moves;

        internal GameResult GameResult;

        public int StonesInHand(PieceColor color) => _stonesInHand[(int)color];
        public int CapStonesInHand(PieceColor color) => _capStonesInHand[(int)color];

        public bool EitherPlayerOutOfPieces => CapStonesInHand(PieceColor.White) + StonesInHand(PieceColor.White) == 0 ||
                                               CapStonesInHand(PieceColor.Black) + StonesInHand(PieceColor.Black) == 0;

        public int FlatScore
        {
            get
            {
                if (_flatScoreDirty)
                {
                    ComputeFlatScoreAndEmptySpaces();
                }
                return _flatScore;
            }
        }

        public int EmptySpaces
        {
            get
            {
                if (_flatScoreDirty)
                {
                    ComputeFlatScoreAndEmptySpaces();
                }
                return _emptySpaces;
            }
        }

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
            _flatScoreDirty = true;
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
            _flatScoreDirty = true;
            return tempStack;
        }

        public void PlaceStack(int r, int c, Stack<Piece> heldStack)
        {
            CheckGridRange(r,c);
            if(heldStack.Count == 0) throw new Exception($"There are no stones in the held stack");
            while (heldStack.Count > 0)
                _grid[r,c].Push(heldStack.Pop());
            _flatScoreDirty = true;
        }

        public bool OnTheBoard(int r, int c) => Math.Max(r, c) < Size && r >= 0 && c >= 0;
        public bool OnTheBoard(Coordinate c) => OnTheBoard(c.Row, c.Column);

        private void CheckGridRange(int r, int c)
        {
            if (!OnTheBoard(r,c)) throw new Exception($"{r},{c} out of range for grid size {Size}");
        }

        private void CheckNonEmpty(int r, int c)
        {
            if(_grid[r,c].Count == 0) throw new Exception($"Operation not valid on empty space at {r}, {c}");
        }

        public DistanceAvailable GetDistanceAvailable(int r, int c, Direction direction)
        {
            CheckGridRange(r,c);
            Coordinate next = new Coordinate(r, c).GetNeighbor(direction);
            int dist = 0;
            while (OnTheBoard(next.Row, next.Column) && !IsWallOrCap(next.Row, next.Column))
            {
                dist++;
                next = (new Coordinate(next.Row, next.Column)).GetNeighbor(direction);
            }
            return new DistanceAvailable()
            {
                Distance = dist,
                EndsWithWall = OnTheBoard(next.Row, next.Column) && IsWall(next.Row, next.Column),
                CapStoneTop = IsCapStone(r, c),
            };
        }

        public bool IsWallOrCap(int r, int c)
            => _grid[r, c].Count > 0 && 
               (_grid[r, c].Peek().Type == PieceType.Wall || _grid[r, c].Peek().Type == PieceType.CapStone);

        public bool IsWall(int r, int c) => _grid[r, c].Count > 0 && (_grid[r, c].Peek().Type == PieceType.Wall);
        public bool IsCapStone(int r, int c) => _grid[r, c].Count > 0 && (_grid[r, c].Peek().Type == PieceType.CapStone);

        public Piece GetPiece(int r, int c)
        {
            CheckGridRange(r, c);
            CheckNonEmpty(r, c);
            return _grid[r, c].Peek();
        }

        public Piece GetPiece(Coordinate c)
        {
            return GetPiece(c.Row, c.Column);
        }

        public Space GetSpace(Coordinate c)
        {
            return new Space(OnTheBoard(c) ? _grid[c.Row, c.Column] : new PieceStack(), c, OnTheBoard(c));
        }

        public IEnumerable<Move> GetAllMoves()
        {
            if (_moves == null || _flatScoreDirty)
            {
                _moves = MoveGenerator.GetAllMoves(this);
            }
            return _moves;
        }

        private void ComputeFlatScoreAndEmptySpaces()
        {
            _emptySpaces = 0;
            _flatScore = 0;
            foreach (Coordinate c in new CoordinateEnumerable(Size))
            {
                Space space = GetSpace(c);
                if (space.IsEmpty)
                    _emptySpaces++;
                else if (space.Piece?.Type == PieceType.Stone)
                    _flatScore += space.ColorEquals(PieceColor.White) ? 1 : -1;
            }
            _flatScoreDirty = false;
        }

        public Board Clone()
        {
            Board clone = new Board();
            clone._capStonesInHand = new[] {_capStonesInHand[0], _capStonesInHand[1]};
            clone._stonesInHand = new[] {_stonesInHand[0], _stonesInHand[1]};
            clone._round = _round;
            clone._turn = _turn;
            clone._size = _size;
            clone._whiteToPlay = _whiteToPlay;
            clone._grid = new PieceStack[_size,_size];
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

    public struct DistanceAvailable
    {
        public int Distance;
        public bool EndsWithWall;
        public bool CapStoneTop;
    }
}
