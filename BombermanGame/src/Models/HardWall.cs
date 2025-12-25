using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace BombermanGame.src.Models
{
    public class HardWall : IWall
    {
        private int _durability = 3;

        public char GetSymbol()
        {
            if (_durability <= 0) return ' ';
            if (_durability == 1) return '░';
            if (_durability == 2) return '▒';
            return '▓';
        }

        public bool IsDestructible() => true;

        public void TakeDamage()
        {
            if (_durability > 0)
                _durability--;
        }

        public bool IsDestroyed() => _durability <= 0;
        public int GetDurability() => _durability;

       
    }
}
