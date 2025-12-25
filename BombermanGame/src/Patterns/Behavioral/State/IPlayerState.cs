
using BombermanGame.src.Models;

namespace BombermanGame.src.Patterns.Behavioral.State
{
    public interface IPlayerState
    {
        void Move(Player player, int dx, int dy, Map map);
        void PlaceBomb(Player player);
        void TakeDamage(Player player);
        string GetStateName();
    }
}
