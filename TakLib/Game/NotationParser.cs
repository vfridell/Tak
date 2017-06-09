using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Permissions;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace TakLib
{
    // ASCII ints
    public enum Direction
    {
        Left = 60,  // <
        Right = 62, // >
        Up = 43,    // +
        Down = 45   // -
    }

    public static class NotationParser
    {

        public static int ToColumn(char c) => char.ToLower(c) - 97;
        public static int ToRow(char c) => int.Parse($"{c}") - 1;
        public static Coordinate ToCoordinateDirection(Direction d)
        {
            switch (d)
            {
                case Direction.Left: return new Coordinate(0,-1);
                case Direction.Right: return new Coordinate(0,1);
                case Direction.Up: return new Coordinate(1,0);
                case Direction.Down: return new Coordinate(-1,0);
                default: throw new Exception("Unrecognized direction");
            }
        }

        public static char ToCharRow(Coordinate coordinate) => (coordinate.Row + 1).ToString()[0];
        public static char ToCharColumn(Coordinate coordinate) => (char)(coordinate.Column + 97);

        public static Regex PlaceCapstoneRegex = new Regex("^C([a-h])([1-8])");
        public static Regex PlaceWallRegex = new Regex("^S([a-h])([1-8])");
        public static Regex PlaceStoneRegex = new Regex("^F?([a-h])([1-8])");
        public static Regex MoveRegex = new Regex("^([1-8]?)([a-h])([1-8])([-+><])([1-8]*)");
        public static Move Parse(string notation)
        {
            if (MoveRegex.IsMatch(notation))
            {
                Match match = MoveRegex.Matches(notation)[0];
                char carry = match.Groups[1].Length > 0 ? match.Groups[1].Value[0] : '1';
                char column = match.Groups[2].Value[0];
                char row = match.Groups[3].Value[0];
                char direction = match.Groups[4].Value[0];
                string drops = match.Groups[5].Length > 0 ? match.Groups[5].Value : "1";
                return new MoveStack(carry, row, column, drops.ToCharArray(), direction);
            }
            else if (PlaceStoneRegex.IsMatch(notation))
            {
                Match match = PlaceStoneRegex.Matches(notation)[0];
                char column = match.Groups[1].Value[0];
                char row = match.Groups[2].Value[0];
                return new PlaceStone(row, column);
            }
            else if (PlaceWallRegex.IsMatch(notation))
            {
                Match match = PlaceWallRegex.Matches(notation)[0];
                char column = match.Groups[1].Value[0];
                char row = match.Groups[2].Value[0];
                return new PlaceWall(row, column);
                
            }
            else if (PlaceCapstoneRegex.IsMatch(notation))
            {
                Match match = PlaceCapstoneRegex.Matches(notation)[0];
                char column = match.Groups[1].Value[0];
                char row = match.Groups[2].Value[0];
                return new PlaceCapstone(row, column);
            }
            else { throw new Exception($"Unrecognized notation: {notation}");}
        }

        public static Regex MoveLineRegex = new Regex(@"^[0-9]+\.\s+(\S+)\s+(\S+)", RegexOptions.Multiline);
        public static Regex MoveLineRegexOnlyOne = new Regex(@"^[0-9]+\.\s+(\S+)$");

        public static List<Move> ParseMoveLines(string fullNotation)
        {
            List<Move> moves = new List<Move>();
            MatchCollection matchCollection = MoveLineRegex.Matches(fullNotation);
            foreach (Match match in matchCollection)
            {
                string move1 = match.Groups[1].Value;
                string move2 = match.Groups[2].Value;
                moves.Add(Parse(move1));
                moves.Add(Parse(move2));
            }
            Match matchOne = MoveLineRegexOnlyOne.Match(fullNotation);
            if(matchOne.Groups.Count > 1) moves.Add(Parse(matchOne.Groups[1].Value));
            return moves;
        }

        public static Regex HeaderRegex = new Regex(@"\[(Date|Player1|Player2|Result|Size) ""(.*)""\]");
        public static GameSetup ParseGameFileHeaderString(string fullNotation)
        {
            //[Date "2017.05.25"]
            //[Player1 "White"]
            //[Player2 "Black"]
            //[Result ""]
            //[Size "5"]
            GameSetup setup = new GameSetup();
            MatchCollection matchCollection = HeaderRegex.Matches(fullNotation);
            foreach (Match match in matchCollection)
            {
                string key = match.Groups[1].Value;
                string value = match.Groups[2].Value;
                switch (key)
                {
                    case "Date":
                    case "Result":
                    break;
                    case "Player1":
                        setup.WhitePlayer = new Player() {Name = value};
                        break;
                    case "Player2":
                        setup.BlackPlayer = new Player() {Name = value};
                        break;
                    case "Size":
                        setup.BoardSize = int.Parse(value);
                        break;
                    default:
                        break;
                }
            }
            //if(setup.BoardSize == 0) throw new ArgumentException("Missing board size in notation");
            if (setup.BoardSize == 0) setup.BoardSize = 5;
            if(setup.WhitePlayer == null) setup.WhitePlayer = new Player() {Name = "White"};
            if(setup.BlackPlayer == null) setup.BlackPlayer = new Player() {Name = "Black"};
            return setup;
        }
    }

    

}
