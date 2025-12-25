
using System;
using BombermanGame.src.Core;
using BombermanGame.src.Patterns.Repository;

namespace BombermanGame.src.Patterns.Behavioral.Observer
{
    public class StatsObserver : IObserver
    {
        private readonly StatsRepository _statsRepository;
        private int _wallsDestroyed = 0;
        private int _enemiesKilled = 0;
        private int _powerUpsCollected = 0;

        public StatsObserver()
        {
            _statsRepository = new StatsRepository();
        }

        public void Update(GameEvent gameEvent)
        {
            switch (gameEvent.Type)
            {
                case EventType.WallDestroyed:
                    _wallsDestroyed++;
                    Console.WriteLine($"[STATS] Walls destroyed: {_wallsDestroyed}");
                    break;

                case EventType.EnemyKilled:
                    _enemiesKilled++;
                    Console.WriteLine($"[STATS] Enemies killed: {_enemiesKilled}");
                    break;

                case EventType.PowerUpCollected:
                    _powerUpsCollected++;
                    Console.WriteLine($"[STATS] Power-ups collected: {_powerUpsCollected}");
                    break;

                case EventType.GameEnded:
                    SaveGameStats();
                    break;
            }
        }

        private void SaveGameStats()
        {
            var gameManager = GameManager.Instance;
            if (gameManager.CurrentUserId > 0)
            {
                Console.WriteLine($"\n[STATS SUMMARY]");
                Console.WriteLine($"Walls Destroyed: {_wallsDestroyed}");
                Console.WriteLine($"Enemies Killed: {_enemiesKilled}");
                Console.WriteLine($"Power-ups Collected: {_powerUpsCollected}");
            }
        }

        public void Reset()
        {
            _wallsDestroyed = 0;
            _enemiesKilled = 0;
            _powerUpsCollected = 0;
        }

        // Getter metodları
        public int GetWallsDestroyed() => _wallsDestroyed;
        public int GetEnemiesKilled() => _enemiesKilled;
        public int GetPowerUpsCollected() => _powerUpsCollected;
    }
}