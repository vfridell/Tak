using System;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TakLib;
using System.Collections.Generic;

namespace TakLibTests
{
    [TestClass]
    public class SimpleJackTests
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

            Assert.AreEqual(PieceColor.Black, game.ColorToPlay);

            //there are two winning moves
            Assert.IsTrue(moves.Any(m => m.ToString() == "1b4>1"));
            Assert.IsTrue(moves.Any(m => m.ToString() == "1d4<1"));
            Move winningMove = moves.First(m => m.ToString() == "1b4>1");

            IBoardAnalyzer analyzer = new BoardAnalyzer(5, BoardAnalysisWeights.bestWeights);
            IAnalysisResult data = analyzer.Analyze(game.CurrentBoard);
            Assert.AreEqual(GameResult.Incomplete, data.gameResult);
            Board winningBoard = game.CurrentBoard.Clone();
            winningMove.Apply(winningBoard);

            data = analyzer.Analyze(winningBoard);
            Assert.AreEqual(GameResult.BlackRoad, data.gameResult);

            SimpleJack ai = new SimpleJack(3, game.CurrentBoard.Size);
            ai.BeginNewGame(false, 5);
            Move move = ai.PickBestMove(game.CurrentBoard);
            game.ApplyMove(move);
            Assert.AreEqual(GameResult.BlackRoad, game.GameResult);
        }


        [TestMethod]
        public void JustBlockAlready()
        {
            GameSetup gameSetup = new GameSetup()
            {
                WhitePlayer = new Player() { Name = "Player1" },
                BlackPlayer = new Player() { Name = "Player2" },
                BoardSize = 5
            };

            Game game = Game.GetNewGame(gameSetup);
            game.ApplyMove(NotationParser.Parse("e5"));
            game.ApplyMove(NotationParser.Parse("a5"));
            game.ApplyMove(NotationParser.Parse("b4"));
            game.ApplyMove(NotationParser.Parse("d4"));
            game.ApplyMove(NotationParser.Parse("a3"));
            game.ApplyMove(NotationParser.Parse("e3"));
            game.ApplyMove(NotationParser.Parse("c5"));
            game.ApplyMove(NotationParser.Parse("d3"));
            game.ApplyMove(NotationParser.Parse("c3"));
            game.ApplyMove(NotationParser.Parse("e4"));
            game.ApplyMove(NotationParser.Parse("b2"));
            game.ApplyMove(NotationParser.Parse("d2"));

            Assert.AreEqual(PieceColor.White, game.ColorToPlay);
            var moves = game.GetAllMoves();

            //there are four blocking moves
            // d1, Sd1, Cd1, 1c3>1
            List<Move> blockingMoves = new List<Move>();
            Assert.IsTrue(moves.Any(m => m.ToString() == "d1"));
            Assert.IsTrue(moves.Any(m => m.ToString() == "Sd1"));
            Assert.IsTrue(moves.Any(m => m.ToString() == "Cd1"));
            Assert.IsTrue(moves.Any(m => m.ToString() == "1c3>1"));
            blockingMoves.AddRange(moves.Where(m => m.ToString() == "d1" || m.ToString() == "Sd1" || m.ToString() == "Cd1" || m.ToString() == "1c3>1"));

            Move blockingMove = moves.First(m => m.ToString() == "Cd1");

            BoardAnalysisWeights weights = BoardAnalysisWeights.bestWeights;

            IBoardAnalyzer analyzer = new SimpleAnalyzer(5);
            IAnalysisResult data = analyzer.Analyze(game.CurrentBoard);
            Assert.AreEqual(GameResult.Incomplete, data.gameResult);
            Board blockedBoard = game.CurrentBoard.Clone();
            blockingMove.Apply(blockedBoard);

            data = analyzer.Analyze(blockedBoard);

            SimpleJack ai = new SimpleJack(2,game.CurrentBoard.Size);
            ai.BeginNewGame(true, 5);
            Move move = ai.PickBestMove(game.CurrentBoard);
            game.ApplyMove(move);
            Assert.IsTrue(blockingMoves.Any(m => m.ToString() == move.ToString()), $"Move selected was {move}");
        }

        [TestMethod]
        public void JustWinAlready2()
        {
            GameSetup gameSetup = new GameSetup()
            {
                WhitePlayer = new Player() { Name = "Player1" },
                BlackPlayer = new Player() { Name = "Player2" },
                BoardSize = 5
            };

            Game game = Game.GetNewGame(gameSetup);
            game.ApplyMove(NotationParser.Parse("a5"));
            game.ApplyMove(NotationParser.Parse("a1"));
            game.ApplyMove(NotationParser.Parse("b1"));
            game.ApplyMove(NotationParser.Parse("a4"));
            game.ApplyMove(NotationParser.Parse("b2"));
            game.ApplyMove(NotationParser.Parse("b3"));
            game.ApplyMove(NotationParser.Parse("1b2+1"));
            game.ApplyMove(NotationParser.Parse("a2"));
            game.ApplyMove(NotationParser.Parse("b2"));
            game.ApplyMove(NotationParser.Parse("b4"));
            game.ApplyMove(NotationParser.Parse("c2"));
            game.ApplyMove(NotationParser.Parse("1b4-1"));
            game.ApplyMove(NotationParser.Parse("1b2+1"));
            game.ApplyMove(NotationParser.Parse("b4"));
            game.ApplyMove(NotationParser.Parse("2b3+2"));
            game.ApplyMove(NotationParser.Parse("Cb2"));
            game.ApplyMove(NotationParser.Parse("c3"));
            game.ApplyMove(NotationParser.Parse("a3"));
            game.ApplyMove(NotationParser.Parse("c1"));
            game.ApplyMove(NotationParser.Parse("1b2+1"));
            game.ApplyMove(NotationParser.Parse("3b4<3"));
            game.ApplyMove(NotationParser.Parse("1a3+1"));
            game.ApplyMove(NotationParser.Parse("b2"));

            Assert.AreEqual(PieceColor.Black, game.ColorToPlay);
            var moves = game.GetAllMoves();

            //there are two "winning" moves
            // a3, 4a4-11
            // a3 doesn't win outright, but there is no white move after that can stop black
            List<Move> winningMoves = new List<Move>();
            winningMoves.AddRange(moves.Where(m => m.ToString() == "4a4-112"));

            Assert.IsTrue(moves.Any(m => m.ToString() == "4a4-112"));
            Move winningMove = moves.First(m => m.ToString() == "4a4-112");

            BoardAnalysisWeights weights = BoardAnalysisWeights.bestWeights;

            IBoardAnalyzer analyzer = new BoardAnalyzer(5, BoardAnalysisWeights.bestWeights);
            IAnalysisResult data = analyzer.Analyze(game.CurrentBoard);
            Assert.AreEqual(GameResult.Incomplete, data.gameResult);
            Board winningBoard = game.CurrentBoard.Clone();
            winningMove.Apply(winningBoard);

            data = analyzer.Analyze(winningBoard);
            Assert.AreEqual(GameResult.BlackRoad, data.gameResult);

            // works with depth of 2 ?!?
            SimpleJack ai = new SimpleJack(3,game.CurrentBoard.Size);
            ai.BeginNewGame(false, 5);
            Move move = ai.PickBestMove(game.CurrentBoard);
            game.ApplyMove(move);
            IAnalysisResult data2 = analyzer.Analyze(game.CurrentBoard);
            Assert.IsTrue(winningMoves.Any(m => m.ToString() == move.ToString()), $"Move selected was {move}");
        }

        [TestMethod]
        public void JustBlockAlready2()
        {
            GameSetup gameSetup = new GameSetup()
            {
                WhitePlayer = new Player() { Name = "Player1" },
                BlackPlayer = new Player() { Name = "Player2" },
                BoardSize = 5
            };

            Game game = Game.GetNewGame(gameSetup);
            game.ApplyMove(NotationParser.Parse("a1"));
            game.ApplyMove(NotationParser.Parse("e5"));
            game.ApplyMove(NotationParser.Parse("d5"));
            game.ApplyMove(NotationParser.Parse("a2"));
            game.ApplyMove(NotationParser.Parse("d4"));
            game.ApplyMove(NotationParser.Parse("b2"));
            game.ApplyMove(NotationParser.Parse("c4"));
            game.ApplyMove(NotationParser.Parse("c2"));
            game.ApplyMove(NotationParser.Parse("b4"));

            Assert.AreEqual(PieceColor.Black, game.ColorToPlay);
            var moves = game.GetAllMoves();

            //there are three blocking moves
            // a4, Sa4, Ca4
            List<Move> blockingMoves = new List<Move>();
            Assert.IsTrue(moves.Any(m => m.ToString() == "a4"));
            Assert.IsTrue(moves.Any(m => m.ToString() == "Sa4"));
            Assert.IsTrue(moves.Any(m => m.ToString() == "Ca4"));
            blockingMoves.AddRange(moves.Where(m => m.ToString() == "a4" || m.ToString() == "Sa4" || m.ToString() == "Ca4"));

            Move blockingMove = moves.First(m => m.ToString() == "Ca4");

            BoardAnalysisWeights weights = BoardAnalysisWeights.bestWeights;

            IBoardAnalyzer analyzer = new BoardAnalyzer(5, BoardAnalysisWeights.bestWeights);
            IAnalysisResult data = analyzer.Analyze(game.CurrentBoard);
            Assert.AreEqual(GameResult.Incomplete, data.gameResult);
            Board blockedBoard = game.CurrentBoard.Clone();
            blockingMove.Apply(blockedBoard);

            data = analyzer.Analyze(blockedBoard);

            SimpleJack ai = new SimpleJack(3,game.CurrentBoard.Size);
            ai.BeginNewGame(false, 5);
            Move move = ai.PickBestMove(game.CurrentBoard);
            Assert.IsTrue(blockingMoves.Any(m => m.ToString() == move.ToString()), $"Move selected was {move}");
        }

        [TestMethod]
        public void UnwinnableSituation()
        {
            GameSetup gameSetup = new GameSetup()
            {
                WhitePlayer = new Player() { Name = "Player1" },
                BlackPlayer = new Player() { Name = "Player2" },
                BoardSize = 5
            };

            Game game = Game.GetNewGame(gameSetup);
            game.ApplyMove(NotationParser.Parse("a5"));
            game.ApplyMove(NotationParser.Parse("c4"));
            game.ApplyMove(NotationParser.Parse("b3"));
            game.ApplyMove(NotationParser.Parse("e3"));
            game.ApplyMove(NotationParser.Parse("b4"));
            game.ApplyMove(NotationParser.Parse("c2"));
            game.ApplyMove(NotationParser.Parse("d4"));
            game.ApplyMove(NotationParser.Parse("c3"));
            game.ApplyMove(NotationParser.Parse("a3"));
            game.ApplyMove(NotationParser.Parse("1c3<1"));
            game.ApplyMove(NotationParser.Parse("1a3>1"));
            game.ApplyMove(NotationParser.Parse("d2"));
            game.ApplyMove(NotationParser.Parse("Ce4"));

            Assert.AreEqual(PieceColor.Black, game.ColorToPlay);
            var moves = game.GetAllMoves();

            //there are no valid blocking moves
            List<Move> blockingMoves = new List<Move>();
            Assert.IsTrue(moves.Any(m => m.ToString() == "a3"));
            Assert.IsTrue(moves.Any(m => m.ToString() == "Sa3"));
            Assert.IsTrue(moves.Any(m => m.ToString() == "Ca3"));
            blockingMoves.AddRange(moves.Where(m => m.ToString() == "a3" || m.ToString() == "Sa3" || m.ToString() == "Ca3"));

            Move blockingMove = moves.First(m => m.ToString() == "Ca3");

            BoardAnalysisWeights weights = BoardAnalysisWeights.bestWeights;

            IBoardAnalyzer analyzer = new BoardAnalyzer(5, BoardAnalysisWeights.bestWeights);
            IAnalysisResult data = analyzer.Analyze(game.CurrentBoard);
            Assert.AreEqual(GameResult.Incomplete, data.gameResult);
            Board blockedBoard = game.CurrentBoard.Clone();
            blockingMove.Apply(blockedBoard);

            data = analyzer.Analyze(blockedBoard);

            SimpleJack ai = new SimpleJack(4, game.CurrentBoard.Size);
            ai.BeginNewGame(false, 5);
            Move move = ai.PickBestMove(game.CurrentBoard);
            Assert.IsTrue(blockingMoves.Any(m => m.ToString() == move.ToString()), $"Move selected was {move}");
        }

        [TestMethod]
        public void UnwinnableSituation2()
        {
            GameSetup gameSetup = new GameSetup()
            {
                WhitePlayer = new Player() { Name = "Player1" },
                BlackPlayer = new Player() { Name = "Player2" },
                BoardSize = 5
            };

            Game game = Game.GetNewGame(gameSetup);
            game.ApplyMove(NotationParser.Parse("a5"));
            game.ApplyMove(NotationParser.Parse("e5"));
            game.ApplyMove(NotationParser.Parse("d4"));
            game.ApplyMove(NotationParser.Parse("a2"));
            game.ApplyMove(NotationParser.Parse("e4"));
            game.ApplyMove(NotationParser.Parse("c1"));
            game.ApplyMove(NotationParser.Parse("d2"));
            game.ApplyMove(NotationParser.Parse("d5"));
            game.ApplyMove(NotationParser.Parse("e3"));
            game.ApplyMove(NotationParser.Parse("b1"));
            game.ApplyMove(NotationParser.Parse("d3"));
            game.ApplyMove(NotationParser.Parse("d1"));
            game.ApplyMove(NotationParser.Parse("e2"));
            game.ApplyMove(NotationParser.Parse("e1"));
            game.ApplyMove(NotationParser.Parse("1e2-1"));


            Assert.AreEqual(PieceColor.Black, game.ColorToPlay);
            var moves = game.GetAllMoves();

            //there are no blocking moves here.  White wins in 4 no matter what
            // most delay comes from 1d5>1
            List<Move> blockingMoves = new List<Move>();
            Assert.IsTrue(moves.Any(m => m.ToString() == "1d5>1"));
            blockingMoves.AddRange(moves.Where(m => m.ToString() == "1d5>1"));

            Move blockingMove = moves.First(m => m.ToString() == "1d5>1");

            BoardAnalysisWeights weights = BoardAnalysisWeights.bestWeights;

            IBoardAnalyzer analyzer = new BoardAnalyzer(5, BoardAnalysisWeights.bestWeights);
            IAnalysisResult data = analyzer.Analyze(game.CurrentBoard);
            Assert.AreEqual(GameResult.Incomplete, data.gameResult);
            Board blockedBoard = game.CurrentBoard.Clone();
            blockingMove.Apply(blockedBoard);

            data = analyzer.Analyze(blockedBoard);

            SimpleJack ai = new SimpleJack(4, game.CurrentBoard.Size);
            ai.BeginNewGame(false, 5);
            Move move = ai.PickBestMove(game.CurrentBoard);
            Assert.IsTrue(blockingMoves.Any(m => m.ToString() == move.ToString()), $"Move selected was {move}");
        }

        [TestMethod]
        public void UnwinnableSituation3()
        {
            GameSetup gameSetup = new GameSetup()
            {
                WhitePlayer = new Player() { Name = "Player1" },
                BlackPlayer = new Player() { Name = "Player2" },
                BoardSize = 5
            };

            Game game = Game.GetNewGame(gameSetup);
            game.ApplyMove(NotationParser.Parse("e1"));
            game.ApplyMove(NotationParser.Parse("b2"));
            game.ApplyMove(NotationParser.Parse("c3"));
            game.ApplyMove(NotationParser.Parse("a5"));
            game.ApplyMove(NotationParser.Parse("d3"));
            game.ApplyMove(NotationParser.Parse("c1"));
            game.ApplyMove(NotationParser.Parse("d4"));
            game.ApplyMove(NotationParser.Parse("b1"));
            game.ApplyMove(NotationParser.Parse("c2"));
            game.ApplyMove(NotationParser.Parse("1c1+1"));
            game.ApplyMove(NotationParser.Parse("1b2>1"));
            game.ApplyMove(NotationParser.Parse("c1"));
            game.ApplyMove(NotationParser.Parse("2c2-2"));
            game.ApplyMove(NotationParser.Parse("1b1>1"));
            game.ApplyMove(NotationParser.Parse("1c2-1"));
            game.ApplyMove(NotationParser.Parse("a3"));
            game.ApplyMove(NotationParser.Parse("d5"));
            game.ApplyMove(NotationParser.Parse("Cc2"));
            game.ApplyMove(NotationParser.Parse("d2"));
            game.ApplyMove(NotationParser.Parse("Sd1"));
            game.ApplyMove(NotationParser.Parse("b4"));
            game.ApplyMove(NotationParser.Parse("c5"));
            game.ApplyMove(NotationParser.Parse("b3"));
            game.ApplyMove(NotationParser.Parse("e4"));
            game.ApplyMove(NotationParser.Parse("b2"));
            game.ApplyMove(NotationParser.Parse("1c5>1"));
            game.ApplyMove(NotationParser.Parse("b1"));
            game.ApplyMove(NotationParser.Parse("1c2<1"));
            game.ApplyMove(NotationParser.Parse("e3"));
            game.ApplyMove(NotationParser.Parse("2b2+2"));
            game.ApplyMove(NotationParser.Parse("c4"));
            game.ApplyMove(NotationParser.Parse("3b3>111"));
            game.ApplyMove(NotationParser.Parse("2c3<11"));
            game.ApplyMove(NotationParser.Parse("2e3<2"));
            game.ApplyMove(NotationParser.Parse("b2"));
            game.ApplyMove(NotationParser.Parse("4d3<13"));
            game.ApplyMove(NotationParser.Parse("c2"));

            Assert.AreEqual(PieceColor.Black, game.ColorToPlay);
            Assert.AreEqual(GameResult.Incomplete, game.GameResult);
            var moves = game.GetAllMoves();

            //there are no blocking moves here.  White wins in 4 no matter what
            // most delay comes from 4b3>4
            List<Move> blockingMoves = new List<Move>();
            Assert.IsTrue(moves.Any(m => m.ToString() == "4b3>4"));
            blockingMoves.AddRange(moves.Where(m => m.ToString() == "4b3>4"));

            SimpleJack ai = new SimpleJack(4, game.CurrentBoard.Size);
            ai.BeginNewGame(false, 5);
            Move move = ai.PickBestMove(game.CurrentBoard);
            Assert.IsTrue(blockingMoves.Any(m => m.ToString() == move.ToString()), $"Move selected was {move}");
        }

        [TestMethod]
        public void JustWinAlreadyCapCrush()
        {
            GameSetup gameSetup = new GameSetup()
            {
                WhitePlayer = new Player() { Name = "Player1" },
                BlackPlayer = new Player() { Name = "Player2" },
                BoardSize = 5
            };

            Game game = Game.GetNewGame(gameSetup);
            game.ApplyMove(NotationParser.Parse("a1"));
            game.ApplyMove(NotationParser.Parse("e5"));
            game.ApplyMove(NotationParser.Parse("d5"));
            game.ApplyMove(NotationParser.Parse("b5"));
            game.ApplyMove(NotationParser.Parse("c4"));
            game.ApplyMove(NotationParser.Parse("b3"));
            game.ApplyMove(NotationParser.Parse("d4"));
            game.ApplyMove(NotationParser.Parse("c3"));
            game.ApplyMove(NotationParser.Parse("1c4-1"));
            game.ApplyMove(NotationParser.Parse("1b3>1"));
            game.ApplyMove(NotationParser.Parse("d2"));
            game.ApplyMove(NotationParser.Parse("b4"));
            game.ApplyMove(NotationParser.Parse("e4"));
            game.ApplyMove(NotationParser.Parse("a2"));
            game.ApplyMove(NotationParser.Parse("Sb3"));
            game.ApplyMove(NotationParser.Parse("3c3+12"));
            game.ApplyMove(NotationParser.Parse("d3"));
            game.ApplyMove(NotationParser.Parse("d1"));
            game.ApplyMove(NotationParser.Parse("1b3+1"));
            game.ApplyMove(NotationParser.Parse("1d1+1"));
            game.ApplyMove(NotationParser.Parse("2b4>2"));
            game.ApplyMove(NotationParser.Parse("e3"));
            game.ApplyMove(NotationParser.Parse("3c4+3"));
            game.ApplyMove(NotationParser.Parse("1e3+1"));
            game.ApplyMove(NotationParser.Parse("4c5<4"));
            game.ApplyMove(NotationParser.Parse("2e4+2"));
            game.ApplyMove(NotationParser.Parse("1d5>1"));
            game.ApplyMove(NotationParser.Parse("Ce3"));
            game.ApplyMove(NotationParser.Parse("Ca4"));
            game.ApplyMove(NotationParser.Parse("e2"));
            game.ApplyMove(NotationParser.Parse("d5"));
            game.ApplyMove(NotationParser.Parse("c2"));
            game.ApplyMove(NotationParser.Parse("b2"));
            game.ApplyMove(NotationParser.Parse("1e2<1"));
            game.ApplyMove(NotationParser.Parse("c4"));
            game.ApplyMove(NotationParser.Parse("b4"));
            game.ApplyMove(NotationParser.Parse("5b5-5"));
            game.ApplyMove(NotationParser.Parse("3d2+21"));
            game.ApplyMove(NotationParser.Parse("b5"));
            game.ApplyMove(NotationParser.Parse("Sa5"));


            Assert.AreEqual(PieceColor.White, game.ColorToPlay);
            var moves = game.GetAllMoves();

            //winning move
            // a4+
            List<Move> winningMoves = new List<Move>();
            winningMoves.AddRange(moves.Where(m => m.ToString() == "1a4+1"));

            Assert.IsTrue(moves.Any(m => m.ToString() == "1a4+1"));
            Move winningMove = moves.First(m => m.ToString() == "1a4+1");

            BoardAnalysisWeights weights = BoardAnalysisWeights.bestWeights;

            IBoardAnalyzer analyzer = new BoardAnalyzer(5, BoardAnalysisWeights.bestWeights);
            IAnalysisResult data = analyzer.Analyze(game.CurrentBoard);
            Assert.AreEqual(GameResult.Incomplete, data.gameResult);
            Board winningBoard = game.CurrentBoard.Clone();
            winningMove.Apply(winningBoard);

            data = analyzer.Analyze(winningBoard);
            Assert.AreEqual(GameResult.WhiteRoad, data.gameResult);

            SimpleJack ai = new SimpleJack(3,game.CurrentBoard.Size);
            ai.BeginNewGame(true, 5);
            Move move = ai.PickBestMove(game.CurrentBoard);
            game.ApplyMove(move);
            IAnalysisResult data2 = analyzer.Analyze(game.CurrentBoard);
            Assert.IsTrue(winningMoves.Any(m => m.ToString() == move.ToString()), $"Move selected was {move}");
        }
    }
}
