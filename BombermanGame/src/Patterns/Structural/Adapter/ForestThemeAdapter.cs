
using System;

namespace BombermanGame.src.Patterns.Structural.Adapter
{
    public class ForestThemeAdapter : ITheme
    {
        private readonly ForestTheme _forestTheme;

        public ForestThemeAdapter()
        {
            _forestTheme = new ForestTheme();
        }

        public string GetName() => _forestTheme.Name;
        public ConsoleColor GetUnbreakableWallColor() => _forestTheme.TreeColor;
        public ConsoleColor GetBreakableWallColor() => _forestTheme.LogColor;
        public ConsoleColor GetHardWallColor() => ConsoleColor.DarkGreen;
        public ConsoleColor GetGroundColor() => _forestTheme.GrassColor;
        public char GetUnbreakableWallChar() => _forestTheme.GetTreeChar();
        public char GetBreakableWallChar() => _forestTheme.GetLogChar();
        public char GetHardWallChar() => _forestTheme.GetGrassChar();
    }
}
