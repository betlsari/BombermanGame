
using System;

namespace BombermanGame.src.Patterns.Behavioral.Observer
{
    public class UIObserver : IObserver
    {
        public void Update(GameEvent gameEvent)
        {
            switch (gameEvent.Type)
            {
                case EventType.BombExploded:
                    Console.WriteLine("[UI] 💥 BOOM! Bomb exploded!");
                    break;

                case EventType.PlayerDied:
                    Console.WriteLine($"[UI] ☠️  Player died: {gameEvent.Data}");
                    break;

                case EventType.PowerUpCollected:
                    Console.WriteLine($"[UI] ⭐ Power-up collected: {gameEvent.Data}");
                    break;

                case EventType.WallDestroyed:
                    Console.WriteLine("[UI] 🧱 Wall destroyed!");
                    break;

                case EventType.EnemyKilled:
                    Console.WriteLine("[UI] 👾 Enemy eliminated!");
                    break;

                case EventType.GameEnded:
                    Console.WriteLine($"[UI] 🏆 Game Over! Winner: {gameEvent.Data}");
                    break;
            }
        }
    }
}