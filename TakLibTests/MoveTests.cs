using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TakLib;

namespace TakLibTests
{
    [TestClass]
    public class MoveTests
    {
        [TestMethod]
        public void CapstoneStackFlatten()
        {
            GameSetup gameSetup = new GameSetup()
            {
                WhitePlayer = new Player() { Name = "Player1" },
                BlackPlayer = new Player() { Name = "Player2" },
                BoardSize = 5
            };

            Game game = Game.GetNewGame(gameSetup);
            game.ApplyMove(NotationParser.Parse("a4"));
            Assert.AreEqual(new Piece(PieceColor.Black, PieceType.Stone), game.CurrentBoard.GetPiece(new Coordinate(3,0)));
            Assert.AreEqual(1, game.Turn);
            Assert.AreEqual(2, game.CurrentBoard.Round);
            game.ApplyMove(NotationParser.Parse("e2"));
            Assert.AreEqual(new Piece(PieceColor.White, PieceType.Stone), game.CurrentBoard.GetPiece(new Coordinate(1,4)));
            game.ApplyMove(NotationParser.Parse("Se3"));
            Assert.AreEqual(new Piece(PieceColor.White, PieceType.Wall), game.CurrentBoard.GetPiece(new Coordinate(2,4)));
            game.ApplyMove(NotationParser.Parse("e1"));
            Assert.AreEqual(new Piece(PieceColor.Black, PieceType.Stone), game.CurrentBoard.GetPiece(new Coordinate(0,4)));
            game.ApplyMove(NotationParser.Parse("b3"));
            Assert.AreEqual(new Piece(PieceColor.White, PieceType.Stone), game.CurrentBoard.GetPiece(new Coordinate(2,1)));
            game.ApplyMove(NotationParser.Parse("Cd1"));
            Assert.AreEqual(new Piece(PieceColor.Black, PieceType.CapStone), game.CurrentBoard.GetPiece(new Coordinate(0,3)));
            game.ApplyMove(NotationParser.Parse("e4"));
            Assert.AreEqual(new Piece(PieceColor.White, PieceType.Stone), game.CurrentBoard.GetPiece(new Coordinate(3,4)));
            game.ApplyMove(NotationParser.Parse("d1>"));
            Assert.AreEqual(new Piece(PieceColor.Black, PieceType.CapStone), game.CurrentBoard.GetPiece(new Coordinate(0,4)));
            game.ApplyMove(NotationParser.Parse("d2"));
            Assert.AreEqual(new Piece(PieceColor.White, PieceType.Stone), game.CurrentBoard.GetPiece(new Coordinate(1,3)));

            IEnumerable<Move> moves = game.GetAllMoves();
            MoveStack ems = new MoveStack('2', '1', 'e', new [] {'1','1'}, '+');
            Assert.IsNotNull(moves.FirstOrDefault(m => m is MoveStack && m.ToString() == ems.ToString()));
        }

        [TestMethod]
        public void FullGameTestBlackRoad()
        {
            GameSetup gameSetup = new GameSetup()
            {
                WhitePlayer = new Player() {Name = "Player1"},
                BlackPlayer = new Player() {Name = "Player2"},
                BoardSize = 5
            };

            Game game = Game.GetNewGame(gameSetup);
            game.ApplyMove(NotationParser.Parse("d2"));
            game.ApplyMove(NotationParser.Parse("c3"));
            game.ApplyMove(NotationParser.Parse("c4"));
            game.ApplyMove(NotationParser.Parse("d3"));
            game.ApplyMove(NotationParser.Parse("e2"));
            game.ApplyMove(NotationParser.Parse("b1"));
            game.ApplyMove(NotationParser.Parse("b3"));
            game.ApplyMove(NotationParser.Parse("b1+"));
            game.ApplyMove(NotationParser.Parse("Sb1"));
            game.ApplyMove(NotationParser.Parse("Cc1"));
            game.ApplyMove(NotationParser.Parse("e4"));
            game.ApplyMove(NotationParser.Parse("c1<"));
            game.ApplyMove(NotationParser.Parse("a1"));
            game.ApplyMove(NotationParser.Parse("2b1>"));
            game.ApplyMove(NotationParser.Parse("b1"));
            game.ApplyMove(NotationParser.Parse("c2"));
            game.ApplyMove(NotationParser.Parse("a3"));
            game.ApplyMove(NotationParser.Parse("d4"));
            game.ApplyMove(NotationParser.Parse("c5"));
            game.ApplyMove(NotationParser.Parse("d5"));
            Assert.AreEqual(GameResult.BlackRoad, game.GameResult);
        }

        [TestMethod]
        public void FullGameTestBlackFlat()
        {
            GameSetup gameSetup = new GameSetup()
            {
                WhitePlayer = new Player() { Name = "Player1" },
                BlackPlayer = new Player() { Name = "Player2" },
                BoardSize = 5
            };

            Game game = Game.GetNewGame(gameSetup);
            game.ApplyMove(NotationParser.Parse("a5"));
            game.ApplyMove(NotationParser.Parse("a4"));
            game.ApplyMove(NotationParser.Parse("a3"));
            game.ApplyMove(NotationParser.Parse("a2"));
            game.ApplyMove(NotationParser.Parse("a1"));
            game.ApplyMove(NotationParser.Parse("b1"));
            game.ApplyMove(NotationParser.Parse("b2"));
            game.ApplyMove(NotationParser.Parse("b3"));
            game.ApplyMove(NotationParser.Parse("b4"));
            game.ApplyMove(NotationParser.Parse("b5"));
            game.ApplyMove(NotationParser.Parse("c5"));
            game.ApplyMove(NotationParser.Parse("c4"));
            game.ApplyMove(NotationParser.Parse("c3"));
            game.ApplyMove(NotationParser.Parse("c2"));
            game.ApplyMove(NotationParser.Parse("c1"));
            game.ApplyMove(NotationParser.Parse("d1"));
            game.ApplyMove(NotationParser.Parse("d2"));
            game.ApplyMove(NotationParser.Parse("d3"));
            game.ApplyMove(NotationParser.Parse("d4"));
            game.ApplyMove(NotationParser.Parse("d5"));
            game.ApplyMove(NotationParser.Parse("e5"));
            game.ApplyMove(NotationParser.Parse("e4"));
            game.ApplyMove(NotationParser.Parse("e3"));
            game.ApplyMove(NotationParser.Parse("e2"));
            game.ApplyMove(NotationParser.Parse("a4+"));
            game.ApplyMove(NotationParser.Parse("b5-"));
            game.ApplyMove(NotationParser.Parse("c5-"));
            game.ApplyMove(NotationParser.Parse("d5-"));
            game.ApplyMove(NotationParser.Parse("e5-"));
            game.ApplyMove(NotationParser.Parse("a2+"));
            game.ApplyMove(NotationParser.Parse("a1>"));
            game.ApplyMove(NotationParser.Parse("d1<"));
            game.ApplyMove(NotationParser.Parse("b2>"));
            game.ApplyMove(NotationParser.Parse("e2<"));
            game.ApplyMove(NotationParser.Parse("e3<"));
            game.ApplyMove(NotationParser.Parse("b3>"));
            game.ApplyMove(NotationParser.Parse("a2"));
            game.ApplyMove(NotationParser.Parse("a1"));
            game.ApplyMove(NotationParser.Parse("b2"));
            game.ApplyMove(NotationParser.Parse("d1"));
            game.ApplyMove(NotationParser.Parse("e2"));
            game.ApplyMove(NotationParser.Parse("e1"));
            game.ApplyMove(NotationParser.Parse("e3"));
            game.ApplyMove(NotationParser.Parse("b3"));
            game.ApplyMove(NotationParser.Parse("b5"));
            game.ApplyMove(NotationParser.Parse("c5"));
            game.ApplyMove(NotationParser.Parse("d5"));
            game.ApplyMove(NotationParser.Parse("e5"));
            game.ApplyMove(NotationParser.Parse("2a5>"));
            game.ApplyMove(NotationParser.Parse("2c1<"));
            game.ApplyMove(NotationParser.Parse("2c2<"));
            game.ApplyMove(NotationParser.Parse("2a3>"));
            game.ApplyMove(NotationParser.Parse("3b5>"));
            game.ApplyMove(NotationParser.Parse("2d2+"));
            game.ApplyMove(NotationParser.Parse("2e4<"));
            game.ApplyMove(NotationParser.Parse("4b1+"));
            game.ApplyMove(NotationParser.Parse("d5>"));
            game.ApplyMove(NotationParser.Parse("e1+"));
            game.ApplyMove(NotationParser.Parse("e3-"));
            game.ApplyMove(NotationParser.Parse("c1"));
            game.ApplyMove(NotationParser.Parse("c2"));
            game.ApplyMove(NotationParser.Parse("d2"));
            game.ApplyMove(NotationParser.Parse("b5"));
            game.ApplyMove(NotationParser.Parse("d5"));
            game.ApplyMove(NotationParser.Parse("b5-"));
            game.ApplyMove(NotationParser.Parse("Ce4"));

            Assert.AreEqual(GameResult.BlackFlat, game.GameResult);
        }

        [TestMethod]
        public void FullGameTestDraw()
        {
            GameSetup gameSetup = new GameSetup()
            {
                WhitePlayer = new Player() { Name = "Player1" },
                BlackPlayer = new Player() { Name = "Player2" },
                BoardSize = 5
            };

            Game game = Game.GetNewGame(gameSetup);
            game.ApplyMove(NotationParser.Parse("a5"));
            game.ApplyMove(NotationParser.Parse("a4"));
            game.ApplyMove(NotationParser.Parse("a3"));
            game.ApplyMove(NotationParser.Parse("a2"));
            game.ApplyMove(NotationParser.Parse("a1"));
            game.ApplyMove(NotationParser.Parse("b1"));
            game.ApplyMove(NotationParser.Parse("b2"));
            game.ApplyMove(NotationParser.Parse("b3"));
            game.ApplyMove(NotationParser.Parse("b4"));
            game.ApplyMove(NotationParser.Parse("b5"));
            game.ApplyMove(NotationParser.Parse("c5"));
            game.ApplyMove(NotationParser.Parse("c4"));
            game.ApplyMove(NotationParser.Parse("c3"));
            game.ApplyMove(NotationParser.Parse("c2"));
            game.ApplyMove(NotationParser.Parse("c1"));
            game.ApplyMove(NotationParser.Parse("d1"));
            game.ApplyMove(NotationParser.Parse("d2"));
            game.ApplyMove(NotationParser.Parse("d3"));
            game.ApplyMove(NotationParser.Parse("d4"));
            game.ApplyMove(NotationParser.Parse("d5"));
            game.ApplyMove(NotationParser.Parse("e5"));
            game.ApplyMove(NotationParser.Parse("e4"));
            game.ApplyMove(NotationParser.Parse("e3"));
            game.ApplyMove(NotationParser.Parse("e2"));
            game.ApplyMove(NotationParser.Parse("a4+"));
            game.ApplyMove(NotationParser.Parse("b5-"));
            game.ApplyMove(NotationParser.Parse("c5-"));
            game.ApplyMove(NotationParser.Parse("d5-"));
            game.ApplyMove(NotationParser.Parse("e5-"));
            game.ApplyMove(NotationParser.Parse("a2+"));
            game.ApplyMove(NotationParser.Parse("a1>"));
            game.ApplyMove(NotationParser.Parse("d1<"));
            game.ApplyMove(NotationParser.Parse("b2>"));
            game.ApplyMove(NotationParser.Parse("e2<"));
            game.ApplyMove(NotationParser.Parse("e3<"));
            game.ApplyMove(NotationParser.Parse("b3>"));
            game.ApplyMove(NotationParser.Parse("a2"));
            game.ApplyMove(NotationParser.Parse("a1"));
            game.ApplyMove(NotationParser.Parse("b2"));
            game.ApplyMove(NotationParser.Parse("d1"));
            game.ApplyMove(NotationParser.Parse("e2"));
            game.ApplyMove(NotationParser.Parse("e1"));
            game.ApplyMove(NotationParser.Parse("e3"));
            game.ApplyMove(NotationParser.Parse("b3"));
            game.ApplyMove(NotationParser.Parse("b5"));
            game.ApplyMove(NotationParser.Parse("c5"));
            game.ApplyMove(NotationParser.Parse("d5"));
            game.ApplyMove(NotationParser.Parse("e5"));
            game.ApplyMove(NotationParser.Parse("2a5>"));
            game.ApplyMove(NotationParser.Parse("2c1<"));
            game.ApplyMove(NotationParser.Parse("2c2<"));
            game.ApplyMove(NotationParser.Parse("2a3>"));
            game.ApplyMove(NotationParser.Parse("3b5>"));
            game.ApplyMove(NotationParser.Parse("2d2+"));
            game.ApplyMove(NotationParser.Parse("2e4<"));
            game.ApplyMove(NotationParser.Parse("4b1+"));
            game.ApplyMove(NotationParser.Parse("d5>"));
            game.ApplyMove(NotationParser.Parse("e1+"));
            game.ApplyMove(NotationParser.Parse("e3-"));
            game.ApplyMove(NotationParser.Parse("c1"));
            game.ApplyMove(NotationParser.Parse("c2"));
            game.ApplyMove(NotationParser.Parse("d2"));
            game.ApplyMove(NotationParser.Parse("b5"));
            game.ApplyMove(NotationParser.Parse("d5"));
            game.ApplyMove(NotationParser.Parse("b5-"));
            game.ApplyMove(NotationParser.Parse("a1>"));
            game.ApplyMove(NotationParser.Parse("a2>"));
            game.ApplyMove(NotationParser.Parse("b1<"));
            game.ApplyMove(NotationParser.Parse("a5"));
            game.ApplyMove(NotationParser.Parse("3b3-"));
            game.ApplyMove(NotationParser.Parse("a5>"));
            game.ApplyMove(NotationParser.Parse("Ce3"));

            Assert.AreEqual(GameResult.Draw, game.GameResult);
        }

        [TestMethod]
        public void FullGameTestWhiteRoad()
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
            game.ApplyMove(NotationParser.Parse("e3"));

            Assert.AreEqual(GameResult.WhiteRoad, game.GameResult);
        }

        [TestMethod]
        public void FullGameTestWhiteFlat()
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
            game.ApplyMove(NotationParser.Parse("d2"));
            game.ApplyMove(NotationParser.Parse("b4"));
            game.ApplyMove(NotationParser.Parse("c3"));
            game.ApplyMove(NotationParser.Parse("d4"));
            game.ApplyMove(NotationParser.Parse("b2"));
            game.ApplyMove(NotationParser.Parse("c2"));
            game.ApplyMove(NotationParser.Parse("c1"));
            game.ApplyMove(NotationParser.Parse("d1"));
            game.ApplyMove(NotationParser.Parse("a1"));
            game.ApplyMove(NotationParser.Parse("b1"));
            game.ApplyMove(NotationParser.Parse("a2"));
            game.ApplyMove(NotationParser.Parse("a3"));
            game.ApplyMove(NotationParser.Parse("c5"));
            game.ApplyMove(NotationParser.Parse("b3"));
            game.ApplyMove(NotationParser.Parse("c4"));
            game.ApplyMove(NotationParser.Parse("e4"));
            game.ApplyMove(NotationParser.Parse("e3"));
            game.ApplyMove(NotationParser.Parse("e2"));
            game.ApplyMove(NotationParser.Parse("a4"));
            game.ApplyMove(NotationParser.Parse("d3"));
            game.ApplyMove(NotationParser.Parse("d5"));
            game.ApplyMove(NotationParser.Parse("e5"));
            game.ApplyMove(NotationParser.Parse("b5"));

            Assert.AreEqual(GameResult.WhiteFlat, game.GameResult);
        }
    }
}
