
using System.Linq;
using BombermanGame.src.Core;
using BombermanGame.src.Models;

namespace BombermanGame.src.Patterns.Behavioral.Command
{
    public class PlaceBombCommand : ICommand
    {
        private readonly Player _player;
        private Bomb? _placedBomb;

        public PlaceBombCommand(Player player)
        {
            _player = player;
        }

        public void Execute()
        {
            _player.PlaceBomb();

            // En son eklenen bombayı kaydet (undo için)
            var gameManager = GameManager.Instance;
            _placedBomb = gameManager.Bombs.LastOrDefault(b =>
                b.OwnerId == _player.Id &&
                b.Position.X == _player.Position.X &&
                b.Position.Y == _player.Position.Y);
        }

        public void Undo()
        {
            if (_placedBomb != null)
            {
                var gameManager = GameManager.Instance;
                gameManager.Bombs.Remove(_placedBomb);
            }
        }

        public string GetDescription()
        {
            return $"Place bomb by {_player.Name}";
        }
    }
}
