
using BombermanGame.src.Models;

namespace BombermanGame.src.Patterns.Behavioral.Command
{
    public class MoveCommand : ICommand
    {
        private readonly Player _player;
        private readonly int _dx;
        private readonly int _dy;
        private readonly Map _map;
        private Position? _previousPosition;

        public MoveCommand(Player player, int dx, int dy, Map map)
        {
            _player = player;
            _dx = dx;
            _dy = dy;
            _map = map;
        }

        public void Execute()
        {
            _previousPosition = new Position(_player.Position.X, _player.Position.Y);
            _player.Move(_dx, _dy, _map);
        }

        public void Undo()
        {
            if (_previousPosition != null)
            {
                _player.Position = _previousPosition;
            }
        }

        public string GetDescription()
        {
            return $"Move {_player.Name} by ({_dx}, {_dy})";
        }
    }
}