
using System;

namespace BombermanGame.src.Patterns.Structural.Adapter
{
    public class DesertThemeAdapter : ITheme
    {
        private readonly DesertTheme _desertTheme;

        public DesertThemeAdapter()
        {
            _desertTheme = new DesertTheme();
        }

        public string GetName() => _desertTheme.Name;
        public ConsoleColor GetUnbreakableWallColor() => _desertTheme.RockColor;
        public ConsoleColor GetBreakableWallColor() => _desertTheme.StoneColor;
        public ConsoleColor GetHardWallColor() => ConsoleColor.DarkGray;
        public ConsoleColor GetGroundColor() => _desertTheme.SandColor;
        public char GetUnbreakableWallChar() => _desertTheme.GetRockChar();
        public char GetBreakableWallChar() => _desertTheme.GetStoneChar();
        public char GetHardWallChar() => _desertTheme.GetSandChar();
    }
}