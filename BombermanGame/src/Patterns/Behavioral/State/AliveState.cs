
using System;
using BombermanGame.src.Models;
using BombermanGame.src.Core;


namespace BombermanGame.src.Patterns.Behavioral.State
{
    public class AliveState : IPlayerState
    {
        public void Move(Player player, int dx, int dy, Map map)
        {
            int newX = player.Position.X + dx;
            int newY = player.Position.Y + dy;

            if (map.IsWalkable(newX, newY))
            {
                player.Position.X = newX;
                player.Position.Y = newY;
            }
        }

        public void PlaceBomb(Player player)
        {
            var gameManager = GameManager.Instance;

            // Mevcut pozisyonda zaten bomba var mı kontrol et
            bool bombExists = gameManager.Bombs.Exists(b =>
                b.Position.X == player.Position.X &&
                b.Position.Y == player.Position.Y);

            if (!bombExists)
            {
                var bomb = new Bomb(
                    new Position(player.Position.X, player.Position.Y),
                    player.BombPower,
                    player.Id
                );
                gameManager.Bombs.Add(bomb);
                Console.WriteLine($"[{player.Name}] Bomb placed at ({player.Position.X}, {player.Position.Y})");
            }
        }

        public void TakeDamage(Player player)
        {
            player.IsAlive = false;
            player.State = new DeadState();
            Console.WriteLine($"[{player.Name}] has died!");
        }

        public string GetStateName() => "Alive";
    }
}