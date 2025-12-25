
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BombermanGame.src.Audio
{
    
    public sealed class SoundManager
    {
        private static SoundManager? _instance;
        private static readonly object _lock = new object();

        private bool _soundEnabled = true;
        private Dictionary<SoundType, string> _soundPaths;

       
        private SoundManager()
        {
            InitializeSoundPaths();
        }

        
        public static SoundManager Instance
        {
            get
            {
                if (_instance == null)
                {
                    lock (_lock)
                    {
                        if (_instance == null)
                        {
                            _instance = new SoundManager();
                        }
                    }
                }
                return _instance;
            }
        }

        
        private void InitializeSoundPaths()
        {
            _soundPaths = new Dictionary<SoundType, string>
            {
                { SoundType.BombPlace, "assets/sounds/bomb_place.wav" },
                { SoundType.BombExplode, "assets/sounds/bomb_explode.wav" },
                { SoundType.PowerUpCollect, "assets/sounds/powerup_collect.wav" },
                { SoundType.PlayerDeath, "assets/sounds/player_death.wav" },
                { SoundType.EnemyDeath, "assets/sounds/enemy_death.wav" },
                { SoundType.WallBreak, "assets/sounds/wall_break.wav" },
                { SoundType.MenuSelect, "assets/sounds/menu_select.wav" },
                { SoundType.GameOver, "assets/sounds/game_over.wav" },
                { SoundType.Victory, "assets/sounds/victory.wav" },
                { SoundType.MenuMove, "assets/sounds/menu_move.wav" }
            };
        }

        
        public void PlaySound(SoundType soundType)
        {
            if (!_soundEnabled) return;

           

            PlaySoundWithBeep(soundType);

            
        }

        
        public async Task PlaySoundAsync(SoundType soundType)
        {
            if (!_soundEnabled) return;

            await Task.Run(() => PlaySound(soundType));
        }

        
        private void PlaySoundWithBeep(SoundType soundType)
        {
            try
            {
               
                switch (soundType)
                {
                    case SoundType.BombPlace:
                        Console.Beep(400, 100); 
                        break;

                    case SoundType.BombExplode:
                        
                        Console.Beep(200, 50);
                        Console.Beep(150, 100);
                        Console.Beep(100, 150);
                        break;

                    case SoundType.PowerUpCollect:
                        
                        Console.Beep(440, 80);
                        Console.Beep(554, 80);
                        Console.Beep(659, 80);
                        break;

                    case SoundType.PlayerDeath:
                        
                        Console.Beep(800, 100);
                        Console.Beep(600, 100);
                        Console.Beep(400, 150);
                        break;

                    case SoundType.EnemyDeath:
                        Console.Beep(300, 100);
                        Console.Beep(250, 100);
                        break;

                    case SoundType.WallBreak:
                        Console.Beep(350, 120);
                        break;

                    case SoundType.MenuSelect:
                        Console.Beep(600, 80);
                        break;

                    case SoundType.MenuMove:
                        Console.Beep(500, 50);
                        break;

                    case SoundType.GameOver:
                        
                        Console.Beep(440, 200);
                        Console.Beep(392, 200);
                        Console.Beep(349, 400);
                        break;

                    case SoundType.Victory:
                        
                        Console.Beep(523, 100);
                        Console.Beep(587, 100); 
                        Console.Beep(659, 100); 
                        Console.Beep(784, 300); 
                        break;
                }
            }
            catch (Exception)
            {
                // Beep çalmazsa sessizce devam et
            }
        }

        
        public void SetSoundEnabled(bool enabled)
        {
            _soundEnabled = enabled;
            Console.WriteLine($"[SOUND] Ses {(enabled ? "açıldı" : "kapatıldı")}");
        }

       
        public bool IsSoundEnabled() => _soundEnabled;

        
        public void ToggleSound()
        {
            SetSoundEnabled(!_soundEnabled);
        }
    }
}