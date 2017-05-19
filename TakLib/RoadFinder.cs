using System.Collections.Generic;
using System.Linq;
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
        public IDictionary<int, IEnumerable<Space>> Roads = new Dictionary<int, IEnumerable<Space>>();
        public PieceColor ColorAnalyzed { get; protected set; }
        public int SubGraphCount { get; protected set; }
        public double AverageSubGraphLength { get; protected set; }
        public int LongestSubGraphLength { get; protected set; }

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
            Roads.Clear();
            ColorAnalyzed = color;
            var spaceAdjacencyGraph = CreateAdjacencyList(board);
            spaceAdjacencyGraph.RemoveVertexIf(v => !v.ColorEquals(color) || v.Piece?.Type == PieceType.Wall);
            if (spaceAdjacencyGraph.VertexCount == 0) return;

            var connectedComponentsAlg = new QuickGraph.Algorithms.ConnectedComponents.ConnectedComponentsAlgorithm<Space, UndirectedEdge<Space>>(spaceAdjacencyGraph);
            connectedComponentsAlg.Compute();

            // get stats on all subgraphs
            SubGraphCount = connectedComponentsAlg.ComponentCount;
            AverageSubGraphLength = connectedComponentsAlg.Components
                .GroupBy(kvp => kvp.Value)
                .Select(g => g.Count())
                .Average();
            LongestSubGraphLength = connectedComponentsAlg.Components
                .GroupBy(kvp => kvp.Value)
                .Select(g => g.Count())
                .Max();

            // find roads
            foreach (Space space in connectedComponentsAlg.Components.Keys.Where(s => DestinationsDictionary.ContainsKey(s.Coordinate)))
            {
                int componentNo = connectedComponentsAlg.Components[space];
                if(Roads.ContainsKey(componentNo)) continue;
                IEnumerable<Space> connectedNodes = connectedComponentsAlg.Components
                    .GroupBy(kvp => kvp.Value)
                    .Where(g => g.Key == componentNo)
                    .SelectMany(g => g).Select(kvp => kvp.Key);
                bool road = connectedNodes.Any(s => DestinationsDictionary[space.Coordinate].Contains(s.Coordinate));
                if(road) Roads.Add(componentNo, connectedNodes);
            }
            
        }

        private UndirectedGraph<Space, UndirectedEdge<Space>> CreateAdjacencyList(Board board)
        {
            var spaceAdjacencyGraph = new UndirectedGraph<Space, UndirectedEdge<Space>>();
            var finishedVertices = new HashSet<Space>();
            foreach (Coordinate c in new CoordinateEnumerable(BoardSize))
            {
                var currentSpace = board.GetSpace(c);
                
                var uSpace = board.GetSpace(c.GetNeighbor(Direction.Up));
                var dSpace = board.GetSpace(c.GetNeighbor(Direction.Down));
                var lSpace = board.GetSpace(c.GetNeighbor(Direction.Left));
                var rSpace = board.GetSpace(c.GetNeighbor(Direction.Right));

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
