using QuickGraph;
using QuickGraph.Algorithms.ConnectedComponents;
using System;
using System.Collections.Generic;
using System.Linq;

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
        private SpaceGraph _whiteSpaceGraph;
        private SpaceGraph _blackSpaceGraph;


        private GameResult _gameResult = GameResult.Incomplete;
        private bool _gameResultSet = false;
        public GameResult GameResult 
        {
            get
            {
                if (!_gameResultSet)
                {
                    _gameResult = GameResultService.GetGameResult(this);
                    _gameResultSet = true;
                }
                return _gameResult;
            }
        }

        public override bool Equals(object obj)
        {
            Board other = obj as Board;
            if (null == other) return false;
            return Equals(other);
        }

        public bool Equals(Board other)
        {
            if (_size != other._size) return false;
            if (_whiteToPlay != other._whiteToPlay) return false;
            if (!_capStonesInHand.SequenceEqual(other._capStonesInHand)) return false;
            if (!_stonesInHand.SequenceEqual(other._stonesInHand)) return false;
            foreach (Coordinate c in new CoordinateEnumerable(_size))
            {
                if (!_grid[c.Row, c.Column].Equals(other._grid[c.Row, c.Column])) return false;
            }
            return true;
        }

        public override int GetHashCode()
        {
            int hash = (_size * 397);
            foreach (Coordinate c in new CoordinateEnumerable(_size))
            {
                hash = hash ^ (_grid[c.Row, c.Column].GetHashCode() * 397);
            }
            return hash;
        }

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
            _whiteSpaceGraph = new SpaceGraph(this, PieceColor.White);
            _blackSpaceGraph = new SpaceGraph(this, PieceColor.Black);
        }

        public static Board GetInitialBoard(GameSetup gameSetup)
        {
            Board newBoard = new Board(gameSetup);
            return newBoard;
        }

        public void PlacePiece(Coordinate c, Piece piece)
        {
            CheckGridRange(c);
            if(piece.Color != ColorToPlay) throw new Exception($"Piece {piece.Color} does not match color to play in game {ColorToPlay}");
            if(StackSize(c) != 0) throw new Exception("Cannot place on non-empty space");
            Space oldSpace = GetSpace(c);
            _grid[c.Row, c.Column].Push(piece);
            Space newSpace = GetSpace(c);
            _whiteSpaceGraph.ChangeStackVertex(this, oldSpace, newSpace);
            _blackSpaceGraph.ChangeStackVertex(this, oldSpace, newSpace);
            _flatScoreDirty = true;
            _gameResultSet = false;
            if (piece.Type == PieceType.CapStone) _capStonesInHand[StonePileIndex]--;
            else _stonesInHand[StonePileIndex]--;
            if(_capStonesInHand[StonePileIndex] < 0 || _stonesInHand[StonePileIndex] < 0) throw new Exception($"Cannot place: no pieces in hand for {ColorToPlay}");
        }

        public int StackSize(Coordinate c) => _grid[c.Row, c.Column].Count;

        public bool StackOwned(Coordinate c, PieceColor color)
        {
            CheckGridRange(c);
            if (StackSize(c) == 0) return false;
            return color == _grid[c.Row, c.Column].Peek().Color;
        }

        public PieceStack PickStack(Coordinate c, int count)
        {
            CheckGridRange(c);
            if(count > Size) throw new Exception($"{count} is greater than the carry limit of {Size}");
            if(StackSize(c) < count) throw new Exception($"There are fewer than {count} stones at {c.Row},{c.Column}");
            Space oldSpace = GetSpace(c);
            PieceStack tempStack = new PieceStack();
            int leftToPick = count;
            while (leftToPick > 0)
            {
                tempStack.Push(_grid[c.Row, c.Column].Pop());
                leftToPick--;
            }
            Space newSpace = GetSpace(c);
            _whiteSpaceGraph.ChangeStackVertex(this, oldSpace, newSpace);
            _blackSpaceGraph.ChangeStackVertex(this, oldSpace, newSpace);
            _flatScoreDirty = true;
            _gameResultSet = false;
            return tempStack;
        }

        public void PlaceStack(Coordinate c, Stack<Piece> heldStack)
        {
            CheckGridRange(c);
            if(heldStack.Count == 0) throw new Exception($"There are no stones in the held stack");
            Space oldSpace = GetSpace(c);
            while (heldStack.Count > 0)
                _grid[c.Row,c.Column].Push(heldStack.Pop());
            Space newSpace = GetSpace(c);
            _whiteSpaceGraph.ChangeStackVertex(this, oldSpace, newSpace);
            _blackSpaceGraph.ChangeStackVertex(this, oldSpace, newSpace);
            _flatScoreDirty = true;
            _gameResultSet = false;
        }

        public ConnectedComponentsAlgorithm<Space, UndirectedEdge<Space>> ComputeConnectedComponents(PieceColor color)
        {
            SpaceGraph spaceAdjacencyGraph = color == PieceColor.White ? _whiteSpaceGraph : _blackSpaceGraph;
            var connectedComponentsAlg = new ConnectedComponentsAlgorithm<Space, UndirectedEdge<Space>>(spaceAdjacencyGraph);
            connectedComponentsAlg.Compute();
            return connectedComponentsAlg;
        }

        public bool OnTheBoard(int r, int c) => Math.Max(r, c) < Size && r >= 0 && c >= 0;
        public bool OnTheBoard(Coordinate c) => OnTheBoard(c.Row, c.Column);

        private void CheckGridRange(Coordinate c)
        {
            if (!OnTheBoard(c)) throw new Exception($"{c.Row},{c.Column} out of range for grid size {Size}");
        }

        private void CheckNonEmpty(Coordinate c)
        {
            if(_grid[c.Row,c.Column].Count == 0) throw new Exception($"Operation not valid on empty space at {c.Row}, {c.Column}");
        }

        public DistanceAvailable GetDistanceAvailable(Coordinate c, Direction direction)
        {
            CheckGridRange(c);
            Coordinate next = c.GetNeighbor(direction);
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
                CapStoneTop = IsCapStone(c.Row, c.Column),
            };
        }

        public bool IsWallOrCap(int r, int c)
            => _grid[r, c].Count > 0 && 
               (_grid[r, c].Peek().Type == PieceType.Wall || _grid[r, c].Peek().Type == PieceType.CapStone);

        public bool IsWall(int r, int c) => _grid[r, c].Count > 0 && (_grid[r, c].Peek().Type == PieceType.Wall);
        public bool IsCapStone(int r, int c) => _grid[r, c].Count > 0 && (_grid[r, c].Peek().Type == PieceType.CapStone);

        public Piece GetPiece(Coordinate c)
        {
            CheckGridRange(c);
            CheckNonEmpty(c);
            return _grid[c.Row, c.Column].Peek();
        }

        public Space GetSpace(Coordinate c)
        {
            return new Space(OnTheBoard(c) ? _grid[c.Row, c.Column] : new PieceStack(), c, OnTheBoard(c));
        }

        public IEnumerable<Move> GetAllMoves()
        {
            if (GameResult != GameResult.Incomplete) throw new Exception("Game is over");
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
            clone._whiteSpaceGraph = _whiteSpaceGraph.Clone();
            clone._blackSpaceGraph = _blackSpaceGraph.Clone();
            clone._gameResult = _gameResult;
            clone._gameResultSet = _gameResultSet;
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
