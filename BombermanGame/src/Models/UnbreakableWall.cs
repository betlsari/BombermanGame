
namespace BombermanGame.src.Models
{
    public class UnbreakableWall : IWall
    {
        public char GetSymbol() => '#';
        public bool IsDestructible() => false;
        public void TakeDamage() { } // Kırılamaz
        public bool IsDestroyed() => false;
        public int GetDurability() => -1; // Sonsuz
    }
}