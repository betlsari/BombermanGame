
using BombermanGame.src.Models;

namespace BombermanGame.src.Patterns.Structural.Decorator
{
    public class BombPowerDecorator : PlayerDecorator
    {
        private int _additionalPower;

        public BombPowerDecorator(IPlayer player, int additionalPower = 1) : base(player)
        {
            _additionalPower = additionalPower;
        }

        public override int BombPower => _decoratedPlayer.BombPower + _additionalPower;

        public override string GetStats()
        {
            return base.GetStats() + $" +{_additionalPower} Power";
        }
    }
}