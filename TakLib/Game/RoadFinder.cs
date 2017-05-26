using System;
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
        public int AverageSubGraphLength { get; protected set; }
        public int LongestSubGraphLength { get; protected set; }

        public RoadFinder(int boardSize)
        {
            DestinationsDictionary = RoadEndPoints.GetRoadEndPoints(boardSize);
        }

        public void Analyze(Board board, PieceColor color)
        {
            Roads.Clear();
            ColorAnalyzed = color;

            var connectedComponents = board.ComputeConnectedComponents(color);

            // get stats on all subgraphs
            SubGraphCount = connectedComponents.ComponentCount;
            if (connectedComponents.ComponentCount == 0) return;
            AverageSubGraphLength = (int)Math.Ceiling(connectedComponents.Components
                .GroupBy(kvp => kvp.Value)
                .Select(g => g.Count())
                .Average());
            LongestSubGraphLength = connectedComponents.Components
                .GroupBy(kvp => kvp.Value)
                .Select(g => g.Count())
                .Max();

            // find roads
            foreach (Space space in connectedComponents.Components.Keys.Where(s => DestinationsDictionary.ContainsKey(s.Coordinate)))
            {
                int componentNo = connectedComponents.Components[space];
                if(Roads.ContainsKey(componentNo)) continue;
                IEnumerable<Space> connectedNodes = connectedComponents.Components
                    .GroupBy(kvp => kvp.Value)
                    .Where(g => g.Key == componentNo)
                    .SelectMany(g => g).Select(kvp => kvp.Key);
                bool road = connectedNodes.Any(s => DestinationsDictionary[space.Coordinate].Contains(s.Coordinate));
                if(road) Roads.Add(componentNo, connectedNodes);
            }
            
        }

        public bool IsCorner(Coordinate c) => DestinationsDictionary.IsCorner(c);
        public bool IsSide(Coordinate c) => DestinationsDictionary.IsSide(c);
    }
}
