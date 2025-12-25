
using System;
using BombermanGame.src.Models;

namespace BombermanGame.src.Patterns.Behavioral.Strategy
{
    public class RandomMovementStrategy : IMovementStrategy
    {
        private Random _random = new Random();

        public Position Move(Position currentPosition, Map map, Position? targetPosition)
        {
            int[] dx = { -1, 1, 0, 0 };
            int[] dy = { 0, 0, -1, 1 };

            int direction = _random.Next(4);
            int newX = currentPosition.X + dx[direction];
            int newY = currentPosition.Y + dy[direction];

            if (map.IsWalkable(newX, newY))
            {
                return new Position(newX, newY);
            }

            return currentPosition;
        }
    }
}