using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TakLib;

namespace TakLibTests
{
    [TestClass]
    public class NotationParserTests
    {
        [TestMethod]
        public void ParsePlacement()
        {
            throw new NotImplementedException();
        }

        [TestMethod]
        public void CannotPlaceOnStack()
        {
            throw new NotImplementedException();
        }

        [TestMethod]
        public void ParseBasicMovement()
        {
            throw new NotImplementedException();
        }

        [TestMethod]
        public void CannotMoveFullStackTwice()
        {
            throw new NotImplementedException();
        }

        [TestMethod]
        public void ParseStackMovement()
        {
            throw new NotImplementedException();
        }

        [TestMethod]
        public void ParseRow()
        {
            Assert.AreEqual(0, NotationParser.ToRow('1'));
            Assert.AreEqual(1, NotationParser.ToRow('2'));
            Assert.AreEqual(2, NotationParser.ToRow('3'));
            Assert.AreEqual(3, NotationParser.ToRow('4'));
            Assert.AreEqual(4, NotationParser.ToRow('5'));
            Assert.AreEqual(5, NotationParser.ToRow('6'));
            Assert.AreEqual(6, NotationParser.ToRow('7'));
            Assert.AreEqual(7, NotationParser.ToRow('8'));
        }

        [TestMethod]
        public void ParseColumn()
        {
            Assert.AreEqual(0, NotationParser.ToColumn('a'));
            Assert.AreEqual(1, NotationParser.ToColumn('b'));
            Assert.AreEqual(2, NotationParser.ToColumn('c'));
            Assert.AreEqual(3, NotationParser.ToColumn('d'));
            Assert.AreEqual(4, NotationParser.ToColumn('e'));
            Assert.AreEqual(5, NotationParser.ToColumn('f'));
            Assert.AreEqual(6, NotationParser.ToColumn('g'));
            Assert.AreEqual(7, NotationParser.ToColumn('h'));
            Assert.AreEqual(0, NotationParser.ToColumn('A'));
            Assert.AreEqual(1, NotationParser.ToColumn('B'));
            Assert.AreEqual(2, NotationParser.ToColumn('C'));
            Assert.AreEqual(3, NotationParser.ToColumn('D'));
            Assert.AreEqual(4, NotationParser.ToColumn('E'));
            Assert.AreEqual(5, NotationParser.ToColumn('F'));
            Assert.AreEqual(6, NotationParser.ToColumn('G'));
            Assert.AreEqual(7, NotationParser.ToColumn('H'));
        }
    }
}
