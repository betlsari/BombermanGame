
using System;
using BombermanGame.src.Models;

namespace BombermanGame.src.Patterns.Behavioral.State
{
    public class DeadState : IPlayerState
    {
        public void Move(Player player, int dx, int dy, Map map)
        {
            Console.WriteLine($"[{player.Name}] cannot move - player is dead!");
        }

        public void PlaceBomb(Player player)
        {
            Console.WriteLine($"[{player.Name}] cannot place bomb - player is dead!");
        }

        public void TakeDamage(Player player)
        {
            // Already dead, nothing happens
        }

        public string GetStateName() => "Dead";
    }
}