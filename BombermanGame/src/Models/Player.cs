
using BombermanGame.src.Patterns.Behavioral.State;
using BombermanGame.src.Patterns.Structural.Decorator;

namespace BombermanGame.src.Models
{
    public class Player : IPlayer
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public Position Position { get; set; }
        public int BombCount { get; private set; }
        public int BombPower { get; private set; }
        public int Speed { get; private set; }
        public bool IsAlive { get; set; }
        public IPlayerState? State { get; set; }
        public int Score { get; set; }
        public int Health { get; internal set; }
        public int BombRange { get; internal set; }
        public int CurrentBombs { get; internal set; }

        public Player(int id, string name, Position position)
        {
            Id = id;
            Name = name;
            Position = position;
            BombCount = 1;
            BombPower = 1;
            Speed = 1;
            IsAlive = true;
            Score = 0;
            State = new AliveState();
        }

        public void Move(int dx, int dy, Map map)
        {
            if (State != null)
            {
                State.Move(this, dx, dy, map);
            }
        }

        public void PlaceBomb()
        {
            if (State != null)
            {
                State.PlaceBomb(this);
            }
        }

        public string GetStats()
        {
            return $"{Name} - Bombs: {BombCount}, Power: {BombPower}, Speed: {Speed}";
        }

        public void IncreaseBombCount() => BombCount++;
        public void IncreaseBombPower() => BombPower++;
        public void IncreaseSpeed() => Speed++;
    }
}