using System.Collections.Generic;
using System.Security.Cryptography;
using QuickGraph;
using QuickGraph.Algorithms.Search;

namespace TakLib
{
    public class RoadFinder
    {
        public readonly int BoardSize;
        public int MaxCoord => BoardSize - 1;

        public Dictionary<Coordinate, HashSet<Coordinate>> DestinationsDictionary = new Dictionary<Coordinate, HashSet<Coordinate>>();

        public RoadFinder(int boardSize)
        {
            BoardSize = boardSize;
            for (int i = 0; i < MaxCoord; i++)
            {
                Coordinate leftSide = new Coordinate(i, 0);
                Coordinate rightSide = new Coordinate(MaxCoord-i, MaxCoord);
                Coordinate topSide = new Coordinate(MaxCoord, i);
                Coordinate bottomSide = new Coordinate(0, MaxCoord-i);

                DestinationsDictionary.Add(leftSide, new HashSet<Coordinate>());
                DestinationsDictionary.Add(rightSide, new HashSet<Coordinate>());
                DestinationsDictionary.Add(topSide, new HashSet<Coordinate>());
                DestinationsDictionary.Add(bottomSide, new HashSet<Coordinate>());

                for (int j = 0; j < BoardSize; j++)
                {
                    DestinationsDictionary[leftSide].Add(new Coordinate(MaxCoord - j, MaxCoord));
                    if(IsCorner(leftSide)) DestinationsDictionary[leftSide].Add(new Coordinate(MaxCoord, j));

                    DestinationsDictionary[rightSide].Add(new Coordinate(j, 0));
                    if(IsCorner(rightSide)) DestinationsDictionary[rightSide].Add(new Coordinate(0, MaxCoord - j));

                    DestinationsDictionary[topSide].Add(new Coordinate(0, MaxCoord - j));
                    if(IsCorner(topSide)) DestinationsDictionary[topSide].Add(new Coordinate(MaxCoord - j, MaxCoord));

                    DestinationsDictionary[bottomSide].Add(new Coordinate(MaxCoord, j));
                    if(IsCorner(bottomSide)) DestinationsDictionary[bottomSide].Add(new Coordinate(j, 0));
                }
            }
        }

        public bool IsCorner(Coordinate coordinate)
        {
            return (coordinate.Row == 0 || coordinate.Row == MaxCoord) &&
                    (coordinate.Column == 0 || coordinate.Column == MaxCoord);
        }

        public bool IsSide(Coordinate coordinate)
        {
            return coordinate.Row == 0 || 
                   coordinate.Column == 0 ||
                   coordinate.Row == MaxCoord || 
                   coordinate.Column == MaxCoord;
        }

        public void Analyze(Board board, PieceColor color)
        {
            var spaceAdjacencyGraph = CreateAdjacencyList(board);
            spaceAdjacencyGraph.RemoveVertexIf(v => !v.ColorEquals(color));
            var dfs = new UndirectedDepthFirstSearchAlgorithm<Space, UndirectedEdge<Space>>(spaceAdjacencyGraph);
            var foo = new QuickGraph.Algorithms.ConnectedComponents.ConnectedComponentsAlgorithm<Space, UndirectedEdge<Space>>(spaceAdjacencyGraph);
            foo.Compute();
            
        }

        private UndirectedGraph<Space, UndirectedEdge<Space>> CreateAdjacencyList(Board board)
        {
            var spaceAdjacencyGraph = new UndirectedGraph<Space, UndirectedEdge<Space>>();
            var finishedVertices = new HashSet<Space>();
            foreach (Coordinate c in new CoordinateEnumerable(BoardSize))
            {
                var currentSpace = board.GetSpace(c);
                
                var uSpace = board.GetSpace(c.Add(c.GetNeighbor(Direction.Up)));
                var dSpace = board.GetSpace(c.Add(c.GetNeighbor(Direction.Down)));
                var lSpace = board.GetSpace(c.Add(c.GetNeighbor(Direction.Left)));
                var rSpace = board.GetSpace(c.Add(c.GetNeighbor(Direction.Right)));

                if(uSpace.OnTheBoard && !finishedVertices.Contains(uSpace)) spaceAdjacencyGraph.AddVerticesAndEdge(new UndirectedEdge<Space>(currentSpace, uSpace));
                if(dSpace.OnTheBoard && !finishedVertices.Contains(dSpace)) spaceAdjacencyGraph.AddVerticesAndEdge(new UndirectedEdge<Space>(currentSpace, dSpace));
                if(lSpace.OnTheBoard && !finishedVertices.Contains(lSpace)) spaceAdjacencyGraph.AddVerticesAndEdge(new UndirectedEdge<Space>(currentSpace, lSpace));
                if(rSpace.OnTheBoard && !finishedVertices.Contains(rSpace)) spaceAdjacencyGraph.AddVerticesAndEdge(new UndirectedEdge<Space>(currentSpace, rSpace));

                finishedVertices.Add(currentSpace);
            }

            return spaceAdjacencyGraph;
        }
    }
}
