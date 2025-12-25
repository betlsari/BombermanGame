
using System;
using BombermanGame.src.Models;

namespace BombermanGame.src.Patterns.Behavioral.State
{
    public class WinnerState : IPlayerState
    {
        public void Move(Player player, int dx, int dy, Map map)
        {
            Console.WriteLine($"[{player.Name}] has won! No more moves needed.");
        }

        public void PlaceBomb(Player player)
        {
            Console.WriteLine($"[{player.Name}] has won! Game is over.");
        }

        public void TakeDamage(Player player)
        {
            // Winner can't take damage
        }

        public string GetStateName() => "Winner";
    }
}