using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

// Audio/SoundType.cs
namespace BombermanGame.src.Audio
{
    /// <summary>
    /// Oyunda kullanılan ses efektleri türleri
    /// </summary>
    public enum SoundType
    {
        BombPlace,
        BombExplode,
        PowerUpCollect,
        PlayerDeath,
        EnemyDeath,
        WallBreak,
        MenuSelect,
        GameOver,
        Victory,
        MenuMove
    }
}
