
using BombermanGame.src.Models;

namespace BombermanGame.src.Patterns.Structural.Decorator
{
    public abstract class PlayerDecorator : IPlayer
    {
        protected IPlayer _decoratedPlayer;

        public PlayerDecorator(IPlayer player)
        {
            _decoratedPlayer = player;
        }

        public virtual int Id => _decoratedPlayer.Id;
        public virtual string Name => _decoratedPlayer.Name;
        public virtual Position Position
        {
            get => _decoratedPlayer.Position;
            set => _decoratedPlayer.Position = value;
        }
        public virtual int BombCount => _decoratedPlayer.BombCount;
        public virtual int BombPower => _decoratedPlayer.BombPower;
        public virtual int Speed => _decoratedPlayer.Speed;
        public virtual bool IsAlive
        {
            get => _decoratedPlayer.IsAlive;
            set => _decoratedPlayer.IsAlive = value;
        }

        public virtual void Move(int dx, int dy, Map map)
        {
            _decoratedPlayer.Move(dx, dy, map);
        }

        public virtual void PlaceBomb()
        {
            _decoratedPlayer.PlaceBomb();
        }

        public virtual string GetStats()
        {
            return _decoratedPlayer.GetStats();
        }
    }
}