
using System;
using BombermanGame.src.Audio;

namespace BombermanGame.src.Patterns.Behavioral.Observer
{
    
    public class SoundObserver : IObserver
    {
        private readonly SoundManager _soundManager;

        public SoundObserver()
        {
            _soundManager = SoundManager.Instance;
        }

        public void Update(GameEvent gameEvent)
        {
            // Her event türüne göre uygun ses çal
            switch (gameEvent.Type)
            {
                case EventType.BombExploded:
                    _soundManager.PlaySound(SoundType.BombExplode);
                    Console.WriteLine("[SOUND] 💥 Bomb explosion sound!");
                    break;

                case EventType.PlayerDied:
                    _soundManager.PlaySound(SoundType.PlayerDeath);
                    Console.WriteLine($"[SOUND] ☠️  Player death sound: {gameEvent.Data}");
                    break;

                case EventType.PowerUpCollected:
                    _soundManager.PlaySound(SoundType.PowerUpCollect);
                    Console.WriteLine($"[SOUND] ⭐ Power-up collected sound: {gameEvent.Data}");
                    break;

                case EventType.WallDestroyed:
                    _soundManager.PlaySound(SoundType.WallBreak);
                    Console.WriteLine("[SOUND] 🧱 Wall break sound!");
                    break;

                case EventType.EnemyKilled:
                    _soundManager.PlaySound(SoundType.EnemyDeath);
                    Console.WriteLine("[SOUND] 👾 Enemy death sound!");
                    break;

                case EventType.GameEnded:
                    // Kazanan var mı kontrol et
                    string result = gameEvent.Data?.ToString() ?? "";
                    if (result.Contains("WIN") || result.Contains("VICTORY"))
                    {
                        _soundManager.PlaySound(SoundType.Victory);
                        Console.WriteLine("[SOUND] 🏆 Victory sound!");
                    }
                    else
                    {
                        _soundManager.PlaySound(SoundType.GameOver);
                        Console.WriteLine("[SOUND] 💀 Game over sound!");
                    }
                    break;
            }
        }
    }
}