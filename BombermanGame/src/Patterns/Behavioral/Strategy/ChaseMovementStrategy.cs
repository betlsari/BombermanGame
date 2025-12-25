using System;
using BombermanGame.src.Models;

namespace BombermanGame.src.Patterns.Behavioral.Strategy
{
    public class ChaseMovementStrategy : IMovementStrategy
    {
        public Position Move(Position currentPosition, Map map, Position? targetPosition)
        {
            if (targetPosition == null)
            {
                return currentPosition;
            }

            int dx = 0, dy = 0;

            // Basit takip: oyuncuya doğru hareket et
            if (Math.Abs(currentPosition.X - targetPosition.X) > Math.Abs(currentPosition.Y - targetPosition.Y))
            {
                dx = currentPosition.X < targetPosition.X ? 1 : -1;
            }
            else
            {
                dy = currentPosition.Y < targetPosition.Y ? 1 : -1;
            }

            int newX = currentPosition.X + dx;
            int newY = currentPosition.Y + dy;

            if (map.IsWalkable(newX, newY))
            {
                return new Position(newX, newY);
            }

            return currentPosition;
        }
    }
}