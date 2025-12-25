
using BombermanGame.src.Models;

namespace BombermanGame.src.Patterns.Structural.Decorator
{
    public class SpeedBoostDecorator : PlayerDecorator
    {
        private int _speedBoost;

        public SpeedBoostDecorator(IPlayer player, int speedBoost = 1) : base(player)
        {
            _speedBoost = speedBoost;
        }

        public override int Speed => _decoratedPlayer.Speed + _speedBoost;

        public override string GetStats()
        {
            return base.GetStats() + $" +{_speedBoost} Speed";
        }
    }
}