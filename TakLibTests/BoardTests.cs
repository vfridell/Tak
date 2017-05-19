using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;
using TakLib;

namespace TakLibTests
{
    [TestClass]
    public class BoardTests
    {
        [TestMethod]
        public void PieceStackClone()
        {
            PieceStack stack = new PieceStack();
            stack.Push(new Piece(PieceColor.Black, PieceType.Stone));
            stack.Push(new Piece(PieceColor.White, PieceType.Stone));
            stack.Push(new Piece(PieceColor.White, PieceType.Stone));
            stack.Push(new Piece(PieceColor.Black, PieceType.Wall));

            var cloneStack = stack.Clone();
            stack.Clear();

            Assert.AreEqual(cloneStack.Pop(), new Piece(PieceColor.Black, PieceType.Wall));
            Assert.AreEqual(cloneStack.Pop(), new Piece(PieceColor.White, PieceType.Stone));
            Assert.AreEqual(cloneStack.Pop(), new Piece(PieceColor.White, PieceType.Stone));
            Assert.AreEqual(cloneStack.Pop(), new Piece(PieceColor.Black, PieceType.Stone));

        }

        [TestMethod]
        public void CornerCheck()
        {
            RoadFinder roadFinder = new RoadFinder(5);
            
            Assert.IsTrue(roadFinder.IsCorner(new Coordinate(0,0)));
            Assert.IsTrue(roadFinder.IsCorner(new Coordinate(0,4)));
            Assert.IsTrue(roadFinder.IsCorner(new Coordinate(4,4)));
            Assert.IsTrue(roadFinder.IsCorner(new Coordinate(4,0)));
            Assert.IsFalse(roadFinder.IsCorner(new Coordinate(0,1)));
            Assert.IsFalse(roadFinder.IsCorner(new Coordinate(1,1)));
            Assert.IsFalse(roadFinder.IsCorner(new Coordinate(3,4)));
        }

        [TestMethod]
        public void SideCheck()
        {
            RoadFinder roadFinder = new RoadFinder(5);

            Assert.IsTrue(roadFinder.IsSide(new Coordinate(0, 0)));
            Assert.IsTrue(roadFinder.IsSide(new Coordinate(0, 4)));
            Assert.IsTrue(roadFinder.IsSide(new Coordinate(4, 4)));
            Assert.IsTrue(roadFinder.IsSide(new Coordinate(4, 0)));
            Assert.IsTrue(roadFinder.IsSide(new Coordinate(0, 1)));
            Assert.IsFalse(roadFinder.IsSide(new Coordinate(1, 1)));
            Assert.IsTrue(roadFinder.IsSide(new Coordinate(3, 4)));
            Assert.IsFalse(roadFinder.IsSide(new Coordinate(2, 3)));
            Assert.IsFalse(roadFinder.IsSide(new Coordinate(3, 3)));

        }

        [TestMethod]
        public void WallsDoNotCountAsRoads()
        {
            GameSetup gameSetup = new GameSetup()
            {
                WhitePlayer = new Player() { Name = "Player1" },
                BlackPlayer = new Player() { Name = "Player2" },
                BoardSize = 5
            };

            Game game = Game.GetNewGame(gameSetup);
            game.ApplyMove(NotationParser.Parse("c2"));
            game.ApplyMove(NotationParser.Parse("c5"));
            game.ApplyMove(NotationParser.Parse("e1"));
            game.ApplyMove(NotationParser.Parse("Sc3"));
            game.ApplyMove(NotationParser.Parse("a1"));
            game.ApplyMove(NotationParser.Parse("d4"));
            game.ApplyMove(NotationParser.Parse("e5"));
            game.ApplyMove(NotationParser.Parse("Sd1"));
            game.ApplyMove(NotationParser.Parse("Cc1"));
            game.ApplyMove(NotationParser.Parse("1c3>1"));
            game.ApplyMove(NotationParser.Parse("d2"));
            game.ApplyMove(NotationParser.Parse("Se2"));
            game.ApplyMove(NotationParser.Parse("b1"));
            game.ApplyMove(NotationParser.Parse("Cc4"));
            game.ApplyMove(NotationParser.Parse("Sb2"));
            game.ApplyMove(NotationParser.Parse("Sa2"));
            game.ApplyMove(NotationParser.Parse("Sb3"));
            game.ApplyMove(NotationParser.Parse("e4"));
            game.ApplyMove(NotationParser.Parse("Sc3"));
            game.ApplyMove(NotationParser.Parse("1c4+1"));
            game.ApplyMove(NotationParser.Parse("Sb4"));
            game.ApplyMove(NotationParser.Parse("1d3+1"));
            game.ApplyMove(NotationParser.Parse("b5"));

            Assert.AreEqual(GameResult.Incomplete, game.GameResult);
        }

        [TestMethod]
        public void WallsCannotSmashWalls()
        {
            GameSetup gameSetup = new GameSetup()
            {
                WhitePlayer = new Player() { Name = "Player1" },
                BlackPlayer = new Player() { Name = "Player2" },
                BoardSize = 5
            };

            Game game = Game.GetNewGame(gameSetup);
            game.ApplyMove(NotationParser.Parse("c4"));
            game.ApplyMove(NotationParser.Parse("a4"));
            game.ApplyMove(NotationParser.Parse("Ce5"));
            game.ApplyMove(NotationParser.Parse("Ce2"));
            game.ApplyMove(NotationParser.Parse("e1"));
            game.ApplyMove(NotationParser.Parse("Sc5"));
            game.ApplyMove(NotationParser.Parse("a1"));
            game.ApplyMove(NotationParser.Parse("Sd2"));
            game.ApplyMove(NotationParser.Parse("b5"));
            game.ApplyMove(NotationParser.Parse("c4<"));
            game.ApplyMove(NotationParser.Parse("Sa5"));
            game.ApplyMove(NotationParser.Parse("d3"));
            game.ApplyMove(NotationParser.Parse("Sc4"));
            game.ApplyMove(NotationParser.Parse("Sb3"));
            game.ApplyMove(NotationParser.Parse("a3"));

            var moves = game.GetAllMoves();

            Assert.IsFalse(moves.Any(m => m.ToString() == "1c5-1"));
        }


    }
}
