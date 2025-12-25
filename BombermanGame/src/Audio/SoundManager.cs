// Audio/SoundManager.cs - SINGLETON PATTERN
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BombermanGame.src.Audio
{
    /// <summary>
    /// Ses yönetimi için Singleton pattern
    /// 3 farklı implementasyon seçeneği sunulmuştur
    /// </summary>
    public sealed class SoundManager
    {
        private static SoundManager? _instance;
        private static readonly object _lock = new object();

        private bool _soundEnabled = true;
        private Dictionary<SoundType, string> _soundPaths;

        // Private constructor - Singleton
        private SoundManager()
        {
            InitializeSoundPaths();
        }

        // Thread-safe Singleton instance
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

        /// <summary>
        /// Ses dosya yollarını initialize et
        /// </summary>
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

        /// <summary>
        /// Ses efekti çal
        /// </summary>
        public void PlaySound(SoundType soundType)
        {
            if (!_soundEnabled) return;

            // Seçtiğiniz implementasyonu kullanın:

            // SEÇENEK 1: Console.Beep (En basit - Kurulum gerektirmez)
            PlaySoundWithBeep(soundType);

            // SEÇENEK 2: NAudio (WAV dosyaları - NAudio paketi gerekli)
            // PlaySoundWithNAudio(soundType);

            // SEÇENEK 3: System.Media.SoundPlayer (Windows only - Built-in)
            // PlaySoundWithSoundPlayer(soundType);
        }

        /// <summary>
        /// Asenkron ses çal
        /// </summary>
        public async Task PlaySoundAsync(SoundType soundType)
        {
            if (!_soundEnabled) return;

            await Task.Run(() => PlaySound(soundType));
        }

        // ========================================
        // SEÇENEK 1: Console.Beep (Önerilen - En Kolay)
        // ========================================
        private void PlaySoundWithBeep(SoundType soundType)
        {
            try
            {
                // Her ses tipi için farklı frekans ve süre
                switch (soundType)
                {
                    case SoundType.BombPlace:
                        Console.Beep(400, 100); // Düşük ses, kısa
                        break;

                    case SoundType.BombExplode:
                        // Patlama efekti (3 ardışık beep)
                        Console.Beep(200, 50);
                        Console.Beep(150, 100);
                        Console.Beep(100, 150);
                        break;

                    case SoundType.PowerUpCollect:
                        // Yükselen ses
                        Console.Beep(440, 80);
                        Console.Beep(554, 80);
                        Console.Beep(659, 80);
                        break;

                    case SoundType.PlayerDeath:
                        // Düşen ses
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
                        // Üzücü melodi
                        Console.Beep(440, 200);
                        Console.Beep(392, 200);
                        Console.Beep(349, 400);
                        break;

                    case SoundType.Victory:
                        // Zafer melodisi
                        Console.Beep(523, 100); // C
                        Console.Beep(587, 100); // D
                        Console.Beep(659, 100); // E
                        Console.Beep(784, 300); // G
                        break;
                }
            }
            catch (Exception)
            {
                // Beep çalmazsa sessizce devam et
            }
        }

        // ========================================
        // SEÇENEK 2: NAudio (WAV dosyaları için)
        // NuGet: dotnet add package NAudio
        // ========================================
        /*
        private void PlaySoundWithNAudio(SoundType soundType)
        {
            try
            {
                if (!_soundPaths.ContainsKey(soundType)) return;
                
                string soundPath = _soundPaths[soundType];
                
                if (!System.IO.File.Exists(soundPath))
                {
                    Console.WriteLine($"[SOUND] Ses dosyası bulunamadı: {soundPath}");
                    return;
                }

                // NAudio ile ses çal
                using (var audioFile = new NAudio.Wave.AudioFileReader(soundPath))
                using (var outputDevice = new NAudio.Wave.WaveOutEvent())
                {
                    outputDevice.Init(audioFile);
                    outputDevice.Play();
                    
                    // Ses bitene kadar bekle
                    while (outputDevice.PlaybackState == NAudio.Wave.PlaybackState.Playing)
                    {
                        System.Threading.Thread.Sleep(10);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[SOUND] Hata: {ex.Message}");
            }
        }
        */

        // ========================================
        // SEÇENEK 3: System.Media.SoundPlayer (Windows Only)
        // Built-in - Ekstra paket gerektirmez
        // ========================================
        /*
        private void PlaySoundWithSoundPlayer(SoundType soundType)
        {
            try
            {
                if (!_soundPaths.ContainsKey(soundType)) return;
                
                string soundPath = _soundPaths[soundType];
                
                if (!System.IO.File.Exists(soundPath))
                {
                    Console.WriteLine($"[SOUND] Ses dosyası bulunamadı: {soundPath}");
                    return;
                }

                using (var player = new System.Media.SoundPlayer(soundPath))
                {
                    player.Play(); // Asenkron çal
                    // VEYA
                    // player.PlaySync(); // Senkron çal (ses bitene kadar bekle)
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[SOUND] Hata: {ex.Message}");
            }
        }
        */

        /// <summary>
        /// Sesi aç/kapa
        /// </summary>
        public void SetSoundEnabled(bool enabled)
        {
            _soundEnabled = enabled;
            Console.WriteLine($"[SOUND] Ses {(enabled ? "açıldı" : "kapatıldı")}");
        }

        /// <summary>
        /// Ses durumunu al
        /// </summary>
        public bool IsSoundEnabled() => _soundEnabled;

        /// <summary>
        /// Sesi toggle et
        /// </summary>
        public void ToggleSound()
        {
            SetSoundEnabled(!_soundEnabled);
        }
    }
}