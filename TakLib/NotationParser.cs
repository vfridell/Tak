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
        public static Regex IsPlaceCapstoneRegex = new Regex("^C([a-h])([1-8])");
        public static Regex IsPlaceWallRegex = new Regex("^S([a-h])([1-8])");
        public static Regex IsPlaceStoneRegex = new Regex("^F?([a-h])([1-8])");
        public static Regex IsMoveRegex = new Regex("^([1-8])([a-h])([1-8]+)([-+><])");

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

    }

    

}
