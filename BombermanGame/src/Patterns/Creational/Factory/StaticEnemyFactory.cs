
using BombermanGame.src.Patterns.Behavioral.Strategy;
using BombermanGame.src.Models;

namespace BombermanGame.src.Patterns.Creational.Factory
{
    public class StaticEnemyFactory : IEnemyFactory
    {
        public Enemy CreateEnemy(int id, Position position)
        {
            var enemy = new Enemy(id, position, EnemyType.Static);
            enemy.MovementStrategy = new StaticMovementStrategy();
            return enemy;
        }
    }
}