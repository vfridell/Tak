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
        private List<Board> _boards;
        private Player _whitePlayer;
        private Player _blackPlayer;

        private List<Move> _movesMade = new List<Move>();
        public IReadOnlyList<Move> movesMade { get { return _movesMade.AsReadOnly(); } }
        public IReadOnlyList<Board> Boards { get { return _boards.AsReadOnly(); } }

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
            _movesMade.Add(move);
            EndPlayerMove();
        }

        public void EndPlayerMove()
        {
            GameResult = CurrentBoard.GameResult;
            CurrentBoard.EndPlayerMove();
            if (CurrentBoard.Round % 2 != 0) EndTurn();
            _boards.Add(CurrentBoard.Clone());
        }

        public void EndTurn()
        {
            CurrentBoard.EndTurn();
        }



        public string GetMoveTranscript()
        {
            StringBuilder sb = new StringBuilder();
            int move = 1;
            int round = 1;
            while(move <= _movesMade.Count)
            {
                string roundNumber = $"{round}. ";
                sb.Append(roundNumber).Append(_movesMade[move-1]).Append(" ");
                move++;
                if (move > _movesMade.Count)
                {
                    sb.Append("\n");
                    break;
                }
                sb.Append(_movesMade[move-1]).Append("\n");
                move++;
                round++;
            }
            return sb.ToString();
        }

        public string GetGameHeaderTranscript()
        {
            //[Date "2017.05.25"]
            //[Player1 "White"]
            //[Player2 "Black"]
            //[Result ""]
            //[Size "5"]
            string resultString = "";
            if (GameResult == GameResult.BlackFlat) resultString = "0-F";
            if (GameResult == GameResult.BlackRoad) resultString = "0-R";
            if (GameResult == GameResult.WhiteFlat) resultString = "F-0";
            if (GameResult == GameResult.WhiteRoad) resultString = "R-0";
            if (GameResult == GameResult.Draw) resultString = "0-0-0";

            return $"[Date \"{DateTime.Today:yyyy.MM.dd}\"]\n" + 
                    $"[Player1 \"{WhitePlayer.Name}\"]\n" +
                    $"[Player2 \"{BlackPlayer.Name}\"]\n" +
                    $"[Result \"{resultString}\"]\n" +
                    $"[Size \"{CurrentBoard.Size}\"]\n";
        }

        public static string WriteGameTranscript(Game game)
        {
            string filename = string.Format("transcript_{0}", DateTime.Now.ToString("yyyy.MM.dd.HHmmss"));
            using (System.IO.StreamWriter writer = new System.IO.StreamWriter(filename + ".txt"))
            {
                writer.Write(game.GetGameHeaderTranscript());
                writer.Write(game.GetMoveTranscript());
            }

            //BinaryFormatter formatter = new BinaryFormatter();
            //using (Stream stream = new FileStream(filename + ".bin", FileMode.Create, FileAccess.Write, FileShare.None))
            //{
            //    formatter.Serialize(stream, game);
            //}
            return filename;
        }

        public static Game CreateGameFromTranscript(string filename)
        {
            string fullNotation;
            using (System.IO.StreamReader reader = new System.IO.StreamReader(filename))
            {
                fullNotation = reader.ReadToEnd();
            }

            GameSetup setup = NotationParser.ParseGameFileHeaderString(fullNotation);
            Game loadedGame = GetNewGame(setup);
            List<Move> moves = NotationParser.ParseMoveLines(fullNotation);
            foreach (Move move in moves)
            {
                loadedGame.ApplyMove(move);
            }
            return loadedGame;
        }
    }
}
