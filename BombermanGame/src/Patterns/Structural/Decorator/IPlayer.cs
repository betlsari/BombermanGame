
using BombermanGame.src.Models;

namespace BombermanGame.src.Patterns.Structural.Decorator
{
    public interface IPlayer
    {
        int Id { get; }
        string Name { get; }
        Position Position { get; set; }
        int BombCount { get; }
        int BombPower { get; }
        int Speed { get; }
        bool IsAlive { get; set; }

        void Move(int dx, int dy, Map map);
        void PlaceBomb();
        string GetStats();
    }
}