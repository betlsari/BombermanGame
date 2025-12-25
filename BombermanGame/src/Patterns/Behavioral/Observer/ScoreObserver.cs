
using System;

namespace BombermanGame.src.Patterns.Behavioral.Observer
{
    public class ScoreObserver : IObserver
    {
        private int _currentScore = 0;

        public void Update(GameEvent gameEvent)
        {
            switch (gameEvent.Type)
            {
                case EventType.WallDestroyed:
                    _currentScore += 10;
                    Console.WriteLine($"[SCORE] +10 points! Total: {_currentScore}");
                    break;

                case EventType.EnemyKilled:
                    _currentScore += 50;
                    Console.WriteLine($"[SCORE] +50 points! Total: {_currentScore}");
                    break;

                case EventType.PowerUpCollected:
                    _currentScore += 25;
                    Console.WriteLine($"[SCORE] +25 points! Total: {_currentScore}");
                    break;

                case EventType.GameEnded:
                    Console.WriteLine($"[SCORE] Final Score: {_currentScore}");
                    break;
            }
        }

        public int GetScore() => _currentScore;
        public void ResetScore() => _currentScore = 0;
    }
}