
using System;

namespace BombermanGame.src.Patterns.Structural.Adapter
{
    public interface ITheme
    {
        string GetName();
        ConsoleColor GetUnbreakableWallColor();
        ConsoleColor GetBreakableWallColor();
        ConsoleColor GetHardWallColor();
        ConsoleColor GetGroundColor();
        char GetUnbreakableWallChar();
        char GetBreakableWallChar();
        char GetHardWallChar();
    }
}