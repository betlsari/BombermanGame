
using System.Collections.Generic;
using System.Linq;
using BombermanGame.src.Models;
using BombermanGame.src.Utils;

namespace BombermanGame.src.Patterns.Behavioral.Strategy
{
    public class PathfindingMovementStrategy : IMovementStrategy
    {
        public Position Move(Position currentPosition, Map map, Position? targetPosition)
        {
            if (targetPosition == null)
            {
                return currentPosition;
            }

            // A* algoritması ile en kısa yolu bul
            List<Position> path = AStar.FindPath(currentPosition, targetPosition, map);

            if (path != null && path.Count > 1)
            {
                return path[1]; // İlk adımı at (current position), ikinci pozisyona git
            }

            return currentPosition;
        }
    }
}