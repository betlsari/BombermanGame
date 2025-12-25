
namespace BombermanGame.src.Models
{
    public interface IWall
    {
        char GetSymbol();
        bool IsDestructible();
        void TakeDamage();
        bool IsDestroyed();
        int GetDurability();
        
    }
}
