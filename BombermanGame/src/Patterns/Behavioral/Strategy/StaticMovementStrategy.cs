
using BombermanGame.src.Models;

namespace BombermanGame.src.Patterns.Behavioral.Strategy
{
    public class StaticMovementStrategy : IMovementStrategy
    {
        public Position Move(Position currentPosition, Map map, Position? targetPosition)
        {
            // Statik düşman hareket etmez
            return currentPosition;
        }
    }
}