
namespace BombermanGame.src.Models
{
    public class BreakableWall : IWall
    {
        private bool _isDestroyed = false;

        public char GetSymbol() => _isDestroyed ? ' ' : '▒';
        public bool IsDestructible() => true;
        public void TakeDamage() => _isDestroyed = true;
        public bool IsDestroyed() => _isDestroyed;
        public int GetDurability() => _isDestroyed ? 0 : 1;
    }
}