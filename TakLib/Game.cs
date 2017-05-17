using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TakLib
{
    public enum GameResult
    {
        Incomplete,
        WhiteFlat,
        BlackFlat,
        WhiteRoad,
        BlackRoad,
        Draw
    };

    public class Game
    {
        private IList<Board> _boards;
        private Player _whitePlayer;
        private Player _blackPlayer;
        private RoadFinder _roadFinder;

        public RoadFinder RoadFinder => _roadFinder;
        public Player WhitePlayer => _whitePlayer;
        public Player BlackPlayer => _blackPlayer;
        public Board CurrentBoard => _boards.Last();
        public bool WhiteToPlay => CurrentBoard.WhiteToPlay;
        public PieceColor ColorToPlay => CurrentBoard.ColorToPlay;
        public Player CurrentPlayer => WhiteToPlay ? _whitePlayer : _blackPlayer;
        public int Turn => CurrentBoard.Turn;
        public GameResult GameResult { get; private set; }

        public static readonly Dictionary<int, Tuple<int, int>> InitialPieceSetup = new Dictionary<int, Tuple<int, int>>
        {
            [3] = new Tuple<int, int>(10, 0),
            [4] = new Tuple<int, int>(15, 0),
            [5] = new Tuple<int, int>(21, 1),
            [6] = new Tuple<int, int>(30, 1),
            [7] = new Tuple<int, int>(40, 2),
            [8] = new Tuple<int, int>(50, 2),
        };

        protected Game() { }

        public static Game GetNewGame(GameSetup gameSetup)
        {
            if (gameSetup.BoardSize < 3 || gameSetup.BoardSize > 8) throw new Exception("Board size must be between 3 and 8");
            Game newGame = new Game();
            newGame._whitePlayer = gameSetup.WhitePlayer;
            newGame._blackPlayer = gameSetup.BlackPlayer;
            gameSetup.NumStonesPerSide = InitialPieceSetup[gameSetup.BoardSize].Item1;
            if(gameSetup.BoardSize != 7) gameSetup.NumCapstones = InitialPieceSetup[gameSetup.BoardSize].Item2;
            newGame._boards = new List<Board> {Board.GetInitialBoard(gameSetup)};
            newGame._roadFinder = new RoadFinder(gameSetup.BoardSize);
            return newGame;
        }

        public static Game GetNewGame(int boardSize, int capStones=0)
        {
            if (boardSize == 7 && capStones != 2 && capStones != 1) throw new Exception("For size 7 board, capstones must be 1 or 2");
            GameSetup gameSetup = new GameSetup()
            {
                WhitePlayer = new Player() { Name = "Player1" },
                BlackPlayer = new Player() { Name = "Player2" },
                BoardSize = boardSize
            };
            return GetNewGame(gameSetup);
        }

        public IEnumerable<Move> GetAllMoves()
        {
            if(GameResult == GameResult.Incomplete)
                return CurrentBoard.GetAllMoves();
            else
                return new List<Move>();
        }

        public void ApplyMove(Move move)
        {
            if (GameResult != GameResult.Incomplete) throw new Exception("Game is already finished");
            move.Apply(CurrentBoard);
            EndPlayerMove();
        }

        public void EndPlayerMove()
        {
            SetGameResult();
            if (GameResult == GameResult.Incomplete)
            {
                CurrentBoard.EndPlayerMove();
                if (CurrentBoard.Round % 2 != 0) EndTurn();
                _boards.Add(CurrentBoard.Clone());
            }
        }

        public void EndTurn()
        {
            CurrentBoard.EndTurn();
        }

        private void SetGameResult()
        {
            var currentColor = CurrentBoard.WhiteToPlay ? PieceColor.White : PieceColor.Black;
            var otherColor = CurrentBoard.WhiteToPlay ? PieceColor.Black : PieceColor.White;
            _roadFinder.Analyze(CurrentBoard, currentColor);
            if (_roadFinder.Roads.Count > 0)
            {
                GameResult = currentColor == PieceColor.White ? GameResult.WhiteRoad : GameResult.BlackRoad;
                return;
            }
            _roadFinder.Analyze(CurrentBoard, otherColor);
            if (_roadFinder.Roads.Count > 0)
            {
                GameResult = otherColor == PieceColor.White ? GameResult.WhiteRoad : GameResult.BlackRoad;
                return;
            }

            if (CurrentBoard.EmptySpaces == 0 || CurrentBoard.EitherPlayerOutOfPieces)
            {
                if (CurrentBoard.FlatScore == 0) GameResult = GameResult.Draw;
                else GameResult = CurrentBoard.FlatScore > 0 ? GameResult.WhiteFlat : GameResult.BlackFlat;
                return;
            }

            GameResult = GameResult.Incomplete;
        }
    }
}
