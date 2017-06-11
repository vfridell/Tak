using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TakLib;

namespace TakLibTests
{
    [TestClass]
    public class NotationParserTests
    {

        [TestMethod]
        public void ParseFullNotation()
        {
            Game game = Game.CreateGameFromTranscript("NotationFiles\\White vs Black 0-R 2017.02.17.ptn");
            Assert.AreEqual(GameResult.BlackRoad, game.GameResult);
        }

        [TestMethod]
        public void ParseNotationHeader()
        {
            string header = "[Date \"2017.05.25\"]\n[Player1 \"Superman\"]\n[Player2 \"Batman\"]\n[Result \"\"]\n[Size \"3\"]\n";
            GameSetup setup = NotationParser.ParseGameFileHeaderString(header);
            Assert.AreEqual(3, setup.BoardSize);
            Assert.AreEqual("Superman", setup.WhitePlayer.Name);
            Assert.AreEqual("Batman", setup.BlackPlayer.Name);
        }

        [TestMethod]
        public void ParseMoveLines()
        {
            string moveLines = @"1. e3 e1
1. c1 d2
2. b3 d3
3. a1 c2
4. b1 1c2-1
5. c5 c2
6. c3 d1
7. a2 d4
8. d5 e4
9. 1c3>1 1e3<1
10. e5 3d3<12
11. b5 a5
12. d3 1d2+1
13. a3 1a5>1
14. 1c5<1 c4
15. c5 1e4+1
16. 3b5-12
";
            List<Move> moves = NotationParser.ParseMoveLines(moveLines);
            Assert.AreEqual(33, moves.Count);
            Assert.AreEqual("3b5-12", moves[32].ToString());
        }

        [TestMethod]
        public void ParsePlacement()
        {
            Move move = NotationParser.Parse("a5");
            Assert.IsInstanceOfType(move, typeof(PlaceStone));

            Move move2 = NotationParser.Parse("Cd3");
            Assert.IsInstanceOfType(move2, typeof(PlaceCapstone));

            Move move3 = NotationParser.Parse("Sb2");
            Assert.IsInstanceOfType(move3, typeof(PlaceWall));
        }

        [TestMethod]
        public void ParseBasicMovement()
        {
            Move move = NotationParser.Parse("a5-");
            Assert.IsInstanceOfType(move, typeof(MoveStack));
            Assert.AreEqual(Direction.Down, (move as MoveStack).DirectionEnum);
            Assert.AreEqual(0, (move as MoveStack).Location.Column);
            Assert.AreEqual(4, (move as MoveStack).Location.Row);
        }

        [TestMethod]
        public void ParseStackMovement()
        {
            Move move = NotationParser.Parse("5e1+41");
            Assert.IsInstanceOfType(move, typeof(MoveStack));
            Assert.AreEqual(Direction.Up, (move as MoveStack).DirectionEnum);
            Assert.AreEqual(4, (move as MoveStack).Location.Column);
            Assert.AreEqual(0, (move as MoveStack).Location.Row);
            Assert.AreEqual(5, (move as MoveStack).CarryInt);
            Assert.AreEqual(2, (move as MoveStack).DropInts.Count);
            Assert.AreEqual(4, (move as MoveStack).DropInts[0]);
            Assert.AreEqual(1, (move as MoveStack).DropInts[1]);
        }

        [TestMethod]
        public void ParseRow()
        {
            Assert.AreEqual(0, NotationParser.ToRow('1'));
            Assert.AreEqual(1, NotationParser.ToRow('2'));
            Assert.AreEqual(2, NotationParser.ToRow('3'));
            Assert.AreEqual(3, NotationParser.ToRow('4'));
            Assert.AreEqual(4, NotationParser.ToRow('5'));
            Assert.AreEqual(5, NotationParser.ToRow('6'));
            Assert.AreEqual(6, NotationParser.ToRow('7'));
            Assert.AreEqual(7, NotationParser.ToRow('8'));
        }

        [TestMethod]
        public void ParseColumn()
        {
            Assert.AreEqual(0, NotationParser.ToColumn('a'));
            Assert.AreEqual(1, NotationParser.ToColumn('b'));
            Assert.AreEqual(2, NotationParser.ToColumn('c'));
            Assert.AreEqual(3, NotationParser.ToColumn('d'));
            Assert.AreEqual(4, NotationParser.ToColumn('e'));
            Assert.AreEqual(5, NotationParser.ToColumn('f'));
            Assert.AreEqual(6, NotationParser.ToColumn('g'));
            Assert.AreEqual(7, NotationParser.ToColumn('h'));
            Assert.AreEqual(0, NotationParser.ToColumn('A'));
            Assert.AreEqual(1, NotationParser.ToColumn('B'));
            Assert.AreEqual(2, NotationParser.ToColumn('C'));
            Assert.AreEqual(3, NotationParser.ToColumn('D'));
            Assert.AreEqual(4, NotationParser.ToColumn('E'));
            Assert.AreEqual(5, NotationParser.ToColumn('F'));
            Assert.AreEqual(6, NotationParser.ToColumn('G'));
            Assert.AreEqual(7, NotationParser.ToColumn('H'));
        }
    }
}
