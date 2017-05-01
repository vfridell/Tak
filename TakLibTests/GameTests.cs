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
            // TODO how many initial moves are there in a size 5 Tak?
            Assert.AreEqual(15, moves.Count());
        }

        [TestMethod]
        public void EndTurnStateCheck()
        {
            throw new NotImplementedException();
        }

    }
}
