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
        public void ApplySimpleMove()
        {
            GameSetup gameSetup = new GameSetup()
            {
                WhitePlayer = new Player() { Name = "Player1" },
                BlackPlayer = new Player() { Name = "Player2" },
                BoardSize = 5
            };

            Game game = Game.GetNewGame(gameSetup);


            //List<string> strings = CarryStackArrayMaker.GetAllDropLists('5');
            List<List<int>> strings = CarryStackArrayMaker.GetAllDropLists(5, 4);

            throw new NotImplementedException();
        }

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
            Assert.AreEqual(new Piece(PieceColor.Black, PieceType.Stone), game.CurrentBoard.GetPiece(3,0));
            Assert.AreEqual(1, game.Turn);
            Assert.AreEqual(2, game.CurrentBoard.Round);
            game.ApplyMove(NotationParser.Parse("e2"));
            Assert.AreEqual(new Piece(PieceColor.White, PieceType.Stone), game.CurrentBoard.GetPiece(1,4));
            game.ApplyMove(NotationParser.Parse("Se3"));
            Assert.AreEqual(new Piece(PieceColor.White, PieceType.Wall), game.CurrentBoard.GetPiece(2,4));
            game.ApplyMove(NotationParser.Parse("e1"));
            Assert.AreEqual(new Piece(PieceColor.Black, PieceType.Stone), game.CurrentBoard.GetPiece(0,4));
            game.ApplyMove(NotationParser.Parse("b3"));
            Assert.AreEqual(new Piece(PieceColor.White, PieceType.Stone), game.CurrentBoard.GetPiece(2,1));
            game.ApplyMove(NotationParser.Parse("Cd1"));
            Assert.AreEqual(new Piece(PieceColor.Black, PieceType.CapStone), game.CurrentBoard.GetPiece(0,3));
            game.ApplyMove(NotationParser.Parse("e4"));
            Assert.AreEqual(new Piece(PieceColor.White, PieceType.Stone), game.CurrentBoard.GetPiece(3,4));
            game.ApplyMove(NotationParser.Parse("d1>"));
            Assert.AreEqual(new Piece(PieceColor.Black, PieceType.CapStone), game.CurrentBoard.GetPiece(0,4));
            game.ApplyMove(NotationParser.Parse("d2"));
            Assert.AreEqual(new Piece(PieceColor.White, PieceType.Stone), game.CurrentBoard.GetPiece(1,3));

            IEnumerable<Move> moves = game.GetAllMoves();
            MoveStack ems = new MoveStack('2', '1', 'e', new [] {'1','1'}, '+');
            Assert.IsNotNull(moves.FirstOrDefault(m => m is MoveStack && m.ToString() == ems.ToString()));
        }
    }
}
