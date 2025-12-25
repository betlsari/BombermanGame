
using System;

namespace BombermanGame.src.Patterns.Structural.Adapter
{
    public class DesertTheme
    {
        public string Name => "Desert";
        public ConsoleColor SandColor => ConsoleColor.Yellow;
        public ConsoleColor StoneColor => ConsoleColor.DarkYellow;
        public ConsoleColor RockColor => ConsoleColor.Gray;

        public char GetSandChar() => '░';
        public char GetStoneChar() => '▒';
        public char GetRockChar() => '▓';
    }
}