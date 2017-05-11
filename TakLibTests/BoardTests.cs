using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TakLib;

namespace TakLibTests
{
    [TestClass]
    public class BoardTests
    {
        [TestMethod]
        public void CloneBoard()
        {
            throw new NotImplementedException();
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
    }
}
