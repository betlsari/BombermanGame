
using System;

namespace BombermanGame.src.Models
{
    public class Bomb
    {
        public Position Position { get; set; }
        public int Power { get; set; }
        public int Timer { get; set; }
        public int OwnerId { get; set; }
        public bool HasExploded { get; set; }
        public int Range { get; internal set; }

        public Bomb(Position position, int power, int ownerId)
        {
            Position = position;
            Power = power;
            OwnerId = ownerId;
            Timer = 3; // 3 saniye
            HasExploded = false;
        }

        public void Update()
        {
            if (Timer > 0 && !HasExploded)
            {
                Timer--;
            }
        }

        public bool ShouldExplode()
        {
            return Timer <= 0 && !HasExploded;
        }
    }
}
