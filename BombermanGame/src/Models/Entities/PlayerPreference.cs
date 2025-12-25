using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace BombermanGame.src.Models.Entities
{
    public class PlayerPreference
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public string Theme { get; set; } = "Desert";
        public bool SoundEnabled { get; set; } = true;
    }
}
