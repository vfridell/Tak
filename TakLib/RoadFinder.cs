using System.Collections.Generic;
using System.Security.Cryptography;
using QuickGraph;

namespace TakLib
{
    public class RoadFinder
    {
        public readonly int BoardSize;
        public int MaxCoord => BoardSize - 1;

        public Dictionary<Coordinate, HashSet<Coordinate>> DestinationsDictionary = new Dictionary<Coordinate, HashSet<Coordinate>>();
        private UndirectedGraph<Piece, UndirectedEdge<Piece>> _whiteAdjacencyGraph = new UndirectedGraph<Piece, UndirectedEdge<Piece>>();
        private UndirectedGraph<Piece, UndirectedEdge<Piece>> _blackAdjacencyGraph = new UndirectedGraph<Piece, UndirectedEdge<Piece>>();

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

        public void Analyze(Board board)
        {
            
        }

        private void CreateAdjacencyList(Board board)
        {
            _whiteAdjacencyGraph.Clear();
            var finishedVertices = new HashSet<Piece>();
            foreach (Coordinate c in new CoordinateEnumerable(BoardSize))
            {
                var cUp = c.Add(c.GetNeighbor(Direction.Up));
                var cDown = c.Add(c.GetNeighbor(Direction.Down));
                var cLeft = c.Add(c.GetNeighbor(Direction.Left));
                var cRight = c.Add(c.GetNeighbor(Direction.Right));

                if (board.GetPiece(cUp).Color == PieceColor.White)
                {
                    
                }

                foreach (Hex directionHex in Neighborhood.neighborDirections)
                {
                    // don't do the center
                    if (directionHex.Equals(new Hex(0, 0))) continue;

                    Hex adjacentHex = kvp.Value + directionHex;
                    Piece adjacentPiece;
                    if (TryGetPieceAtHex(adjacentHex, out adjacentPiece))
                    {
                        if (!finishedVertices.Contains(adjacentPiece))
                        {
                            _adjacencyGraph.AddVerticesAndEdge(new UndirectedEdge<Piece>(kvp.Key, adjacentPiece));
                        }
                    }
                }
                finishedVertices.Add(kvp.Key);
            }
        }
    }
}
