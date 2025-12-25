
using System;

namespace BombermanGame.src.Patterns.Structural.Adapter
{
    public static class ThemeFactory
    {
        public static ITheme GetTheme(string themeName)
        {
            return themeName.ToLower() switch
            {
                "desert" => new DesertThemeAdapter(),
                "forest" => new ForestThemeAdapter(),
                "city" => new CityThemeAdapter(),
                _ => new DesertThemeAdapter()
            };
        }
    }
}