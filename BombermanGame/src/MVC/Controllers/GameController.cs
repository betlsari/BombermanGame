// MVC/Controllers/GameController.cs
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using BombermanGame.src.Patterns.Behavioral.Command;
using BombermanGame.src.Patterns.Repository;
using BombermanGame.src.Core;
using BombermanGame.src.Models;
using BombermanGame.src.Patterns.Behavioral.Observer;
using BombermanGame.src.Patterns.Creational.Factory;
using BombermanGame.src.Patterns.Structural.Decorator;
using BombermanGame.src.UI;
using BombermanGame.src.Audio;

namespace BombermanGame.src.MVC.Controllers
{
    public class GameController
    {
        private GameManager _gameManager;
        private GameRenderer _gameRenderer;
        private InputController _inputController;
        private ScoreObserver _scoreObserver;
        private StatsObserver _statsObserver;
        private UIObserver _uiObserver;
        private SoundObserver _soundObserver;
        private bool _isRunning;
        private ScoreRepository _scoreRepository;
        private StatsRepository _statsRepository;
        private bool _isSinglePlayer;
        private Dictionary<int, IPlayer> _decoratedPlayers;

        public GameController()
        {
            _gameManager = GameManager.Instance;
            _gameRenderer = new GameRenderer();
            _inputController = new InputController();
            _scoreRepository = new ScoreRepository();
            _statsRepository = new StatsRepository();
            _decoratedPlayers = new Dictionary<int, IPlayer>();

            _scoreObserver = new ScoreObserver();
            _statsObserver = new StatsObserver();
            _uiObserver = new UIObserver();
            _soundObserver = new SoundObserver();

            _gameManager.Attach(_scoreObserver);
            _gameManager.Attach(_statsObserver);
            _gameManager.Attach(_uiObserver);
            _gameManager.Attach(_soundObserver);
        }

        public void StartGame(string theme, bool isSinglePlayer = false)
        {
            _isRunning = true;
            _isSinglePlayer = isSinglePlayer;
            _gameManager.ResetGame();
            _decoratedPlayers.Clear();

            _scoreObserver.ResetScore();
            _statsObserver.Reset();

            var themeAdapter = Patterns.Structural.Adapter.ThemeFactory.GetTheme(theme);
            _gameManager.CurrentMap = new Map(21, 15, themeAdapter);

            var player1 = new Player(1, "Player 1", new Position(1, 1));
            _gameManager.Players.Add(player1);

            _decoratedPlayers[1] = player1;

            if (!isSinglePlayer)
            {
                var player2 = new Player(2, "Player 2", new Position(19, 13));
                _gameManager.Players.Add(player2);
                _decoratedPlayers[2] = player2;

                Console.Clear();
                Console.WriteLine("╔══════════════════════════════════════════════════════════════╗");
                Console.WriteLine("║                  TWO PLAYER MODE + ENEMIES                   ║");
                Console.WriteLine("╚══════════════════════════════════════════════════════════════╝\n");
                Console.WriteLine("Player 1:");
                Console.WriteLine("  WASD / Arrow Keys - Move");
                Console.WriteLine("  SPACE - Place Bomb\n");
                Console.WriteLine("Player 2:");
                Console.WriteLine("  I J K L - Move");
                Console.WriteLine("  ENTER - Place Bomb\n");
                Console.WriteLine("⚔️  WARNING: Enemies will spawn! Work together to survive!\n");
                Console.WriteLine("Press any key to start...");
                Console.ReadKey();
            }

            SpawnEnemies();
            _gameRenderer.RenderCountdown(3);
            GameLoop();
        }

        private void SpawnEnemies()
        {
            var staticFactory = EnemyFactoryProvider.GetFactory("static");
            var chaseFactory = EnemyFactoryProvider.GetFactory("chase");
            var smartFactory = EnemyFactoryProvider.GetFactory("smart");

            _gameManager.Enemies.Add(staticFactory.CreateEnemy(1, new Position(10, 7)));
            _gameManager.Enemies.Add(chaseFactory.CreateEnemy(2, new Position(15, 5)));
            _gameManager.Enemies.Add(smartFactory.CreateEnemy(3, new Position(5, 10)));
        }

        private void GameLoop()
        {
            int frameCount = 0;

            while (_isRunning)
            {
                Console.Clear();

                Console.Write($"Score: {_scoreObserver.GetScore()} | Enemies: {_gameManager.Enemies.Count(e => e.IsAlive)} | PowerUps: {_gameManager.PowerUps.Count(p => !p.IsCollected)} | ");

                foreach (var kvp in _decoratedPlayers)
                {
                    var player = _gameManager.Players.FirstOrDefault(p => p.Id == kvp.Key);
                    if (player != null)
                    {
                        var decoratedPlayer = kvp.Value;
                        string status = player.IsAlive ? "ALIVE" : "DEAD";
                        ConsoleColor color = player.IsAlive ? ConsoleColor.Green : ConsoleColor.Red;

                        Console.ForegroundColor = color;
                        Console.Write($"P{kvp.Key}({status})");
                        Console.ResetColor();
                        Console.Write($": Pos({player.Position.X},{player.Position.Y}) B:{decoratedPlayer.BombCount} Pwr:{decoratedPlayer.BombPower} Spd:{decoratedPlayer.Speed} | ");
                    }
                }
                Console.WriteLine();

                _gameRenderer.Render(_gameManager);

                if (Console.KeyAvailable)
                {
                    var key = Console.ReadKey(true);
                    HandleInput(key);
                }

                frameCount++;
                if (frameCount % 10 == 0)
                {
                    UpdateBombs();
                    UpdateEnemies();
                    CheckCollisions();
                }

                if (CheckGameOver())
                {
                    _isRunning = false;
                    EndGame();
                }

                Thread.Sleep(100);
            }
        }

        private void HandleInput(ConsoleKeyInfo key)
        {
            if (key.Key == ConsoleKey.Escape)
            {
                _isRunning = false;
                return;
            }

            if (key.Key == ConsoleKey.U)
            {
                _inputController.UndoLastCommand();
                return;
            }

            if (_gameManager.CurrentMap == null) return;

            var playerPositionsBefore = _gameManager.Players
                .Where(p => p.IsAlive)
                .ToDictionary(p => p.Id, p => new Position(p.Position.X, p.Position.Y));

            _inputController.ProcessMultiplayerInput(key, _gameManager.Players, _gameManager.CurrentMap);

            foreach (var player in _gameManager.Players.Where(p => p.IsAlive))
            {
                CheckPowerUpCollection(player);
            }
        }

        private void UpdateBombs()
        {
            var bombsToExplode = new List<Bomb>();

            foreach (var bomb in _gameManager.Bombs)
            {
                bomb.Update();
                if (bomb.ShouldExplode())
                {
                    bombsToExplode.Add(bomb);
                }
            }

            foreach (var bomb in bombsToExplode)
            {
                ExplodeBomb(bomb);
            }
        }

        private void ExplodeBomb(Bomb bomb)
        {
            if (_gameManager.CurrentMap == null) return;

            bomb.HasExploded = true;

            var owner = _gameManager.Players.FirstOrDefault(p => p.Id == bomb.OwnerId);
            int explosionPower = owner != null && _decoratedPlayers.ContainsKey(owner.Id)
                ? _decoratedPlayers[owner.Id].BombPower
                : bomb.Power;

            var explosionArea = _gameManager.CurrentMap.GetExplosionArea(bomb.Position, explosionPower);

            _gameManager.Notify(new GameEvent(EventType.BombExploded, bomb));

            foreach (var pos in explosionArea)
            {
                if (_gameManager.CurrentMap.IsDestructible(pos.X, pos.Y))
                {
                    _gameManager.CurrentMap.DamageWall(pos.X, pos.Y);
                    _gameManager.Notify(new GameEvent(EventType.WallDestroyed));

                    if (new Random().Next(100) < 100)
                    {
                        SpawnPowerUp(pos);
                    }
                }
            }

            foreach (var pos in explosionArea)
            {
                foreach (var player in _gameManager.Players)
                {
                    if (player.IsAlive && player.Position.X == pos.X && player.Position.Y == pos.Y)
                    {
                        Console.SetCursorPosition(0, Console.WindowHeight - 4);
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine($"[EXPLOSION] {player.Name} hit at ({pos.X},{pos.Y}) by bomb at ({bomb.Position.X},{bomb.Position.Y})!");
                        Console.ResetColor();
                        Thread.Sleep(500);

                        player.State?.TakeDamage(player);
                        _gameManager.Notify(new GameEvent(EventType.PlayerDied, player.Name));
                    }
                }

                foreach (var enemy in _gameManager.Enemies)
                {
                    if (enemy.IsAlive && enemy.Position.X == pos.X && enemy.Position.Y == pos.Y)
                    {
                        enemy.IsAlive = false;
                        _gameManager.Notify(new GameEvent(EventType.EnemyKilled));
                    }
                }
            }

            _gameManager.Bombs.Remove(bomb);
        }

        private void SpawnPowerUp(Position position)
        {
            bool alreadyExists = _gameManager.PowerUps.Any(p =>
                p.Position.X == position.X && p.Position.Y == position.Y && !p.IsCollected);

            if (alreadyExists)
            {
                Console.SetCursorPosition(0, Console.WindowHeight - 1);
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.Write($"⚠️  Power-up already exists at ({position.X},{position.Y})              ");
                Console.ResetColor();
                return;
            }

            var types = Enum.GetValues(typeof(PowerUpType));
            var randomType = (PowerUpType)types.GetValue(new Random().Next(types.Length))!;

            var powerUp = new PowerUp(new Position(position.X, position.Y), randomType);
            powerUp.IsCollected = false;

            _gameManager.PowerUps.Add(powerUp);

            Console.SetCursorPosition(0, Console.WindowHeight - 1);
            Console.ForegroundColor = ConsoleColor.Green;
            Console.Write($"✅ Power-up spawned at ({position.X},{position.Y}): {randomType} | Total: {_gameManager.PowerUps.Count}     ");
            Console.ResetColor();
        }

        private void CheckPowerUpCollection(Player player)
        {
            if (!player.IsAlive) return;

            for (int i = _gameManager.PowerUps.Count - 1; i >= 0; i--)
            {
                var powerUp = _gameManager.PowerUps[i];

                Console.SetCursorPosition(0, Console.WindowHeight - 3);
                Console.WriteLine($"[DEBUG] Player {player.Id} at ({player.Position.X},{player.Position.Y}) | PowerUp at ({powerUp.Position.X},{powerUp.Position.Y}) Collected:{powerUp.IsCollected}");

                bool notCollected = !powerUp.IsCollected;
                bool sameX = powerUp.Position.X == player.Position.X;
                bool sameY = powerUp.Position.Y == player.Position.Y;

                Console.SetCursorPosition(0, Console.WindowHeight - 2);
                Console.WriteLine($"[DEBUG] NotCollected:{notCollected} SameX:{sameX} SameY:{sameY}                    ");

                if (notCollected && sameX && sameY)
                {
                    powerUp.IsCollected = true;

                    ApplyPowerUpWithDecorator(player, powerUp);

                    _gameManager.Notify(new GameEvent(EventType.PowerUpCollected, powerUp.Type));

                    _gameManager.PowerUps.RemoveAt(i);

                    Console.SetCursorPosition(0, 0);
                    Console.ForegroundColor = ConsoleColor.Magenta;
                    Console.WriteLine($"                                                                    ");
                    Console.SetCursorPosition(0, 0);
                    Console.WriteLine($"💎💎💎 {player.Name} collected {powerUp.Type}! New: {_decoratedPlayers[player.Id].GetStats()} 💎💎💎");
                    Console.ResetColor();

                    Thread.Sleep(1500);
                    break;
                }
            }
        }

        private void ApplyPowerUpWithDecorator(Player player, PowerUp powerUp)
        {
            IPlayer currentPlayer = _decoratedPlayers[player.Id];

            switch (powerUp.Type)
            {
                case PowerUpType.BombCount:
                    _decoratedPlayers[player.Id] = new BombCountDecorator(currentPlayer, 1);
                    player.IncreaseBombCount();
                    Console.WriteLine($"[DECORATOR] Applied BombCountDecorator to {player.Name}");
                    break;

                case PowerUpType.BombPower:
                    _decoratedPlayers[player.Id] = new BombPowerDecorator(currentPlayer, 1);
                    player.IncreaseBombPower();
                    Console.WriteLine($"[DECORATOR] Applied BombPowerDecorator to {player.Name}");
                    break;

                case PowerUpType.SpeedBoost:
                    _decoratedPlayers[player.Id] = new SpeedBoostDecorator(currentPlayer, 1);
                    player.IncreaseSpeed();
                    Console.WriteLine($"[DECORATOR] Applied SpeedBoostDecorator to {player.Name}");
                    break;
            }
        }

        private void UpdateEnemies()
        {
            var alivePlayer = _gameManager.Players
                .Where(p => p.IsAlive)
                .OrderBy(p => _gameManager.Enemies.Any(e => e.IsAlive)
                    ? _gameManager.Enemies.First(e => e.IsAlive).Position.DistanceTo(p.Position)
                    : 0)
                .FirstOrDefault();

            if (alivePlayer == null || _gameManager.CurrentMap == null) return;

            foreach (var enemy in _gameManager.Enemies.Where(e => e.IsAlive))
            {
                enemy.Move(_gameManager.CurrentMap, alivePlayer.Position);
            }
        }

        private void CheckCollisions()
        {
            foreach (var player in _gameManager.Players.Where(p => p.IsAlive))
            {
                foreach (var enemy in _gameManager.Enemies.Where(e => e.IsAlive))
                {
                    if (player.Position.X == enemy.Position.X && player.Position.Y == enemy.Position.Y)
                    {
                        player.State?.TakeDamage(player);
                        _gameManager.Notify(new GameEvent(EventType.PlayerDied, player.Name));
                    }
                }
            }
        }

        private bool CheckGameOver()
        {
            int alivePlayers = _gameManager.Players.Count(p => p.IsAlive);
            int aliveEnemies = _gameManager.Enemies.Count(e => e.IsAlive);

            if (_isSinglePlayer)
            {
                return alivePlayers == 0 || aliveEnemies == 0;
            }
            else
            {
                if (aliveEnemies == 0) return true;
                return alivePlayers == 0;
            }
        }

        private void EndGame()
        {
            int aliveEnemies = _gameManager.Enemies.Count(e => e.IsAlive);
            var alivePlayers = _gameManager.Players.Where(p => p.IsAlive).ToList();

            string result;
            bool isVictory = aliveEnemies == 0 && alivePlayers.Count > 0;

            if (isVictory)
            {
                if (_isSinglePlayer)
                {
                    result = $"{alivePlayers[0].Name} WINS!";
                }
                else
                {
                    result = alivePlayers.Count == 2
                        ? "BOTH PLAYERS WIN!"
                        : $"{alivePlayers[0].Name} SURVIVES!";
                }
            }
            else
            {
                result = "GAME OVER - ENEMIES WIN!";
            }

            _gameManager.Notify(new GameEvent(EventType.GameEnded, result));

            if (_gameManager.CurrentUserId > 0)
            {
                try
                {
                    var score = new Models.Entities.HighScore
                    {
                        UserId = _gameManager.CurrentUserId,
                        Score = _scoreObserver.GetScore(),
                        GameDate = DateTime.Now
                    };
                    _scoreRepository.Add(score);

                    if (isVictory)
                    {
                        _statsRepository.IncrementWins(_gameManager.CurrentUserId);
                    }
                    else
                    {
                        _statsRepository.IncrementLosses(_gameManager.CurrentUserId);
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"[ERROR] Database save failed: {ex.Message}");
                }
            }

            Console.Clear();
            Console.WriteLine("\n\n");

            if (isVictory)
            {
                ConsoleUI.WriteLineColored($"🏆 {result} 🏆", ConsoleColor.Green);
            }
            else
            {
                ConsoleUI.WriteLineColored($"💀 {result} 💀", ConsoleColor.Red);
            }

            Console.WriteLine($"\n🎯 Final Score: {_scoreObserver.GetScore()}");
            Console.WriteLine($"📊 Walls Destroyed: {_statsObserver.GetWallsDestroyed()}");
            Console.WriteLine($"👾 Enemies Killed: {_statsObserver.GetEnemiesKilled()}");
            Console.WriteLine($"⭐ Power-ups Collected: {_statsObserver.GetPowerUpsCollected()}");
            Console.WriteLine($"\n🎮 Players Survived: {alivePlayers.Count}/{_gameManager.Players.Count}");

            Console.WriteLine("\n📈 Final Player Stats (with decorators):");
            foreach (var kvp in _decoratedPlayers)
            {
                var player = _gameManager.Players.FirstOrDefault(p => p.Id == kvp.Key);
                if (player != null)
                {
                    Console.WriteLine($"  {player.Name}: {kvp.Value.GetStats()}");
                }
            }

            Console.WriteLine("\nPress any key to return to menu...");
            Console.ReadKey();
        }
    }
}