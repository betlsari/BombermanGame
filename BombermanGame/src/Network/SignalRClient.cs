using Microsoft.AspNetCore.SignalR.Client;
using System;
using System.Threading.Tasks;
using BombermanGame.src.Models;

namespace BombermanGame.src.Network
{
	public class SignalRClient
	{
		private HubConnection? _connection;
		private string _serverUrl = "";
		private bool _isConnected = false;
		private int _reconnectAttempts = 0;
		private const int MaxReconnectAttempts = 5;

		public event Action<string>? OnConnected;
		public event Action<string>? OnDisconnected;
		public event Action<string>? OnReconnecting;
		public event Action<string>? OnReconnected;
		public event Action<Exception>? OnConnectionError;

		public event Action<string, object>? OnRoomCreated;
		public event Action<string, object>? OnPlayerJoined;
		public event Action<string>? OnPlayerLeft;
		public event Action<object>? OnGameStarted;
		public event Action<string, int, int>? OnPlayerMoved;
		public event Action<string, int, int, int>? OnBombPlaced;
		public event Action<object>? OnGameStateUpdated;
		public event Action<string, int>? OnGameEnded;
		public event Action<string>? OnError;
		public event Action<object>? OnRoomListReceived;

		public bool IsConnected => _isConnected && _connection?.State == HubConnectionState.Connected;
		public string ConnectionId => _connection?.ConnectionId ?? "";

		public async Task<bool> ConnectAsync(string serverUrl)
		{
			try
			{
				_serverUrl = serverUrl.TrimEnd('/');

				_connection = new HubConnectionBuilder()
					.WithUrl($"{_serverUrl}/gameHub")
					.WithAutomaticReconnect(new[]
					{
						TimeSpan.Zero,
						TimeSpan.FromSeconds(2),
						TimeSpan.FromSeconds(5),
						TimeSpan.FromSeconds(10),
						TimeSpan.FromSeconds(30)
					})
					.Build();

				RegisterEventHandlers();
				RegisterConnectionEvents();

				await _connection.StartAsync();
				_isConnected = true;
				_reconnectAttempts = 0;

				return true;
			}
			catch (Exception ex)
			{
				_isConnected = false;
				OnConnectionError?.Invoke(ex);
				return false;
			}
		}

		public async Task DisconnectAsync()
		{
			if (_connection != null)
			{
				try
				{
					await _connection.StopAsync();
					await _connection.DisposeAsync();
					_connection = null;
					_isConnected = false;
				}
				catch (Exception ex)
				{
					OnConnectionError?.Invoke(ex);
				}
			}
		}

		private void RegisterConnectionEvents()
		{
			if (_connection == null) return;

			_connection.Closed += async (error) =>
			{
				_isConnected = false;
				OnDisconnected?.Invoke(error?.Message ?? "Connection closed");

				if (_reconnectAttempts < MaxReconnectAttempts)
				{
					await Task.Delay(TimeSpan.FromSeconds(5));
					await TryReconnectAsync();
				}
			};

			_connection.Reconnecting += (error) =>
			{
				_isConnected = false;
				_reconnectAttempts++;
				OnReconnecting?.Invoke($"Reconnecting... Attempt {_reconnectAttempts}");
				return Task.CompletedTask;
			};

			_connection.Reconnected += (connectionId) =>
			{
				_isConnected = true;
				_reconnectAttempts = 0;
				OnReconnected?.Invoke(connectionId ?? "");
				return Task.CompletedTask;
			};
		}

		private async Task TryReconnectAsync()
		{
			try
			{
				if (_connection?.State == HubConnectionState.Disconnected)
				{
					await _connection.StartAsync();
					_isConnected = true;
					_reconnectAttempts = 0;
				}
			}
			catch (Exception ex)
			{
				OnConnectionError?.Invoke(ex);

				if (_reconnectAttempts < MaxReconnectAttempts)
				{
					await Task.Delay(TimeSpan.FromSeconds(10));
					await TryReconnectAsync();
				}
			}
		}

		private void RegisterEventHandlers()
		{
			if (_connection == null) return;

			_connection.On<string>("Connected", (connectionId) =>
			{
				OnConnected?.Invoke(connectionId);
			});

			_connection.On<string, object>("PlayerJoined", (playerName, roomInfo) =>
			{
				OnPlayerJoined?.Invoke(playerName, roomInfo);
			});

			_connection.On<string>("PlayerLeft", (playerName) =>
			{
				OnPlayerLeft?.Invoke(playerName);
			});

			_connection.On<object>("GameStarted", (startMessage) =>
			{
				OnGameStarted?.Invoke(startMessage);
			});

			_connection.On<string, int, int>("PlayerMoved", (connectionId, x, y) =>
			{
				OnPlayerMoved?.Invoke(connectionId, x, y);
			});

			_connection.On<string, int, int, int>("BombPlaced", (connectionId, x, y, range) =>
			{
				OnBombPlaced?.Invoke(connectionId, x, y, range);
			});

			_connection.On<object>("GameStateUpdated", (gameState) =>
			{
				OnGameStateUpdated?.Invoke(gameState);
			});

			_connection.On<string, int>("GameEnded", (winnerId, finalScore) =>
			{
				OnGameEnded?.Invoke(winnerId, finalScore);
			});

			_connection.On<string>("Error", (errorMessage) =>
			{
				OnError?.Invoke(errorMessage);
			});
		}

		public async Task<object?> CreateRoomAsync(string roomName, string playerName, string theme = "Desert", int maxPlayers = 2)
		{
			if (!IsConnected || _connection == null)
			{
				throw new InvalidOperationException("Not connected to server");
			}

			try
			{
				var request = new
				{
					RoomName = roomName,
					PlayerName = playerName,
					Theme = theme,
					MaxPlayers = maxPlayers
				};

				var response = await _connection.InvokeAsync<object>("CreateRoom", request);
				return response;
			}
			catch (Exception ex)
			{
				OnConnectionError?.Invoke(ex);
				return null;
			}
		}

		public async Task<bool> JoinRoomAsync(string roomId, string playerName)
		{
			if (!IsConnected || _connection == null)
			{
				throw new InvalidOperationException("Not connected to server");
			}

			try
			{
				var request = new
				{
					RoomId = roomId,
					PlayerName = playerName
				};

				var success = await _connection.InvokeAsync<bool>("JoinRoom", request);
				return success;
			}
			catch (Exception ex)
			{
				OnConnectionError?.Invoke(ex);
				return false;
			}
		}

		public async Task LeaveRoomAsync(string roomId)
		{
			if (!IsConnected || _connection == null)
			{
				return;
			}

			try
			{
				await _connection.InvokeAsync("LeaveRoom", roomId);
			}
			catch (Exception ex)
			{
				OnConnectionError?.Invoke(ex);
			}
		}

		public async Task<object?> GetRoomListAsync()
		{
			if (!IsConnected || _connection == null)
			{
				throw new InvalidOperationException("Not connected to server");
			}

			try
			{
				var response = await _connection.InvokeAsync<object>("GetRoomList");
				return response;
			}
			catch (Exception ex)
			{
				OnConnectionError?.Invoke(ex);
				return null;
			}
		}

		public async Task StartGameAsync(string roomId)
		{
			if (!IsConnected || _connection == null)
			{
				throw new InvalidOperationException("Not connected to server");
			}

			try
			{
				await _connection.InvokeAsync("StartGame", roomId);
			}
			catch (Exception ex)
			{
				OnConnectionError?.Invoke(ex);
			}
		}

		public async Task SendMoveAsync(string roomId, int x, int y)
		{
			if (!IsConnected || _connection == null)
			{
				return;
			}

			try
			{
				var message = new
				{
					RoomId = roomId,
					X = x,
					Y = y
				};

				await _connection.InvokeAsync("PlayerMove", message);
			}
			catch (Exception ex)
			{
				OnConnectionError?.Invoke(ex);
			}
		}

		public async Task SendPlaceBombAsync(string roomId, int x, int y, int range)
		{
			if (!IsConnected || _connection == null)
			{
				return;
			}

			try
			{
				var message = new
				{
					RoomId = roomId,
					X = x,
					Y = y,
					Range = range
				};

				await _connection.InvokeAsync("PlaceBomb", message);
			}
			catch (Exception ex)
			{
				OnConnectionError?.Invoke(ex);
			}
		}

		public async Task SendGameStateAsync(string roomId, object gameState)
		{
			if (!IsConnected || _connection == null)
			{
				return;
			}

			try
			{
				await _connection.InvokeAsync("UpdateGameState", roomId, gameState);
			}
			catch (Exception ex)
			{
				OnConnectionError?.Invoke(ex);
			}
		}

		public async Task SendChatMessageAsync(string roomId, string message)
		{
			if (!IsConnected || _connection == null)
			{
				return;
			}

			try
			{
				var chatMessage = new
				{
					RoomId = roomId,
					Message = message
				};

				await _connection.InvokeAsync("SendChatMessage", chatMessage);
			}
			catch (Exception ex)
			{
				OnConnectionError?.Invoke(ex);
			}
		}

		public async Task NotifyGameEndAsync(string roomId, string winnerId, int finalScore)
		{
			if (!IsConnected || _connection == null)
			{
				return;
			}

			try
			{
				await _connection.InvokeAsync("NotifyGameEnd", roomId, winnerId, finalScore);
			}
			catch (Exception ex)
			{
				OnConnectionError?.Invoke(ex);
			}
		}

		public HubConnectionState GetConnectionState()
		{
			return _connection?.State ?? HubConnectionState.Disconnected;
		}

		public async Task<long> MeasureLatencyAsync()
		{
			if (!IsConnected || _connection == null)
			{
				return -1;
			}

			try
			{
				var startTime = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();
				await _connection.InvokeAsync("Ping");
				var endTime = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();
				return endTime - startTime;
			}
			catch
			{
				return -1;
			}
		}
	}
}