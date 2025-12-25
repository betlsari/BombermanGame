// MVC/Controllers/OnlineGameController.cs
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using BombermanGame.src.Core;
using BombermanGame.src.Models;
using BombermanGame.src.Models.Entities;
using BombermanGame.src.Network;
using BombermanGame.src.Patterns.Behavioral.Command;
using BombermanGame.src.Patterns.Behavioral.Observer;
using BombermanGame.src.Patterns.Creational.Factory;
using BombermanGame.src.Patterns.Repository;
using BombermanGame.src.Patterns.Structural.Adapter;
using BombermanGame.src.UI;
using BombermanGame.src.Audio;

namespace BombermanGame.src.MVC.Controllers
{
    public class OnlineGameController
    {
        private readonly SignalRClient _signalRClient;
        private readonly SignalREventHandler _eventHandler;
        private readonly GameManager _gameManager;
        private readonly GameRenderer _gameRenderer;
        private readonly InputController _inputController;
        private readonly ScoreRepository _scoreRepository;
        private readonly StatsRepository _statsRepository;
        private readonly CommandInvoker _commandInvoker;
        private readonly ScoreObserver _scoreObserver;
        private readonly StatsObserver _statsObserver;
        private readonly UIObserver _uiObserver;
        private readonly SoundObserver _soundObserver;

        private bool _isRunning;
        private bool _isHost;
        private string _currentRoomId = "";
        private int _localPlayerId;
        private CancellationTokenSource? _gameLoopCancellation;

        public OnlineGameController(SignalRClient signalRClient)
        {
            _signalRClient = signalRClient ?? throw new ArgumentNullException(nameof(signalRClient));

            _gameManager = GameManager.Instance;
            _gameRenderer = new GameRenderer();
            _inputController = new InputController();
            _scoreRepository = new ScoreRepository();
            _statsRepository = new StatsRepository();
            _commandInvoker = new CommandInvoker();

            _scoreObserver = new ScoreObserver();
            _statsObserver = new StatsObserver();
            _uiObserver = new UIObserver();
            _soundObserver = new SoundObserver();

            _gameManager.Attach(_scoreObserver);
            _gameManager.Attach(_statsObserver);
            _gameManager.Attach(_uiObserver);
            _gameManager.Attach(_soundObserver);

            _eventHandler = new SignalREventHandler(_signalRClient);
            RegisterEventHandlers();
        }

        private void RegisterEventHandlers()
        {
            _eventHandler.OnGameReady += HandleGameReady;
            _eventHandler.OnGameStateChanged += HandleGameStateChanged;
            _eventHandler.OnPlayerStatusChanged += HandlePlayerStatusChanged;
            _eventHandler.OnErrorOccurred += HandleError;
        }

        public async Task<bool> CreateRoomAsync(string roomName, string playerName, string theme, int maxPlayers)
        {
            try
            {
                Console.Clear();
                Console.WriteLine("╔══════════════════════════════════════════════════════════════╗");
                Console.WriteLine("║                    CREATING ROOM                             ║");
                Console.WriteLine("╚══════════════════════════════════════════════════════════════╝\n");

                Console.WriteLine($"Room Name: {roomName}");
                Console.WriteLine($"Theme: {theme}");
                Console.WriteLine($"Max Players: {maxPlayers}");
                Console.WriteLine("\n🔄 Creating room...");

                var response = await _signalRClient.CreateRoomAsync(roomName, playerName, theme, maxPlayers);

                if (response == null)
                {
                    Console.WriteLine("\n❌ Failed to create room - no response from server");
                    Thread.Sleep(2000);
                    return false;
                }

                var responseElement = (System.Text.Json.JsonElement)response;
                bool success = responseElement.GetProperty("Success").GetBoolean();

                if (!success)
                {
                    string errorMsg = responseElement.GetProperty("ErrorMessage").GetString() ?? "Unknown error";
                    Console.WriteLine($"\n❌ Failed to create room: {errorMsg}");
                    Thread.Sleep(2000);
                    return false;
                }

                _currentRoomId = responseElement.GetProperty("RoomId").GetString() ?? "";
                _isHost = true;
                _localPlayerId = 1;

                Console.WriteLine($"\n✅ Room created successfully!");
                Console.WriteLine($"Room ID: {_currentRoomId.Substring(0, 8)}...");
                Console.WriteLine($"\n⏳ Waiting for players to join...");

                await ShowWaitingRoom();
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"\n❌ Error creating room: {ex.Message}");
                Thread.Sleep(2000);
                return false;
            }
        }

        public async Task<bool> JoinRoomAsync(string roomId, string playerName)
        {
            try
            {
                Console.Clear();
                Console.WriteLine("╔══════════════════════════════════════════════════════════════╗");
                Console.WriteLine("║                     JOINING ROOM                             ║");
                Console.WriteLine("╚══════════════════════════════════════════════════════════════╝\n");

                Console.WriteLine($"Room ID: {roomId.Substring(0, 8)}...");
                Console.WriteLine($"Player Name: {playerName}");
                Console.WriteLine("\n🔄 Joining room...");

                bool success = await _signalRClient.JoinRoomAsync(roomId, playerName);

                if (!success)
                {
                    Console.WriteLine("\n❌ Failed to join room");
                    Thread.Sleep(2000);
                    return false;
                }

                _currentRoomId = roomId;
                _isHost = false;
                _localPlayerId = 2;

                Console.WriteLine("\n✅ Joined room successfully!");
                Console.WriteLine("\n⏳ Waiting for host to start game...");

                await ShowWaitingRoom();
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"\n❌ Error joining room: {ex.Message}");
                Thread.Sleep(2000);
                return false;
            }
        }

        private async Task ShowWaitingRoom()
        {
            var waitCancellation = new CancellationTokenSource();
            int frame = 0;
            string[] spinner = { "⠋", "⠙", "⠹", "⠸", "⠼", "⠴", "⠦", "⠧", "⠇", "⠏" };

            while (!_eventHandler.IsGameActive && !waitCancellation.Token.IsCancellationRequested)
            {
                Console.SetCursorPosition(0, 10);
                Console.Write($"{spinner[frame % spinner.Length]} Waiting");

                if (_isHost)
                {
                    Console.WriteLine("\n\nPress SPACE to start game (requires 2+ players)");
                    Console.WriteLine("Press ESC to cancel");

                    if (Console.KeyAvailable)
                    {
                        var key = Console.ReadKey(true);
                        if (key.Key == ConsoleKey.Spacebar)
                        {
                            await StartGameAsync();
                            break;
                        }
                        else if (key.Key == ConsoleKey.Escape)
                        {
                            await _signalRClient.LeaveRoomAsync(_currentRoomId);
                            return;
                        }
                    }
                }
                else
                {
                    Console.WriteLine("\n\nWaiting for host to start...");
                    Console.WriteLine("Press ESC to leave");

                    if (Console.KeyAvailable)
                    {
                        var key = Console.ReadKey(true);
                        if (key.Key == ConsoleKey.Escape)
                        {
                            await _signalRClient.LeaveRoomAsync(_currentRoomId);
                            return;
                        }
                    }
                }

                frame++;
                await Task.Delay(200);
            }
        }

        public async Task StartGameAsync()
        {
            if (!_isHost)
            {
                Console.WriteLine("⚠️  Only host can start the game");
                return;
            }

            try
            {
                Console.WriteLine("\n🎮 Starting game...");
                await _signalRClient.StartGameAsync(_currentRoomId);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ Error starting game: {ex.Message}");
            }
        }

        public void StartGame(User user, string roomId, bool isHost)
        {
            _currentRoomId = roomId;
            _isHost = isHost;
            _localPlayerId = isHost ? 1 : 2;

            Console.WriteLine($"\n[SYSTEM] Game Controller Initialized.");
            Console.WriteLine($"Room: {roomId} | Role: {(_isHost ? "HOST" : "CLIENT")}");

            if (_gameManager != null)
            {
                _gameManager.CurrentUserId = user.Id;

                ITheme theme = ThemeFactory.GetTheme("Desert");

                _gameManager.CurrentMap = new Models.Map(21, 11, theme);

                _gameManager.Enemies.Clear();
                _gameManager.PowerUps.Clear();
                _gameManager.Bombs.Clear();
                _gameManager.Players.Clear();

                var player1 = new Models.Player(1, "Player 1", new Models.Position(1, 1));
                _gameManager.Players.Add(player1);

                var player2 = new Models.Player(2, "Player 2", new Models.Position(19, 9));
                _gameManager.Players.Add(player2);
            }

            HandleGameReady();
        }

        private void HandleGameReady()
        {
            Console.WriteLine("\n✅ Game is ready! Starting in 3 seconds...");
            Thread.Sleep(3000);

            _isRunning = true;
            _gameLoopCancellation = new CancellationTokenSource();

            Task.Run(() => GameLoopAsync(_gameLoopCancellation.Token));
        }

        private async Task GameLoopAsync(CancellationToken cancellationToken)
        {
            int frameCount = 0;
            int syncInterval = _isHost ? 5 : 100;

            _gameRenderer.RenderCountdown(3);

            while (_isRunning && !cancellationToken.IsCancellationRequested)
            {
                try
                {
                    Console.Clear();

                    var latency = await _signalRClient.MeasureLatencyAsync();
                    _gameRenderer.RenderConnectionStatus(
                        _signalRClient.IsConnected,
                        _signalRClient.ConnectionId,
                        latency
                    );

                    _gameRenderer.Render(_gameManager);
                    _gameRenderer.RenderOnlinePlayerIndicator(_gameManager, _signalRClient.ConnectionId);

                    if (Console.KeyAvailable)
                    {
                        var key = Console.ReadKey(true);
                        await HandleInputAsync(key);
                    }

                    frameCount++;

                    if (_isHost && frameCount % syncInterval == 0)
                    {
                        await SyncGameStateAsync();
                        UpdateGameLogic();
                    }

                    if (CheckGameOver())
                    {
                        await EndGameAsync();
                        break;
                    }

                    await Task.Delay(100, cancellationToken);
                }
                catch (OperationCanceledException)
                {
                    break;
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"[GAME LOOP] Error: {ex.Message}");
                }
            }
        }

        private async Task HandleInputAsync(ConsoleKeyInfo key)
        {
            if (key.Key == ConsoleKey.Escape)
            {
                _isRunning = false;
                return;
            }

            var localPlayer = _gameManager.Players.FirstOrDefault(p => p.Id == _localPlayerId);
            if (localPlayer == null || !localPlayer.IsAlive || _gameManager.CurrentMap == null)
                return;

            var oldX = localPlayer.Position.X;
            var oldY = localPlayer.Position.Y;

            var command = _inputController.ProcessInput(key, localPlayer, _gameManager.CurrentMap);
            if (command != null)
            {
                _commandInvoker.ExecuteCommand(command);

                if (localPlayer.Position.X != oldX || localPlayer.Position.Y != oldY)
                {
                    await _signalRClient.SendMoveAsync(
                        _currentRoomId,
                        localPlayer.Position.X,
                        localPlayer.Position.Y
                    );
                }

                if (command is PlaceBombCommand)
                {
                    var bomb = _gameManager.Bombs.LastOrDefault(b => b.OwnerId == localPlayer.Id);
                    if (bomb != null)
                    {
                        await _signalRClient.SendPlaceBombAsync(
                            _currentRoomId,
                            bomb.Position.X,
                            bomb.Position.Y,
                            bomb.Power
                        );
                    }
                }

                CheckPowerUpCollection(localPlayer);
            }
        }

        private async Task SyncGameStateAsync()
        {
            if (!_isHost || !_signalRClient.IsConnected)
                return;

            try
            {
                var gameState = CreateGameStateDTO();
                await _signalRClient.SendGameStateAsync(_currentRoomId, gameState);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[SYNC] Error: {ex.Message}");
            }
        }

        private object CreateGameStateDTO()
        {
            return new
            {
                Players = _gameManager.Players.Select(p => new
                {
                    ConnectionId = p.Id.ToString(),
                    Name = p.Name,
                    PlayerId = p.Id,
                    X = p.Position.X,
                    Y = p.Position.Y,
                    IsAlive = p.IsAlive,
                    Health = p.Health,
                    Score = p.Score,
                    BombCount = p.BombCount,
                    BombPower = p.BombPower,
                    Speed = p.Speed
                }).ToList(),
                Bombs = _gameManager.Bombs.Where(b => !b.HasExploded).Select(b => new
                {
                    Id = Guid.NewGuid().ToString(),
                    OwnerId = b.OwnerId.ToString(),
                    X = b.Position.X,
                    Y = b.Position.Y,
                    Timer = b.Timer,
                    Range = b.Power,
                    HasExploded = b.HasExploded
                }).ToList(),
                Enemies = _gameManager.Enemies.Where(e => e.IsAlive).Select(e => new
                {
                    Id = e.Id.ToString(),
                    Type = e.Type.ToString() + "Enemy",
                    X = e.Position.X,
                    Y = e.Position.Y,
                    IsAlive = e.IsAlive,
                    Health = e.Health
                }).ToList(),
                PowerUps = _gameManager.PowerUps.Where(p => !p.IsCollected).Select(p => new
                {
                    Id = Guid.NewGuid().ToString(),
                    Type = p.Type.ToString(),
                    X = p.Position.X,
                    Y = p.Position.Y,
                    IsCollected = p.IsCollected
                }).ToList(),
                GameTick = Environment.TickCount
            };
        }

        private void UpdateGameLogic()
        {
            UpdateBombs();
            UpdateEnemies();
            CheckCollisions();
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
            var explosionArea = _gameManager.CurrentMap.GetExplosionArea(bomb.Position, bomb.Power);

            _gameManager.Notify(new GameEvent(EventType.BombExploded, bomb));

            foreach (var pos in explosionArea)
            {
                if (_gameManager.CurrentMap.IsDestructible(pos.X, pos.Y))
                {
                    _gameManager.CurrentMap.DamageWall(pos.X, pos.Y);
                    _gameManager.Notify(new GameEvent(EventType.WallDestroyed));

                    if (new Random().Next(100) < 30)
                    {
                        SpawnPowerUp(pos);
                    }
                }

                foreach (var player in _gameManager.Players)
                {
                    if (player.IsAlive && player.Position.X == pos.X && player.Position.Y == pos.Y)
                    {
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
            var types = Enum.GetValues(typeof(PowerUpType));
            var randomType = (PowerUpType)types.GetValue(new Random().Next(types.Length))!;
            _gameManager.PowerUps.Add(new PowerUp(position, randomType));
        }

        private void UpdateEnemies()
        {
            var alivePlayer = _gameManager.Players.FirstOrDefault(p => p.IsAlive);
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

        private void CheckPowerUpCollection(Player player)
        {
            var powerUp = _gameManager.PowerUps.FirstOrDefault(p =>
                !p.IsCollected && p.Position.X == player.Position.X && p.Position.Y == player.Position.Y);

            if (powerUp != null)
            {
                powerUp.IsCollected = true;
                ApplyPowerUp(player, powerUp);
                _gameManager.Notify(new GameEvent(EventType.PowerUpCollected, powerUp.Type));
                _gameManager.PowerUps.Remove(powerUp);
            }
        }

        private void ApplyPowerUp(Player player, PowerUp powerUp)
        {
            switch (powerUp.Type)
            {
                case PowerUpType.BombCount:
                    player.IncreaseBombCount();
                    break;
                case PowerUpType.BombPower:
                    player.IncreaseBombPower();
                    break;
                case PowerUpType.SpeedBoost:
                    player.IncreaseSpeed();
                    break;
            }
        }

        private bool CheckGameOver()
        {
            int alivePlayers = _gameManager.Players.Count(p => p.IsAlive);
            return alivePlayers <= 1;
        }

        private async Task EndGameAsync()
        {
            _isRunning = false;
            _gameLoopCancellation?.Cancel();

            var winner = _gameManager.Players.FirstOrDefault(p => p.IsAlive);
            string winnerId = winner?.Id.ToString() ?? "0";
            int finalScore = _scoreObserver.GetScore();

            if (_isHost)
            {
                await _signalRClient.NotifyGameEndAsync(_currentRoomId, winnerId, finalScore);
            }

            Console.Clear();
            if (winner?.Id == _localPlayerId)
            {
                ConsoleUI.WriteLineColored("\n🏆 YOU WIN! 🏆\n", ConsoleColor.Green);
            }
            else if (winner != null)
            {
                ConsoleUI.WriteLineColored("\n💀 YOU LOSE 💀\n", ConsoleColor.Red);
            }
            else
            {
                ConsoleUI.WriteLineColored("\n🤝 DRAW 🤝\n", ConsoleColor.Yellow);
            }

            Console.WriteLine($"Final Score: {finalScore}");
            Console.WriteLine($"Walls Destroyed: {_statsObserver.GetWallsDestroyed()}");
            Console.WriteLine($"Enemies Killed: {_statsObserver.GetEnemiesKilled()}");
            Console.WriteLine($"Power-ups Collected: {_statsObserver.GetPowerUpsCollected()}");

            if (_gameManager.CurrentUserId > 0)
            {
                SaveGameStatistics(winner?.Id == _localPlayerId);
            }

            Console.WriteLine("\nPress any key to return to lobby...");
            Console.ReadKey();

            await _signalRClient.LeaveRoomAsync(_currentRoomId);
        }

        private void SaveGameStatistics(bool isVictory)
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
                Console.WriteLine($"[ERROR] Failed to save statistics: {ex.Message}");
            }
        }

        private void HandleDisconnection()
        {
            _isRunning = false;
            _gameLoopCancellation?.Cancel();

            Console.Clear();
            ConsoleUI.WriteLineColored("\n❌ CONNECTION LOST\n", ConsoleColor.Red);
            Console.WriteLine("The connection to the server was lost.");
            Console.WriteLine("\nPress any key to return to menu...");
            Console.ReadKey();
        }

        private void HandleGameStateChanged(string message)
        {
            Console.WriteLine($"[GAME STATE] {message}");
        }

        private void HandlePlayerStatusChanged(string message)
        {
            Console.WriteLine($"[PLAYER STATUS] {message}");
        }

        private void HandleError(string error)
        {
            Console.WriteLine($"[ERROR] {error}");
        }

        public void Cleanup()
        {
            _isRunning = false;
            _gameLoopCancellation?.Cancel();
            _eventHandler.UnregisterEventHandlers();
        }
    }
}