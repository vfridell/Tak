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
        public void SpaceEquality()
        {
            Piece whiteStone = new Piece(PieceColor.White, PieceType.Stone);
            Piece whiteWall = new Piece(PieceColor.White, PieceType.Wall);
            Piece whiteCap = new Piece(PieceColor.White, PieceType.CapStone);
            Piece blackStone = new Piece(PieceColor.Black, PieceType.Stone);
            Piece blackWall = new Piece(PieceColor.Black, PieceType.Wall);
            Piece blackCap = new Piece(PieceColor.Black, PieceType.CapStone);

            PieceStack stack1 = new PieceStack();
            stack1.Push(blackStone);
            stack1.Push(blackStone);
            stack1.Push(whiteStone);
            stack1.Push(whiteWall);

            PieceStack stack2 = new PieceStack();
            stack2.Push(blackStone);
            stack2.Push(whiteStone);
            stack2.Push(whiteStone);

            Space space1 = new Space(stack1, new Coordinate(1, 1), true);
            Space space2 = new Space(stack2, new Coordinate(1, 1), true);
            Space space3 = new Space(stack2, new Coordinate(1, 2), true);
            Assert.AreNotEqual(space1, space2);
            Assert.AreNotEqual(space2, space3);

            stack2.Push(whiteWall);
            Space space4 = new Space(stack2, new Coordinate(1, 1), true);
            Assert.AreEqual(space1, space4);
        }


        [TestMethod]
        public void StackEquality()
        {
            Piece whiteStone = new Piece(PieceColor.White, PieceType.Stone);
            Piece whiteWall = new Piece(PieceColor.White, PieceType.Wall);
            Piece whiteCap = new Piece(PieceColor.White, PieceType.CapStone);
            Piece blackStone = new Piece(PieceColor.Black, PieceType.Stone);
            Piece blackWall = new Piece(PieceColor.Black, PieceType.Wall);
            Piece blackCap = new Piece(PieceColor.Black, PieceType.CapStone);

            PieceStack stack1 = new PieceStack();
            stack1.Push(blackStone);
            stack1.Push(blackStone);
            stack1.Push(whiteStone);
            stack1.Push(whiteWall);

            PieceStack stack2 = new PieceStack();
            stack2.Push(blackStone);
            stack2.Push(whiteStone);
            stack2.Push(whiteStone);

            PieceStack stack3 = new PieceStack();
            stack3.Push(blackStone);
            stack3.Push(blackStone);
            stack3.Push(whiteStone);

            PieceStack stack4 = new PieceStack();
            stack4.Push(whiteStone);
            stack4.Push(whiteStone);
            stack4.Push(blackStone);
            stack4.Push(whiteWall);

            PieceStack stack5 = new PieceStack();
            PieceStack stack6 = new PieceStack();

            Assert.AreNotEqual(stack1, stack2);
            Assert.AreNotEqual(stack1, stack3);
            Assert.AreNotEqual(stack1, stack4);
            Assert.AreNotEqual(stack1, stack5);

            stack3.Push(whiteWall);
            Assert.AreEqual(stack1, stack3);
            Assert.AreEqual(stack5, stack6);
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
        public void BoardEquality()
        {
            GameSetup gameSetup = new GameSetup()
            {
                WhitePlayer = new Player() { Name = "Player1" },
                BlackPlayer = new Player() { Name = "Player2" },
                BoardSize = 5
            };

            Game game = Game.GetNewGame(gameSetup);
            game.ApplyMove(NotationParser.Parse("c4"));
            game.ApplyMove(NotationParser.Parse("c3"));
            Board board1 = game.CurrentBoard.Clone();
            game.ApplyMove(NotationParser.Parse("c3>"));
            Assert.AreNotEqual(board1, game.CurrentBoard);
            game.ApplyMove(NotationParser.Parse("c4-"));
            Assert.AreNotEqual(board1, game.CurrentBoard);
            game.ApplyMove(NotationParser.Parse("d3+"));
            Assert.AreNotEqual(board1, game.CurrentBoard);
            game.ApplyMove(NotationParser.Parse("c3>"));
            Assert.AreNotEqual(board1, game.CurrentBoard);
            game.ApplyMove(NotationParser.Parse("d4<"));
            Assert.AreNotEqual(board1, game.CurrentBoard);
            game.ApplyMove(NotationParser.Parse("d3+"));
            Assert.AreNotEqual(board1, game.CurrentBoard);
            game.ApplyMove(NotationParser.Parse("c4-"));
            Assert.AreNotEqual(board1, game.CurrentBoard);
            game.ApplyMove(NotationParser.Parse("d4<"));
            Assert.AreEqual(board1, game.CurrentBoard); // back to original position but at a later turn
            game.ApplyMove(NotationParser.Parse("b2"));
            Assert.AreNotEqual(board1, game.CurrentBoard);
            game.ApplyMove(NotationParser.Parse("d4"));
            Assert.AreNotEqual(board1, game.CurrentBoard);
            game.ApplyMove(NotationParser.Parse("c2"));
            game.ApplyMove(NotationParser.Parse("d5"));
            game.ApplyMove(NotationParser.Parse("b2>"));
            game.ApplyMove(NotationParser.Parse("d5-"));
            game.ApplyMove(NotationParser.Parse("2c2+"));
            game.ApplyMove(NotationParser.Parse("2d4<"));
            Assert.AreNotEqual(board1, game.CurrentBoard);  // different due to stacking
            Board board2 = game.CurrentBoard.Clone();
            game.ApplyMove(NotationParser.Parse("3c3>3"));
            game.ApplyMove(NotationParser.Parse("3c4>3"));
            game.ApplyMove(NotationParser.Parse("3d3<3"));
            game.ApplyMove(NotationParser.Parse("3d4<3"));
            Assert.AreEqual(board2, game.CurrentBoard); // back to original position but at a later turn

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
