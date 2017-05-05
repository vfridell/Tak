﻿using System;
using System.Collections.Generic;
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
    }
}
