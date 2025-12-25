using System;


namespace BombermanGame.src.Models
{
    public enum PowerUpType
    {
        BombCount,
        BombPower,
        SpeedBoost
    }

    public class PowerUp
    {
        public Position Position { get; set; }
        public PowerUpType Type { get; set; }
        public bool IsCollected { get; set; }

        public PowerUp(Position position, PowerUpType type)
        {
            Position = position;
            Type = type;
            IsCollected = false;
        }

        public char GetSymbol()
        {
            return Type switch
            {
                PowerUpType.BombCount => 'B',
                PowerUpType.BombPower => 'P',
                PowerUpType.SpeedBoost => 'S',
                _ => '?'
            };
        }
    }
}
