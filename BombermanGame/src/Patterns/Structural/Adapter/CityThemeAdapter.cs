
using System;

namespace BombermanGame.src.Patterns.Structural.Adapter
{
    public class CityThemeAdapter : ITheme
    {
        private readonly CityTheme _cityTheme;

        public CityThemeAdapter()
        {
            _cityTheme = new CityTheme();
        }

        public string GetName() => _cityTheme.Name;
        public ConsoleColor GetUnbreakableWallColor() => _cityTheme.MetalColor;
        public ConsoleColor GetBreakableWallColor() => _cityTheme.BrickColor;
        public ConsoleColor GetHardWallColor() => ConsoleColor.Gray;
        public ConsoleColor GetGroundColor() => _cityTheme.ConcreteColor;
        public char GetUnbreakableWallChar() => _cityTheme.GetMetalChar();
        public char GetBreakableWallChar() => _cityTheme.GetBrickChar();
        public char GetHardWallChar() => _cityTheme.GetConcreteChar();
    }
}
