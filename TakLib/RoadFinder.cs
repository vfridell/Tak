using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using QuickGraph;
using QuickGraph.Algorithms.Search;
using System.Collections.Concurrent;

namespace TakLib
{
    public class RoadFinder
    {
        public readonly RoadEndPoints DestinationsDictionary;
        public IDictionary<int, IEnumerable<Space>> Roads = new Dictionary<int, IEnumerable<Space>>();
        public PieceColor ColorAnalyzed { get; protected set; }
        public int SubGraphCount { get; protected set; }
        public double AverageSubGraphLength { get; protected set; }
        public int LongestSubGraphLength { get; protected set; }

        public RoadFinder(int boardSize)
        {
            DestinationsDictionary = RoadEndPoints.GetRoadEndPoints(boardSize);
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
            foreach (Coordinate c in new CoordinateEnumerable(DestinationsDictionary.BoardSize))
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

        public bool IsCorner(Coordinate c) => DestinationsDictionary.IsCorner(c);
        public bool IsSide(Coordinate c) => DestinationsDictionary.IsSide(c);
    }
}
