
using BombermanGame.src.Models;

namespace BombermanGame.src.Patterns.Structural.Decorator
{
    public class BombCountDecorator : PlayerDecorator
    {
        private int _additionalBombs;

        public BombCountDecorator(IPlayer player, int additionalBombs = 1) : base(player)
        {
            _additionalBombs = additionalBombs;
        }

        public override int BombCount => _decoratedPlayer.BombCount + _additionalBombs;

        public override string GetStats()
        {
            return base.GetStats() + $" +{_additionalBombs} Bombs";
        }
    }
}