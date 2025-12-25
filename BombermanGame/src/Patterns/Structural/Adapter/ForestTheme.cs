
using System;

namespace BombermanGame.src.Patterns.Structural.Adapter
{
    public class ForestTheme
    {
        public string Name => "Forest";
        public ConsoleColor GrassColor => ConsoleColor.Green;
        public ConsoleColor TreeColor => ConsoleColor.DarkGreen;
        public ConsoleColor LogColor => ConsoleColor.DarkYellow;

        public char GetGrassChar() => '·';
        public char GetTreeChar() => '♣';
        public char GetLogChar() => '≡';
    }
}