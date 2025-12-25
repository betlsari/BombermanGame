using System;
using System.Collections.Generic;
using System.Linq;
using BombermanGame.src.Models;

namespace BombermanGame.src.Utils
{
    public class AStarNode
    {
        public Position Position { get; set; }
        public AStarNode? Parent { get; set; }
        public int G { get; set; } // Cost from start
        public int H { get; set; } // Heuristic cost to end
        public int F => G + H; // Total cost

        public AStarNode(Position position)
        {
            Position = position;
        }
    }

    public static class AStar
    {
        public static List<Position>? FindPath(Position start, Position end, Map map)
        {
            var openList = new List<AStarNode>();
            var closedList = new HashSet<string>();

            var startNode = new AStarNode(start) { G = 0, H = GetHeuristic(start, end) };
            openList.Add(startNode);

            while (openList.Count > 0)
            {
                // En düşük F değerine sahip node'u seç
                var currentNode = openList.OrderBy(n => n.F).First();
                openList.Remove(currentNode);

                string key = $"{currentNode.Position.X},{currentNode.Position.Y}";
                if (closedList.Contains(key))
                    continue;

                closedList.Add(key);

                // Hedefe ulaştık mı?
                if (currentNode.Position.X == end.X && currentNode.Position.Y == end.Y)
                {
                    return ReconstructPath(currentNode);
                }

                // Komşuları kontrol et
                foreach (var neighbor in GetNeighbors(currentNode.Position, map))
                {
                    string neighborKey = $"{neighbor.X},{neighbor.Y}";
                    if (closedList.Contains(neighborKey))
                        continue;

                    int gScore = currentNode.G + 1;
                    var neighborNode = openList.FirstOrDefault(n =>
                        n.Position.X == neighbor.X && n.Position.Y == neighbor.Y);

                    if (neighborNode == null)
                    {
                        neighborNode = new AStarNode(neighbor)
                        {
                            Parent = currentNode,
                            G = gScore,
                            H = GetHeuristic(neighbor, end)
                        };
                        openList.Add(neighborNode);
                    }
                    else if (gScore < neighborNode.G)
                    {
                        neighborNode.Parent = currentNode;
                        neighborNode.G = gScore;
                    }
                }
            }

            return null; // Yol bulunamadı
        }

        private static int GetHeuristic(Position a, Position b)
        {
            // Manhattan distance
            return Math.Abs(a.X - b.X) + Math.Abs(a.Y - b.Y);
        }

        private static List<Position> GetNeighbors(Position pos, Map map)
        {
            var neighbors = new List<Position>();
            int[] dx = { -1, 1, 0, 0 };
            int[] dy = { 0, 0, -1, 1 };

            for (int i = 0; i < 4; i++)
            {
                int newX = pos.X + dx[i];
                int newY = pos.Y + dy[i];

                if (map.IsWalkable(newX, newY))
                {
                    neighbors.Add(new Position(newX, newY));
                }
            }

            return neighbors;
        }

        private static List<Position> ReconstructPath(AStarNode endNode)
        {
            var path = new List<Position>();
            var current = endNode;

            while (current != null)
            {
                path.Add(current.Position);
                current = current.Parent;
            }

            path.Reverse();
            return path;
        }
    }
}