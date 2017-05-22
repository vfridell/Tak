using QuickGraph;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TakLib
{
    public class SpaceGraph : UndirectedGraph<Space, UndirectedEdge<Space>>
    {
        PieceColor _color;

        private SpaceGraph() { }

        public SpaceGraph(Board board, PieceColor color)
        {
            _color = color;
            var finishedVertices = new HashSet<Space>();
            foreach (Coordinate c in new CoordinateEnumerable(board.Size))
            {
                var currentSpace = board.GetSpace(c);

                var uSpace = board.GetSpace(c.GetNeighbor(Direction.Up));
                var dSpace = board.GetSpace(c.GetNeighbor(Direction.Down));
                var lSpace = board.GetSpace(c.GetNeighbor(Direction.Left));
                var rSpace = board.GetSpace(c.GetNeighbor(Direction.Right));

                if (!finishedVertices.Contains(uSpace)) AddVertexAndEdge(currentSpace, uSpace);
                if (!finishedVertices.Contains(dSpace)) AddVertexAndEdge(currentSpace, dSpace);
                if (!finishedVertices.Contains(lSpace)) AddVertexAndEdge(currentSpace, lSpace);
                if (!finishedVertices.Contains(rSpace)) AddVertexAndEdge(currentSpace, rSpace);

                finishedVertices.Add(currentSpace);
            }
        }

        public void ChangeStackVertex(Board board, Space oldSpace, Space newSpace)
        {
            if (oldSpace.Equals(newSpace)) return;  // nothing to do
            if (!oldSpace.Coordinate.Equals(newSpace.Coordinate)) throw new ArgumentException("Space coordinates must be equal");

            var uSpace = board.GetSpace(oldSpace.Coordinate.GetNeighbor(Direction.Up));
            var dSpace = board.GetSpace(oldSpace.Coordinate.GetNeighbor(Direction.Down));
            var lSpace = board.GetSpace(oldSpace.Coordinate.GetNeighbor(Direction.Left));
            var rSpace = board.GetSpace(oldSpace.Coordinate.GetNeighbor(Direction.Right));

            if(ContainsVertex(oldSpace)) RemoveVertex(oldSpace);

            if (newSpace.ColorEquals(_color))
            {
                AddVertex(newSpace);
                AddVertexAndEdge(newSpace, uSpace);
                AddVertexAndEdge(newSpace, dSpace);
                AddVertexAndEdge(newSpace, lSpace);
                AddVertexAndEdge(newSpace, rSpace);
            }

        }

        private void AddVertexAndEdge(Space confirmedSpace, Space adjacentSpace)
        {
            if (adjacentSpace.OnTheBoard && 
                adjacentSpace.Piece?.Type != PieceType.Wall &&
                adjacentSpace.ColorEquals(_color)
                )
                AddVerticesAndEdge(new UndirectedEdge<Space>(confirmedSpace, adjacentSpace));
        }

        public SpaceGraph Clone()
        {
            SpaceGraph clone = new SpaceGraph();
            foreach(Space s in Vertices) clone.AddVertex(s);
            foreach (UndirectedEdge<Space> e in Edges) clone.AddEdge(e);
            clone._color = _color;
            return clone;
        }
    }
}
