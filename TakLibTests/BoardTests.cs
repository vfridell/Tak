using System;
using System.Collections.Generic;
using System.IO;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
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
        public void StackCounts()
        {
            Piece whiteStone = new Piece(PieceColor.White, PieceType.Stone);
            Piece whiteWall = new Piece(PieceColor.White, PieceType.Wall);
            Piece whiteCap = new Piece(PieceColor.White, PieceType.CapStone);
            Piece blackStone = new Piece(PieceColor.Black, PieceType.Stone);
            Piece blackWall = new Piece(PieceColor.Black, PieceType.Wall);
            Piece blackCap = new Piece(PieceColor.Black, PieceType.CapStone);

            PieceStack stack1 = new PieceStack();
            stack1.Push(blackStone);
            Assert.AreEqual(1, stack1.OwnerPieceCount);
            Assert.AreEqual(0, stack1.CapturedPieceCount);
            stack1.Push(blackStone);
            Assert.AreEqual(2, stack1.OwnerPieceCount);
            Assert.AreEqual(0, stack1.CapturedPieceCount);
            stack1.Push(whiteStone);
            Assert.AreEqual(1, stack1.OwnerPieceCount);
            Assert.AreEqual(2, stack1.CapturedPieceCount);
            stack1.Push(whiteWall);
            Assert.AreEqual(2, stack1.OwnerPieceCount);
            Assert.AreEqual(2, stack1.CapturedPieceCount);

            PieceStack stack2 = new PieceStack();
            stack2.Push(blackStone);
            stack2.Push(whiteStone);
            stack2.Push(blackCap);
            Assert.AreEqual(2, stack2.OwnerPieceCount);
            Assert.AreEqual(1, stack2.CapturedPieceCount);
            stack2.Pop();
            Assert.AreEqual(1, stack2.OwnerPieceCount);
            Assert.AreEqual(1, stack2.CapturedPieceCount);
            stack2.Pop();
            Assert.AreEqual(1, stack2.OwnerPieceCount);
            Assert.AreEqual(0, stack2.CapturedPieceCount);

            PieceStack stack3 = new PieceStack();
            stack3.Push(blackStone);
            stack3.Push(blackStone);
            stack3.Push(blackStone);
            Assert.AreEqual(3, stack3.OwnerPieceCount);
            Assert.AreEqual(0, stack3.CapturedPieceCount);

            PieceStack stack4 = new PieceStack();
            stack4.Push(whiteStone);
            stack4.Push(whiteStone);
            stack4.Push(whiteStone);
            stack4.Push(whiteStone);
            stack4.Push(whiteStone);
            stack4.Push(blackStone);
            Assert.AreEqual(1, stack4.OwnerPieceCount);
            Assert.AreEqual(5, stack4.CapturedPieceCount);

            PieceStack stack5 = new PieceStack();
            Assert.AreEqual(0, stack5.OwnerPieceCount);
            Assert.AreEqual(0, stack5.CapturedPieceCount);
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
        public void BoardSerialize()
        {
            GameSetup gameSetup = new GameSetup()
            {
                WhitePlayer = new Player() { Name = "Player1" },
                BlackPlayer = new Player() { Name = "Player2" },
                BoardSize = 5
            };

            Game game = Game.GetNewGame(gameSetup);
            game.ApplyMove(NotationParser.Parse("a5"));
            game.ApplyMove(NotationParser.Parse("e1"));
            game.ApplyMove(NotationParser.Parse("b4"));
            game.ApplyMove(NotationParser.Parse("c2"));
            game.ApplyMove(NotationParser.Parse("d4"));
            game.ApplyMove(NotationParser.Parse("c3"));
            game.ApplyMove(NotationParser.Parse("a4"));
            game.ApplyMove(NotationParser.Parse("a5-"));
            game.ApplyMove(NotationParser.Parse("b4<"));
            game.ApplyMove(NotationParser.Parse("e3"));
            game.ApplyMove(NotationParser.Parse("2a4>"));
            game.ApplyMove(NotationParser.Parse("a2"));
            game.ApplyMove(NotationParser.Parse("Sb2"));
            game.ApplyMove(NotationParser.Parse("c5"));
            game.ApplyMove(NotationParser.Parse("e4"));
            game.ApplyMove(NotationParser.Parse("Sc4"));
            game.ApplyMove(NotationParser.Parse("Cd3"));
            game.ApplyMove(NotationParser.Parse("c4<"));
            game.ApplyMove(NotationParser.Parse("e5"));
            game.ApplyMove(NotationParser.Parse("c1"));
            game.ApplyMove(NotationParser.Parse("b2>"));
            game.ApplyMove(NotationParser.Parse("c4"));
            game.ApplyMove(NotationParser.Parse("e5-"));
            game.ApplyMove(NotationParser.Parse("Cb2"));
            game.ApplyMove(NotationParser.Parse("c2-"));
            game.ApplyMove(NotationParser.Parse("e2"));
            game.ApplyMove(NotationParser.Parse("Sd2"));
            game.ApplyMove(NotationParser.Parse("b2>"));
            game.ApplyMove(NotationParser.Parse("2e4<11"));
            game.ApplyMove(NotationParser.Parse("c2-"));
            game.ApplyMove(NotationParser.Parse("d4>"));
            game.ApplyMove(NotationParser.Parse("b5"));
            game.ApplyMove(NotationParser.Parse("d3<"));
            game.ApplyMove(NotationParser.Parse("2c1+"));
            game.ApplyMove(NotationParser.Parse("2c3+"));
            game.ApplyMove(NotationParser.Parse("e3+"));
            game.ApplyMove(NotationParser.Parse("c3"));
            game.ApplyMove(NotationParser.Parse("3c2+"));
            game.ApplyMove(NotationParser.Parse("d1"));
            game.ApplyMove(NotationParser.Parse("2c3-"));
            game.ApplyMove(NotationParser.Parse("Sb1"));
            game.ApplyMove(NotationParser.Parse("e5"));
            game.ApplyMove(NotationParser.Parse("b1>"));
            game.ApplyMove(NotationParser.Parse("c2-"));
            game.ApplyMove(NotationParser.Parse("d2<"));
            game.ApplyMove(NotationParser.Parse("e3"));
            game.ApplyMove(NotationParser.Parse("2c2>11"));
            game.ApplyMove(NotationParser.Parse("c2"));
            game.ApplyMove(NotationParser.Parse("4c4-"));
            game.ApplyMove(NotationParser.Parse("2e4<"));
            game.ApplyMove(NotationParser.Parse("b3"));
            game.ApplyMove(NotationParser.Parse("3d4+"));
            game.ApplyMove(NotationParser.Parse("Sa5"));
            game.ApplyMove(NotationParser.Parse("3c1>"));
            game.ApplyMove(NotationParser.Parse("a3"));
            game.ApplyMove(NotationParser.Parse("c1"));
            game.ApplyMove(NotationParser.Parse("d3"));
            game.ApplyMove(NotationParser.Parse("4d1>"));
            game.ApplyMove(NotationParser.Parse("d2+"));
            game.ApplyMove(NotationParser.Parse("e3<"));
            game.ApplyMove(NotationParser.Parse("2e2+"));
            game.ApplyMove(NotationParser.Parse("5e1+41"));
            game.ApplyMove(NotationParser.Parse("2c3>"));
            game.ApplyMove(NotationParser.Parse("Sd2"));
            game.ApplyMove(NotationParser.Parse("c4"));
            game.ApplyMove(NotationParser.Parse("3b4>"));
            game.ApplyMove(NotationParser.Parse("a5>"));
            game.ApplyMove(NotationParser.Parse("c5>"));
            game.ApplyMove(NotationParser.Parse("a5"));
            game.ApplyMove(NotationParser.Parse("e4"));
            game.ApplyMove(NotationParser.Parse("4d3+"));
            game.ApplyMove(NotationParser.Parse("3e3+"));

            Assert.AreEqual(GameResult.Incomplete, game.GameResult);
            IFormatter formatter = new BinaryFormatter();
            MemoryStream stream = new MemoryStream();
            formatter.Serialize(stream, game.CurrentBoard);
            byte[] serializedBytes = stream.GetBuffer();

            MemoryStream stream2 = new MemoryStream(serializedBytes);
            Board deserializedBoard = (Board)formatter.Deserialize(stream2);
            Assert.AreEqual(GameResult.Incomplete, deserializedBoard.GameResult);
            Assert.AreEqual(game.CurrentBoard, deserializedBoard);

            game.ApplyMove(NotationParser.Parse("e3"));
            Assert.AreEqual(GameResult.WhiteRoad, game.GameResult);

            NotationParser.Parse("e3").Apply(deserializedBoard);
            Assert.AreEqual(GameResult.WhiteRoad, deserializedBoard.GameResult);
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

        [TestMethod]
        public void BoardCongruence0()
        {
            GameSetup gameSetup = new GameSetup()
            {
                WhitePlayer = new Player() { Name = "Player1" },
                BlackPlayer = new Player() { Name = "Player2" },
                BoardSize = 5
            };

            Game game = Game.GetNewGame(gameSetup);
            game.ApplyMove(NotationParser.Parse("b4"));
            game.ApplyMove(NotationParser.Parse("d2"));
            game.ApplyMove(NotationParser.Parse("e4"));
            game.ApplyMove(NotationParser.Parse("b1"));

            var board1 = game.CurrentBoard.Clone();
            Move move1 = NotationParser.Parse("c3");
            move1.Apply(board1);

            var board2 = game.CurrentBoard.Clone();
            Move move2 = NotationParser.Parse("e1");
            move2.Apply(board2);

            Assert.IsFalse(Board.IsCongruent(board1, board2));
        }

        [TestMethod]
        public void BoardCongruence0_Dict()
        {
            GameSetup gameSetup = new GameSetup()
            {
                WhitePlayer = new Player() { Name = "Player1" },
                BlackPlayer = new Player() { Name = "Player2" },
                BoardSize = 5
            };

            Game game = Game.GetNewGame(gameSetup);
            game.ApplyMove(NotationParser.Parse("b4"));
            game.ApplyMove(NotationParser.Parse("d2"));
            game.ApplyMove(NotationParser.Parse("e4"));
            game.ApplyMove(NotationParser.Parse("b1"));

            var congruenceDict = game.CurrentBoard.GetCongruenceDictionary();
            Assert.IsFalse(congruenceDict[1]);
            Assert.IsFalse(congruenceDict[2]);
            Assert.IsFalse(congruenceDict[3]);
            Assert.IsFalse(congruenceDict[4]);
            Assert.IsFalse(congruenceDict[5]);
            Assert.IsFalse(congruenceDict[6]);
            Assert.IsFalse(congruenceDict[7]);
        }

        [TestMethod]
        public void BoardCongruence1()
        {
            GameSetup gameSetup = new GameSetup()
            {
                WhitePlayer = new Player() { Name = "Player1" },
                BlackPlayer = new Player() { Name = "Player2" },
                BoardSize = 5
            };

            Game game = Game.GetNewGame(gameSetup);
            game.ApplyMove(NotationParser.Parse("a5"));
            game.ApplyMove(NotationParser.Parse("a3"));
            game.ApplyMove(NotationParser.Parse("a1"));
            game.ApplyMove(NotationParser.Parse("e5"));
            game.ApplyMove(NotationParser.Parse("e3"));
            game.ApplyMove(NotationParser.Parse("d5"));
            game.ApplyMove(NotationParser.Parse("e1"));
            game.ApplyMove(NotationParser.Parse("b5"));
            game.ApplyMove(NotationParser.Parse("b3"));
            game.ApplyMove(NotationParser.Parse("1b5<1"));
            game.ApplyMove(NotationParser.Parse("d3"));
            game.ApplyMove(NotationParser.Parse("1d5>1"));
            game.ApplyMove(NotationParser.Parse("c4"));
            game.ApplyMove(NotationParser.Parse("c3"));
            game.ApplyMove(NotationParser.Parse("c2"));

            var board1 = game.CurrentBoard.Clone();
            Move move1 = NotationParser.Parse("d4");
            move1.Apply(board1);

            var board2 = game.CurrentBoard.Clone();
            Move move2 = NotationParser.Parse("b4");
            move2.Apply(board2);

            Assert.IsTrue(Board.IsCongruent(board1, board2));
        }

        [TestMethod]
        public void BoardCongruence1_Dict()
        {
            GameSetup gameSetup = new GameSetup()
            {
                WhitePlayer = new Player() { Name = "Player1" },
                BlackPlayer = new Player() { Name = "Player2" },
                BoardSize = 5
            };

            Game game = Game.GetNewGame(gameSetup);
            game.ApplyMove(NotationParser.Parse("a5"));
            game.ApplyMove(NotationParser.Parse("a3"));
            game.ApplyMove(NotationParser.Parse("a1"));
            game.ApplyMove(NotationParser.Parse("e5"));
            game.ApplyMove(NotationParser.Parse("e3"));
            game.ApplyMove(NotationParser.Parse("d5"));
            game.ApplyMove(NotationParser.Parse("e1"));
            game.ApplyMove(NotationParser.Parse("b5"));
            game.ApplyMove(NotationParser.Parse("b3"));
            game.ApplyMove(NotationParser.Parse("1b5<1"));
            game.ApplyMove(NotationParser.Parse("d3"));
            game.ApplyMove(NotationParser.Parse("1d5>1"));
            game.ApplyMove(NotationParser.Parse("c4"));
            game.ApplyMove(NotationParser.Parse("c3"));
            game.ApplyMove(NotationParser.Parse("c2"));

            var congruenceDict = game.CurrentBoard.GetCongruenceDictionary();
            Assert.IsTrue(congruenceDict[1]);
            Assert.IsFalse(congruenceDict[2]);
            Assert.IsFalse(congruenceDict[3]);
            Assert.IsFalse(congruenceDict[4]);
            Assert.IsFalse(congruenceDict[5]);
            Assert.IsFalse(congruenceDict[6]);
            Assert.IsFalse(congruenceDict[7]);
        }

        [TestMethod]
        public void BoardCongruence2()
        {
            GameSetup gameSetup = new GameSetup()
            {
                WhitePlayer = new Player() { Name = "Player1" },
                BlackPlayer = new Player() { Name = "Player2" },
                BoardSize = 5
            };

            Game game = Game.GetNewGame(gameSetup);
            game.ApplyMove(NotationParser.Parse("a2"));
            game.ApplyMove(NotationParser.Parse("e2"));
            game.ApplyMove(NotationParser.Parse("e4"));
            game.ApplyMove(NotationParser.Parse("a4"));
            game.ApplyMove(NotationParser.Parse("b3"));
            game.ApplyMove(NotationParser.Parse("e3"));

            var board1 = game.CurrentBoard.Clone();
            Move move1 = NotationParser.Parse("d4");
            move1.Apply(board1);

            var board2 = game.CurrentBoard.Clone();
            Move move2 = NotationParser.Parse("d2");
            move2.Apply(board2);

            Assert.IsTrue(Board.IsCongruent(board1, board2));
        }

        [TestMethod]
        public void BoardCongruence2_Dict()
        {
            GameSetup gameSetup = new GameSetup()
            {
                WhitePlayer = new Player() { Name = "Player1" },
                BlackPlayer = new Player() { Name = "Player2" },
                BoardSize = 5
            };

            Game game = Game.GetNewGame(gameSetup);
            game.ApplyMove(NotationParser.Parse("a2"));
            game.ApplyMove(NotationParser.Parse("e2"));
            game.ApplyMove(NotationParser.Parse("e4"));
            game.ApplyMove(NotationParser.Parse("a4"));
            game.ApplyMove(NotationParser.Parse("b3"));
            game.ApplyMove(NotationParser.Parse("e3"));

            var congruenceDict = game.CurrentBoard.GetCongruenceDictionary();
            Assert.IsFalse(congruenceDict[1]);
            Assert.IsTrue(congruenceDict[2]);
            Assert.IsFalse(congruenceDict[3]);
            Assert.IsFalse(congruenceDict[4]);
            Assert.IsFalse(congruenceDict[5]);
            Assert.IsFalse(congruenceDict[6]);
            Assert.IsFalse(congruenceDict[7]);
        }

        [TestMethod]
        public void BoardCongruence3()
        {
            GameSetup gameSetup = new GameSetup()
            {
                WhitePlayer = new Player() { Name = "Player1" },
                BlackPlayer = new Player() { Name = "Player2" },
                BoardSize = 5
            };

            Game game = Game.GetNewGame(gameSetup);
            game.ApplyMove(NotationParser.Parse("d5"));
            game.ApplyMove(NotationParser.Parse("a2"));
            game.ApplyMove(NotationParser.Parse("b1"));
            game.ApplyMove(NotationParser.Parse("e4"));
            game.ApplyMove(NotationParser.Parse("c3"));
            game.ApplyMove(NotationParser.Parse("b4"));
            game.ApplyMove(NotationParser.Parse("b2"));
            game.ApplyMove(NotationParser.Parse("d2"));

            var board1 = game.CurrentBoard.Clone();
            Move move1 = NotationParser.Parse("c1");
            move1.Apply(board1);

            var board2 = game.CurrentBoard.Clone();
            Move move2 = NotationParser.Parse("a3");
            move2.Apply(board2);

            Assert.IsTrue(Board.IsCongruent(board1, board2));
        }

        [TestMethod]
        public void BoardCongruence3_Dict()
        {
            GameSetup gameSetup = new GameSetup()
            {
                WhitePlayer = new Player() { Name = "Player1" },
                BlackPlayer = new Player() { Name = "Player2" },
                BoardSize = 5
            };

            Game game = Game.GetNewGame(gameSetup);
            game.ApplyMove(NotationParser.Parse("d5"));
            game.ApplyMove(NotationParser.Parse("a2"));
            game.ApplyMove(NotationParser.Parse("b1"));
            game.ApplyMove(NotationParser.Parse("e4"));
            game.ApplyMove(NotationParser.Parse("c3"));
            game.ApplyMove(NotationParser.Parse("b4"));
            game.ApplyMove(NotationParser.Parse("b2"));
            game.ApplyMove(NotationParser.Parse("d2"));

            var congruenceDict = game.CurrentBoard.GetCongruenceDictionary();
            Assert.IsFalse(congruenceDict[1]);
            Assert.IsFalse(congruenceDict[2]);
            Assert.IsTrue(congruenceDict[3]);
            Assert.IsFalse(congruenceDict[4]);
            Assert.IsFalse(congruenceDict[5]);
            Assert.IsFalse(congruenceDict[6]);
            Assert.IsFalse(congruenceDict[7]);


            Game game2 = Game.GetNewGame(gameSetup);
            game2.ApplyMove(NotationParser.Parse("b1"));

            congruenceDict = game2.CurrentBoard.GetCongruenceDictionary();
            Assert.IsFalse(congruenceDict[1]);
            Assert.IsFalse(congruenceDict[2]);
            Assert.IsFalse(congruenceDict[3]);
            Assert.IsFalse(congruenceDict[4]);
            Assert.IsFalse(congruenceDict[5]);
            Assert.IsFalse(congruenceDict[6]);
            Assert.IsFalse(congruenceDict[7]);
        }

        [TestMethod]
        public void BoardCongruence4()
        {
            GameSetup gameSetup = new GameSetup()
            {
                WhitePlayer = new Player() { Name = "Player1" },
                BlackPlayer = new Player() { Name = "Player2" },
                BoardSize = 5
            };

            Game game = Game.GetNewGame(gameSetup);
            game.ApplyMove(NotationParser.Parse("b5"));
            game.ApplyMove(NotationParser.Parse("d3"));
            game.ApplyMove(NotationParser.Parse("c2"));
            game.ApplyMove(NotationParser.Parse("a4"));
            game.ApplyMove(NotationParser.Parse("d2"));
            game.ApplyMove(NotationParser.Parse("b4"));

            var board1 = game.CurrentBoard.Clone();
            Move move1 = NotationParser.Parse("a1");
            move1.Apply(board1);

            var board2 = game.CurrentBoard.Clone();
            Move move2 = NotationParser.Parse("e5");
            move2.Apply(board2);

            Assert.IsTrue(Board.IsCongruent(board1, board2));
        }

        [TestMethod]
        public void BoardCongruence4_Dict()
        {
            GameSetup gameSetup = new GameSetup()
            {
                WhitePlayer = new Player() { Name = "Player1" },
                BlackPlayer = new Player() { Name = "Player2" },
                BoardSize = 5
            };

            Game game = Game.GetNewGame(gameSetup);
            game.ApplyMove(NotationParser.Parse("b5"));
            game.ApplyMove(NotationParser.Parse("d3"));
            game.ApplyMove(NotationParser.Parse("c2"));
            game.ApplyMove(NotationParser.Parse("a4"));
            game.ApplyMove(NotationParser.Parse("d2"));
            game.ApplyMove(NotationParser.Parse("b4"));

            var congruenceDict = game.CurrentBoard.GetCongruenceDictionary();
            Assert.IsFalse(congruenceDict[1]);
            Assert.IsFalse(congruenceDict[2]);
            Assert.IsFalse(congruenceDict[3]);
            Assert.IsTrue(congruenceDict[4]);
            Assert.IsFalse(congruenceDict[5]);
            Assert.IsFalse(congruenceDict[6]);
            Assert.IsFalse(congruenceDict[7]);
        }

        [TestMethod]
        public void BoardCongruence5()
        {
            GameSetup gameSetup = new GameSetup()
            {
                WhitePlayer = new Player() { Name = "Player1" },
                BlackPlayer = new Player() { Name = "Player2" },
                BoardSize = 5
            };

            Game game = Game.GetNewGame(gameSetup);
            game.ApplyMove(NotationParser.Parse("b5"));
            game.ApplyMove(NotationParser.Parse("d4"));
            game.ApplyMove(NotationParser.Parse("d2"));
            game.ApplyMove(NotationParser.Parse("d1"));
            game.ApplyMove(NotationParser.Parse("b2"));
            game.ApplyMove(NotationParser.Parse("a2"));
            game.ApplyMove(NotationParser.Parse("b4"));
            game.ApplyMove(NotationParser.Parse("e4"));

            var board1 = game.CurrentBoard.Clone();
            Move move1 = NotationParser.Parse("c5");
            move1.Apply(board1);

            var board2 = game.CurrentBoard.Clone();
            Move move2 = NotationParser.Parse("a3");
            move2.Apply(board2);

            Assert.IsTrue(Board.IsCongruent(board1, board2));
        }

        [TestMethod]
        public void BoardCongruence5_Dict()
        {
            GameSetup gameSetup = new GameSetup()
            {
                WhitePlayer = new Player() { Name = "Player1" },
                BlackPlayer = new Player() { Name = "Player2" },
                BoardSize = 5
            };

            Game game = Game.GetNewGame(gameSetup);
            game.ApplyMove(NotationParser.Parse("b5"));
            game.ApplyMove(NotationParser.Parse("d4"));
            game.ApplyMove(NotationParser.Parse("d2"));
            game.ApplyMove(NotationParser.Parse("d1"));
            game.ApplyMove(NotationParser.Parse("b2"));
            game.ApplyMove(NotationParser.Parse("a2"));
            game.ApplyMove(NotationParser.Parse("b4"));
            game.ApplyMove(NotationParser.Parse("e4"));


            var congruenceDict = game.CurrentBoard.GetCongruenceDictionary();
            Assert.IsFalse(congruenceDict[1]);
            Assert.IsFalse(congruenceDict[2]);
            Assert.IsFalse(congruenceDict[3]);
            Assert.IsFalse(congruenceDict[4]);
            Assert.IsTrue(congruenceDict[5]);
            Assert.IsTrue(congruenceDict[6]);
            Assert.IsTrue(congruenceDict[7]);
        }

        [TestMethod]
        public void BoardCongruence6()
        {
            GameSetup gameSetup = new GameSetup()
            {
                WhitePlayer = new Player() { Name = "Player1" },
                BlackPlayer = new Player() { Name = "Player2" },
                BoardSize = 5
            };

            Game game = Game.GetNewGame(gameSetup);
            game.ApplyMove(NotationParser.Parse("b5"));
            game.ApplyMove(NotationParser.Parse("d2"));
            game.ApplyMove(NotationParser.Parse("b4"));
            game.ApplyMove(NotationParser.Parse("d1"));

            var board1 = game.CurrentBoard.Clone();
            Move move1 = NotationParser.Parse("b2");
            move1.Apply(board1);

            var board2 = game.CurrentBoard.Clone();
            Move move2 = NotationParser.Parse("d4");
            move2.Apply(board2);

            Assert.IsTrue(Board.IsCongruent(board1, board2));
        }

        [TestMethod]
        public void BoardCongruence6_Dict()
        {
            GameSetup gameSetup = new GameSetup()
            {
                WhitePlayer = new Player() { Name = "Player1" },
                BlackPlayer = new Player() { Name = "Player2" },
                BoardSize = 5
            };

            Game game = Game.GetNewGame(gameSetup);
            game.ApplyMove(NotationParser.Parse("b5"));
            game.ApplyMove(NotationParser.Parse("d2"));
            game.ApplyMove(NotationParser.Parse("b4"));
            game.ApplyMove(NotationParser.Parse("d1"));


            var congruenceDict = game.CurrentBoard.GetCongruenceDictionary();
            Assert.IsFalse(congruenceDict[1]);
            Assert.IsFalse(congruenceDict[2]);
            Assert.IsFalse(congruenceDict[3]);
            Assert.IsFalse(congruenceDict[4]);
            Assert.IsFalse(congruenceDict[5]);
            Assert.IsTrue(congruenceDict[6]);
            Assert.IsFalse(congruenceDict[7]);
        }

        [TestMethod]
        public void BoardCongruence6_StackNot()
        {
            GameSetup gameSetup = new GameSetup()
            {
                WhitePlayer = new Player() { Name = "Player1" },
                BlackPlayer = new Player() { Name = "Player2" },
                BoardSize = 5
            };

            Game game = Game.GetNewGame(gameSetup);
            game.ApplyMove(NotationParser.Parse("b4"));
            game.ApplyMove(NotationParser.Parse("b3"));
            game.ApplyMove(NotationParser.Parse("d3"));
            game.ApplyMove(NotationParser.Parse("c3"));
            game.ApplyMove(NotationParser.Parse("c4"));
            game.ApplyMove(NotationParser.Parse("1c3<1"));
            game.ApplyMove(NotationParser.Parse("1c4<1"));

            var congruenceDict = game.CurrentBoard.GetCongruenceDictionary();
            Assert.IsFalse(congruenceDict[6]);
        }

        [TestMethod]
        public void BoardCongruence6_StackNot2()
        {
            GameSetup gameSetup = new GameSetup()
            {
                WhitePlayer = new Player() { Name = "Player1" },
                BlackPlayer = new Player() { Name = "Player2" },
                BoardSize = 5
            };

            Game game = Game.GetNewGame(gameSetup);
            game.ApplyMove(NotationParser.Parse("b4"));
            game.ApplyMove(NotationParser.Parse("b3"));
            game.ApplyMove(NotationParser.Parse("d3"));
            game.ApplyMove(NotationParser.Parse("c2"));
            game.ApplyMove(NotationParser.Parse("d2"));
            game.ApplyMove(NotationParser.Parse("1c2>1"));

            var congruenceDict = game.CurrentBoard.GetCongruenceDictionary();
            Assert.IsFalse(congruenceDict[6]);
        }

        [TestMethod]
        public void BoardCongruenceMoveFilter()
        {
            GameSetup gameSetup = new GameSetup()
            {
                WhitePlayer = new Player() { Name = "Player1" },
                BlackPlayer = new Player() { Name = "Player2" },
                BoardSize = 5
            };

            Game game = Game.GetNewGame(gameSetup);
            IEnumerable<Move> moves = game.GetAllMoves();
            IEnumerable<Move> filteredMoves = game.GetAllMoves(true);

            Assert.AreEqual(25, moves.Count());
            Assert.AreEqual(6, filteredMoves.Count());
        }
    }
}
