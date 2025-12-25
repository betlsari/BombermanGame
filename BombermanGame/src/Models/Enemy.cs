
using BombermanGame.src.Patterns.Behavioral.Strategy;
using System;

namespace BombermanGame.src.Models
{
    public enum EnemyType
    {
        Static,
        Chase,
        Smart
    }

    public class Enemy
    {
        public int Id { get; set; }
        public Position Position { get; set; }
        public EnemyType Type { get; set; }
        public IMovementStrategy? MovementStrategy { get; set; }
        public bool IsAlive { get; set; }
        public int Health { get; internal set; }

        public Enemy(int id, Position position, EnemyType type)
        {
            Id = id;
            Position = position;
            Type = type;
            IsAlive = true;
        }

        public void Move(Map map, Position? playerPosition)
        {
            if (MovementStrategy != null && IsAlive)
            {
                Position = MovementStrategy.Move(Position, map, playerPosition);
            }
        }

        public char GetSymbol()
        {
            return Type switch
            {
                EnemyType.Static => 'E',
                EnemyType.Chase => 'C',
                EnemyType.Smart => 'A',
                _ => '?'
            };
        }
    }
}