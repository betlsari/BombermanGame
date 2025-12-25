
using System;

namespace BombermanGame.src.Patterns.Behavioral.Observer
{
    public enum EventType
    {
        BombExploded,
        PlayerDied,
        PowerUpCollected,
        WallDestroyed,
        GameEnded,
        EnemyKilled
    }

    public class GameEvent
    {
        public EventType Type { get; set; }
        public object? Data { get; set; }
        public DateTime Timestamp { get; set; }

        public GameEvent(EventType type, object? data = null)
        {
            Type = type;
            Data = data;
            Timestamp = DateTime.Now;
        }
    }
}
