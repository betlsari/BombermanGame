
using BombermanGame.src.Patterns.Creational.Factory;
using System;

namespace BombermanGame.src.Patterns.Creational.Factory
{
    public static class EnemyFactoryProvider
    {
        public static IEnemyFactory GetFactory(string type)
        {
            return type.ToLower() switch
            {
                "static" => new StaticEnemyFactory(),
                "chase" => new ChaseEnemyFactory(),
                "smart" => new SmartEnemyFactory(),
                _ => throw new ArgumentException($"Unknown enemy type: {type}")
            };
        }
    }
}