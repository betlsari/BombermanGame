
using BombermanGame.src.Models;

namespace BombermanGame.src.Patterns.Creational.Factory
{
    public interface IEnemyFactory
    {
        Enemy CreateEnemy(int id, Position position);
    }
}