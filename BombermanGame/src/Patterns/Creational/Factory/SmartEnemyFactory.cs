
using BombermanGame.src.Patterns.Behavioral.Strategy;
using BombermanGame.src.Models;
using System;

namespace BombermanGame.src.Patterns.Creational.Factory
{
    public class SmartEnemyFactory : IEnemyFactory
    {
        public Enemy CreateEnemy(int id, Position position)
        {
            var enemy = new Enemy(id, position, EnemyType.Smart);
            enemy.MovementStrategy = new PathfindingMovementStrategy();
            return enemy;
        }
    }
}
