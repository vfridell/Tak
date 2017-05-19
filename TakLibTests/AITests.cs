﻿using System;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TakLib;

namespace TakLibTests
{
    [TestClass]
    public class AITests
    {
        [TestMethod]
        public void JustWinAlready()
        {
            GameSetup gameSetup = new GameSetup()
            {
                WhitePlayer = new Player() { Name = "Player1" },
                BlackPlayer = new Player() { Name = "Player2" },
                BoardSize = 5
            };

            Game game = Game.GetNewGame(gameSetup);
            game.ApplyMove(NotationParser.Parse("c3"));
            game.ApplyMove(NotationParser.Parse("c4"));
            game.ApplyMove(NotationParser.Parse("a5"));
            game.ApplyMove(NotationParser.Parse("a1"));
            game.ApplyMove(NotationParser.Parse("e5"));
            game.ApplyMove(NotationParser.Parse("e1"));
            game.ApplyMove(NotationParser.Parse("a3"));
            game.ApplyMove(NotationParser.Parse("a4"));
            game.ApplyMove(NotationParser.Parse("b1"));
            game.ApplyMove(NotationParser.Parse("e3"));
            game.ApplyMove(NotationParser.Parse("e2"));
            game.ApplyMove(NotationParser.Parse("b5"));
            game.ApplyMove(NotationParser.Parse("d1"));
            game.ApplyMove(NotationParser.Parse("c1"));
            game.ApplyMove(NotationParser.Parse("d3"));
            game.ApplyMove(NotationParser.Parse("d5"));
            game.ApplyMove(NotationParser.Parse("c2"));
            game.ApplyMove(NotationParser.Parse("d2"));
            game.ApplyMove(NotationParser.Parse("1a5>1"));
            game.ApplyMove(NotationParser.Parse("b2"));
            game.ApplyMove(NotationParser.Parse("e4"));
            game.ApplyMove(NotationParser.Parse("1d5>1"));
            game.ApplyMove(NotationParser.Parse("a2"));
            game.ApplyMove(NotationParser.Parse("d4"));
            game.ApplyMove(NotationParser.Parse("1e4+1"));
            game.ApplyMove(NotationParser.Parse("c5"));
            game.ApplyMove(NotationParser.Parse("b3"));
            game.ApplyMove(NotationParser.Parse("1a4-1"));
            game.ApplyMove(NotationParser.Parse("d5"));
            game.ApplyMove(NotationParser.Parse("e4"));
            game.ApplyMove(NotationParser.Parse("2e5-11"));
            game.ApplyMove(NotationParser.Parse("2e4-11"));
            game.ApplyMove(NotationParser.Parse("e4"));
            game.ApplyMove(NotationParser.Parse("2e3+11"));
            game.ApplyMove(NotationParser.Parse("2e4-11"));
            game.ApplyMove(NotationParser.Parse("b4"));
            game.ApplyMove(NotationParser.Parse("1c2<1"));
            game.ApplyMove(NotationParser.Parse("c2"));
            game.ApplyMove(NotationParser.Parse("2e2<11"));
            game.ApplyMove(NotationParser.Parse("2d2<11"));
            game.ApplyMove(NotationParser.Parse("d2"));
            game.ApplyMove(NotationParser.Parse("2c2>11"));
            game.ApplyMove(NotationParser.Parse("1d2<1"));
            game.ApplyMove(NotationParser.Parse("3b2>12"));
            game.ApplyMove(NotationParser.Parse("b2"));
            game.ApplyMove(NotationParser.Parse("2c2<11"));
            game.ApplyMove(NotationParser.Parse("2b2>11"));
            game.ApplyMove(NotationParser.Parse("2e2<11"));
            game.ApplyMove(NotationParser.Parse("4d2<211"));
            game.ApplyMove(NotationParser.Parse("4c2>22"));
            game.ApplyMove(NotationParser.Parse("3a2+12"));
            var moves = game.GetAllMoves();
            Assert.IsTrue(moves.Any(m => m.ToString() == "1b4>1"));
            Move winningMove = moves.First(m => m.ToString() == "1b4>1");

            Board winningBoard = game.CurrentBoard.Clone();
            winningMove.Apply(winningBoard);
            
            BoardAnalyzer analyzer = new BoardAnalyzer(5);
            var data = analyzer.Analyze(winningBoard, BoardAnalysisWeights.bestWeights);

            JohnnyDeep jd = new JohnnyDeep(BoardAnalysisWeights.bestWeights, 1);
            jd.BeginNewGame(false, 5);
            Move move = jd.PickBestMove(game.CurrentBoard);
            Assert.AreEqual("1b4>1", move.ToString());
            //game.ApplyMove(NotationParser.Parse("1b4-1"));

        }
    }
}
