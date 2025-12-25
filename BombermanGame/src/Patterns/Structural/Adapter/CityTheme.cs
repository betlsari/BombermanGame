
using System;

namespace BombermanGame.src.Patterns.Structural.Adapter
{
    public class CityTheme
    {
        public string Name => "City";
        public ConsoleColor ConcreteColor => ConsoleColor.Gray;
        public ConsoleColor BrickColor => ConsoleColor.Red;
        public ConsoleColor MetalColor => ConsoleColor.DarkGray;

        public char GetConcreteChar() => '█';
        public char GetBrickChar() => '▓';
        public char GetMetalChar() => '■';
    }
}