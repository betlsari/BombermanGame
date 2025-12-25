
using BombermanGame.src.Models;

namespace BombermanGame.src.Patterns.Behavioral.Strategy
{
    public interface IMovementStrategy
    {
        Position Move(Position currentPosition, Map map, Position? targetPosition);
    }
}