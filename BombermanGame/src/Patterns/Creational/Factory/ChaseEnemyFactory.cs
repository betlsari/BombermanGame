
using BombermanGame.src.Patterns.Behavioral.Strategy;
using BombermanGame.src.Models;
using System;

namespace BombermanGame.src.Patterns.Creational.Factory
{
    public class ChaseEnemyFactory : IEnemyFactory
    {
        public Enemy CreateEnemy(int id, Position position)
        {
            var enemy = new Enemy(id, position, EnemyType.Chase);
            enemy.MovementStrategy = new ChaseMovementStrategy();
            return enemy;
        }
    }
}