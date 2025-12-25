
using System;
using System.Threading;
using BombermanGame.src.Core;
using BombermanGame.src.Database;
using BombermanGame.src.Audio;

namespace BombermanGame.src
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.Title = "Bomberman Multiplayer";
            Console.CursorVisible = false;

            // Ses sistemini test et
            try
            {
                var soundManager = SoundManager.Instance;
                Console.WriteLine("[SYSTEM] ✅ Sound system initialized successfully");

                // Hoşgeldin sesi
                soundManager.PlaySound(SoundType.MenuSelect);
                Thread.Sleep(500);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[WARNING] ⚠️  Sound system failed to initialize: {ex.Message}");
                Console.WriteLine("[INFO] Game will continue without sound");
                Thread.Sleep(2000);
            }

            // Database initialize
            try
            {
                DatabaseManager.Instance.Initialize();
                Console.WriteLine("[SYSTEM] ✅ Database initialized successfully");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[ERROR] ❌ Database initialization failed: {ex.Message}");
                Console.WriteLine("Press any key to exit...");
                Console.ReadKey();
                return;
            }

            // Game Manager başlat
            GameManager gameManager = GameManager.Instance;

            // Ana menü göster
            MainMenu menu = new MainMenu();
            menu.Show();
        }
    }
}