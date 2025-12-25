// Models/Map.cs - PATLAMA ALANI DÜZELTMESİ
using System;
using System.Collections.Generic;

using BombermanGame.src.Patterns.Structural.Adapter;

namespace BombermanGame.src.Models
{
    public class Map
    {
        public int Width { get; private set; }
        public int Height { get; private set; }
        public IWall[,] Tiles { get; private set; }
        public ITheme Theme { get; set; }
        private int _seed;

        public Map(int width, int height, ITheme theme)
            : this(width, height, theme, Environment.TickCount)
        {
        }

        public Map(int width, int height, ITheme theme, int seed)
        {
            Width = width;
            Height = height;
            Theme = theme;
            _seed = seed;
            Tiles = new IWall[height, width];
            InitializeMap();
        }

        private void InitializeMap()
        {
            Random random = new Random(_seed);

            for (int y = 0; y < Height; y++)
            {
                for (int x = 0; x < Width; x++)
                {
                    if (x == 0 || x == Width - 1 || y == 0 || y == Height - 1)
                    {
                        Tiles[y, x] = new UnbreakableWall();
                    }
                    else if (x % 2 == 0 && y % 2 == 0)
                    {
                        Tiles[y, x] = new UnbreakableWall();
                    }
                    else if ((x <= 2 && y <= 2) || (x >= Width - 3 && y >= Height - 3))
                    {
                        Tiles[y, x] = new EmptySpace();
                    }
                    else
                    {
                        int rand = random.Next(100);
                        if (rand < 40)
                        {
                            Tiles[y, x] = new BreakableWall();
                        }
                        else if (rand < 50)
                        {
                            Tiles[y, x] = new HardWall();
                        }
                        else
                        {
                            Tiles[y, x] = new EmptySpace();
                        }
                    }
                }
            }

            Console.WriteLine($"[MAP] ✅ Harita oluşturuldu (Seed: {_seed})");
        }

        public bool IsWalkable(int x, int y)
        {
            if (x < 0 || x >= Width || y < 0 || y >= Height)
                return false;

            var tile = Tiles[y, x];
            return tile is EmptySpace || tile.IsDestroyed();
        }

        public bool IsDestructible(int x, int y)
        {
            if (x < 0 || x >= Width || y < 0 || y >= Height)
                return false;

            return Tiles[y, x].IsDestructible();
        }

        public void DamageWall(int x, int y)
        {
            if (x >= 0 && x < Width && y >= 0 && y < Height)
            {
                Tiles[y, x].TakeDamage();
            }
        }

        public IWall GetTile(int x, int y)
        {
            if (x < 0 || x >= Width || y < 0 || y >= Height)
                return new UnbreakableWall();

            return Tiles[y, x];
        }

        // DÜZELTİLMİŞ: Patlama alanı hesaplama - duvarları dikkate alır
        public List<Position> GetExplosionArea(Position bombPosition, int power)
        {
            var explosionArea = new List<Position>();

            // Bombanın kendisi her zaman patlama alanında
            explosionArea.Add(new Position(bombPosition.X, bombPosition.Y));

            // 4 yön: Sol, Sağ, Yukarı, Aşağı
            int[] dx = { -1, 1, 0, 0 };
            int[] dy = { 0, 0, -1, 1 };

            for (int dir = 0; dir < 4; dir++)
            {
                // Her yönde power kadar ilerle
                for (int i = 1; i <= power; i++)
                {
                    int x = bombPosition.X + dx[dir] * i;
                    int y = bombPosition.Y + dy[dir] * i;

                    // Harita sınırları dışında mı?
                    if (x < 0 || x >= Width || y < 0 || y >= Height)
                        break;

                    var tile = Tiles[y, x];

                    // Bu pozisyonu patlama alanına ekle
                    explosionArea.Add(new Position(x, y));

                    // KIRILAMAZ DUVARA ÇARPTI MI?
                    // Kırılamaz duvarda DUR (patlama daha ileriye gidemez)
                    if (tile is UnbreakableWall)
                    {
                        // Kırılamaz duvardan sonrasını ekleme
                        break;
                    }

                    // KIRILAB İLİR DUVARA ÇARPTI MI?
                    // Kırılabilir duvar veya hard wall'a çarptıysa:
                    // - Bu duvarı patlama alanına ekle (yukarıda eklendi)
                    // - Ama bundan sonrasını EKLEME (patlama durur)
                    if (tile.IsDestructible() && !tile.IsDestroyed())
                    {
                        // Bu duvarı yık ama daha ileriye gitme
                        break;
                    }
                }
            }

            // DEBUG: Patlama alanını göster
            Console.SetCursorPosition(0, Console.WindowHeight - 5);
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.Write($"[EXPLOSION] Bomb at ({bombPosition.X},{bombPosition.Y}) Power:{power} Area:{explosionArea.Count} tiles    ");
            Console.ResetColor();

            return explosionArea;
        }

        public int GetSeed() => _seed;
    }
}