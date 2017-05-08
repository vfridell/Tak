using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TakLib;

namespace TakLibTests
{
    [TestClass]
    public class GameTests
    {
        [TestMethod]
        public void CreateGame()
        {
            GameSetup gameSetup = new GameSetup()
            {
                WhitePlayer = new Player() {Name = "Player1"},
                BlackPlayer = new Player() {Name = "Player2"},
                BoardSize = 5
            };

            Game game = Game.GetNewGame(gameSetup);

            Assert.AreEqual(5, game.CurrentBoard.Size);
            Assert.AreEqual(1, game.CurrentBoard.CapStonesInHand(PieceColor.White));
            Assert.AreEqual(0, game.CurrentBoard.CapStonesInHand(PieceColor.Black));
        }

        [TestMethod]
        public void EnumerateInitialMoves()
        {
            GameSetup gameSetup = new GameSetup()
            {
                WhitePlayer = new Player() {Name = "Player1"},
                BlackPlayer = new Player() {Name = "Player2"},
                BoardSize = 5
            };

            Game game = Game.GetNewGame(gameSetup);
            IEnumerable<Move> moves = game.GetAllMoves();
            Assert.AreEqual(25, moves.Count());
        }

        [TestMethod]
        public void EnumerateThirdRoundMoves()
        {
            GameSetup gameSetup = new GameSetup()
            {
                WhitePlayer = new Player() { Name = "Player1" },
                BlackPlayer = new Player() { Name = "Player2" },
                BoardSize = 5
            };

            Game game = Game.GetNewGame(gameSetup);

            game.ApplyMove(NotationParser.Parse("b2"));
            Assert.AreEqual(new Piece(PieceColor.Black, PieceType.Stone), game.CurrentBoard.GetPiece(1, 1));
            Assert.AreEqual(1, game.Turn);
            Assert.AreEqual(2, game.CurrentBoard.Round);
            game.ApplyMove(NotationParser.Parse("d4"));
            Assert.AreEqual(new Piece(PieceColor.White, PieceType.Stone), game.CurrentBoard.GetPiece(3, 3));
            Assert.AreEqual(2, game.Turn);
            Assert.AreEqual(3, game.CurrentBoard.Round);

            IEnumerable<Move> moves = game.GetAllMoves();
            // 23 board positions to place on times three piece types to place, plus 4 possible movement moves
            Assert.AreEqual(73, moves.Count());

        }

        [TestMethod]
        public void CheckPlayerFirstTurn()
        {
            GameSetup gameSetup = new GameSetup()
            {
                WhitePlayer = new Player() { Name = "Player1" },
                BlackPlayer = new Player() { Name = "Player2" },
                BoardSize = 5
            };

            Game game = Game.GetNewGame(gameSetup);
            Assert.AreEqual(true, game.WhiteToPlay);
            Assert.AreEqual(gameSetup.WhitePlayer, game.CurrentPlayer);
            Assert.AreEqual(PieceColor.Black, game.ColorToPlay);
        }

        [TestMethod]
        public void EndTurnStateCheck()
        {
            GameSetup gameSetup = new GameSetup()
            {
                WhitePlayer = new Player() { Name = "Player1" },
                BlackPlayer = new Player() { Name = "Player2" },
                BoardSize = 5
            };

            Game game = Game.GetNewGame(gameSetup);
            game.ApplyMove(NotationParser.Parse("a4"));
            Assert.AreEqual(new Piece(PieceColor.Black, PieceType.Stone), game.CurrentBoard.GetPiece(3, 0));
            Assert.AreEqual(1, game.Turn);
            Assert.AreEqual(2, game.CurrentBoard.Round);
            game.ApplyMove(NotationParser.Parse("e2"));
            Assert.AreEqual(new Piece(PieceColor.White, PieceType.Stone), game.CurrentBoard.GetPiece(1, 4));
            Assert.AreEqual(2, game.Turn);
            Assert.AreEqual(3, game.CurrentBoard.Round);
        }

    }
}
