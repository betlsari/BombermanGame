using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace BombermanGame.src.Models.Entities
{
    public class GameStatistic
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public int Wins { get; set; }
        public int Losses { get; set; }
        public int TotalGames { get; set; }
    }
}