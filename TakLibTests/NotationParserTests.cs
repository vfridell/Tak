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
            Move move = NotationParser.Parse("a5");
            Assert.IsInstanceOfType(move, typeof(PlaceStone));

            Move move2 = NotationParser.Parse("Cd3");
            Assert.IsInstanceOfType(move2, typeof(PlaceCapstone));

            Move move3 = NotationParser.Parse("Sb2");
            Assert.IsInstanceOfType(move3, typeof(PlaceWall));
        }

        [TestMethod]
        public void CannotPlaceOnStack()
        {
            throw new NotImplementedException();
        }

        [TestMethod]
        public void ParseBasicMovement()
        {
            Move move = NotationParser.Parse("a5-");
            Assert.IsInstanceOfType(move, typeof(MoveStack));
            Assert.AreEqual(Direction.Down, (move as MoveStack).DirectionEnum);
            Assert.AreEqual(0, (move as MoveStack).Location.Column);
            Assert.AreEqual(4, (move as MoveStack).Location.Row);
        }

        [TestMethod]
        public void CannotMoveFullStackTwice()
        {
            throw new NotImplementedException();
        }

        [TestMethod]
        public void ParseStackMovement()
        {
            Move move = NotationParser.Parse("5e1+41");
            Assert.IsInstanceOfType(move, typeof(MoveStack));
            Assert.AreEqual(Direction.Up, (move as MoveStack).DirectionEnum);
            Assert.AreEqual(4, (move as MoveStack).Location.Column);
            Assert.AreEqual(0, (move as MoveStack).Location.Row);
            Assert.AreEqual(5, (move as MoveStack).CarryInt);
            Assert.AreEqual(2, (move as MoveStack).DropInts.Count);
            Assert.AreEqual(4, (move as MoveStack).DropInts[0]);
            Assert.AreEqual(1, (move as MoveStack).DropInts[1]);
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
