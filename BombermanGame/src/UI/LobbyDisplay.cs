using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace BombermanGame.src.UI
{
	
	public class LobbyDisplay
	{
		private const int REFRESH_INTERVAL = 2000; // 2 saniye
		private bool _isWaiting = false;
		private CancellationTokenSource? _refreshCancellation;

	

		
		public void ShowRoomList(List<RoomInfo> rooms, int selectedIndex = -1)
		{
			Console.Clear();
			Console.WriteLine("╔══════════════════════════════════════════════════════════════╗");
			Console.WriteLine("║                      AVAILABLE ROOMS                         ║");
			Console.WriteLine("╚══════════════════════════════════════════════════════════════╝\n");

			if (rooms == null || rooms.Count == 0)
			{
				ConsoleUI.WriteLineColored("  No rooms available. Create one to get started!\n", ConsoleColor.DarkGray);
				Console.WriteLine("\nPress any key to go back...");
				return;
			}

			Console.WriteLine("ID   | Room Name          | Players | Theme    | Status");
			Console.WriteLine("─────┼────────────────────┼─────────┼──────────┼─────────");

			for (int i = 0; i < rooms.Count; i++)
			{
				var room = rooms[i];
				var statusColor = room.State == "Waiting" ? ConsoleColor.Green : ConsoleColor.Yellow;
				var isSelected = i == selectedIndex;

				if (isSelected)
				{
					Console.ForegroundColor = ConsoleColor.Yellow;
					Console.Write("► ");
				}
				else
				{
					Console.Write("  ");
				}

				// Room number
				Console.Write($"{i + 1,-3} | ");

				// Room name
				Console.Write($"{TruncateString(room.Name, 18),-18} | ");

				// Players
				var playerStr = $"{room.CurrentPlayers}/{room.MaxPlayers}";
				if (room.IsFull)
				{
					Console.ForegroundColor = ConsoleColor.Red;
					Console.Write($"{playerStr,-7} | ");
					Console.ResetColor();
				}
				else
				{
					Console.Write($"{playerStr,-7} | ");
				}

				// Theme
				Console.Write($"{room.Theme,-8} | ");

				// Status
				Console.ForegroundColor = statusColor;
				Console.Write(room.State);
				Console.ResetColor();
				Console.WriteLine();

				if (isSelected)
				{
					Console.ResetColor();
				}
			}

			Console.WriteLine("─────┴────────────────────┴─────────┴──────────┴─────────");
			Console.WriteLine("\nUse ↑↓ to navigate | Enter to join | ESC to go back | R to refresh");
		}

		
		public int NavigateRoomList(List<RoomInfo> rooms)
		{
			if (rooms == null || rooms.Count == 0)
			{
				ShowRoomList(rooms);
				Console.ReadKey(true);
				return -1;
			}

			int selectedIndex = 0;

			while (true)
			{
				ShowRoomList(rooms, selectedIndex);

				var key = Console.ReadKey(true);

				switch (key.Key)
				{
					case ConsoleKey.UpArrow:
					case ConsoleKey.W:
						selectedIndex = (selectedIndex - 1 + rooms.Count) % rooms.Count;
						break;

					case ConsoleKey.DownArrow:
					case ConsoleKey.S:
						selectedIndex = (selectedIndex + 1) % rooms.Count;
						break;

					case ConsoleKey.Enter:
					case ConsoleKey.Spacebar:
						if (!rooms[selectedIndex].IsFull)
						{
							return selectedIndex; // Return selected room index
						}
						else
						{
							ConsoleUI.ShowError("Room is full!");
							Thread.Sleep(1500);
						}
						break;

					case ConsoleKey.R:
						return -2; // Signal to refresh

					case ConsoleKey.Escape:
						return -1; // Go back
				}
			}
		}

		

		

		#region Create Room Form

		
		public CreateRoomData? ShowCreateRoomForm(string defaultPlayerName)
		{
			Console.Clear();
			Console.WriteLine("╔══════════════════════════════════════════════════════════════╗");
			Console.WriteLine("║                      CREATE ROOM                             ║");
			Console.WriteLine("╚══════════════════════════════════════════════════════════════╝\n");

			// Room Name
			Console.Write("Room Name: ");
			string? roomName = Console.ReadLine();
			if (string.IsNullOrWhiteSpace(roomName))
			{
				roomName = $"{defaultPlayerName}'s Room";
			}

			// Player Name
			Console.Write($"Your Name (default: {defaultPlayerName}): ");
			string? playerName = Console.ReadLine();
			if (string.IsNullOrWhiteSpace(playerName))
			{
				playerName = defaultPlayerName;
			}

			// Theme Selection
			Console.WriteLine("\nSelect Theme:");
			Console.WriteLine("  1. Desert");
			Console.WriteLine("  2. Forest");
			Console.WriteLine("  3. City");
			Console.Write("\nChoice (1-3, default: Desert): ");

			string? themeChoice = Console.ReadLine();
			string theme = themeChoice switch
			{
				"2" => "Forest",
				"3" => "City",
				_ => "Desert"
			};

			// Max Players
			Console.Write("\nMax Players (2-4, default: 2): ");
			string? maxPlayersInput = Console.ReadLine();
			int maxPlayers = 2;
			if (int.TryParse(maxPlayersInput, out int parsedMax))
			{
				maxPlayers = Math.Clamp(parsedMax, 2, 4);
			}

			// Confirmation
			Console.WriteLine("\n──────────────────────────────────────────────────────────────");
			Console.WriteLine("Room Configuration:");
			Console.WriteLine($"  Room Name:    {roomName}");
			Console.WriteLine($"  Your Name:    {playerName}");
			Console.WriteLine($"  Theme:        {theme}");
			Console.WriteLine($"  Max Players:  {maxPlayers}");
			Console.WriteLine("──────────────────────────────────────────────────────────────");

			Console.Write("\nCreate this room? (Y/N): ");
			var confirm = Console.ReadKey(true);

			if (confirm.Key != ConsoleKey.Y)
			{
				ConsoleUI.ShowWarning("Room creation cancelled");
				Thread.Sleep(1000);
				return null;
			}

			return new CreateRoomData
			{
				RoomName = roomName,
				PlayerName = playerName,
				Theme = theme,
				MaxPlayers = maxPlayers
			};
		}

		#endregion

		#region Player List

		public void ShowPlayerList(List<string> playerNames, bool isHost)
		{
			Console.WriteLine("\n╔══════════════════════════════════════════════════════════════╗");
			Console.WriteLine("║                      PLAYERS IN ROOM                         ║");
			Console.WriteLine("╚══════════════════════════════════════════════════════════════╝\n");

			if (playerNames.Count == 0)
			{
				ConsoleUI.WriteLineColored("  No players in room\n", ConsoleColor.DarkGray);
				return;
			}

			for (int i = 0; i < playerNames.Count; i++)
			{
				var icon = i == 0 ? "👑" : "👤";
				var status = i == 0 ? "(Host)" : "";

				if (i == 0)
				{
					ConsoleUI.WriteColored($"{icon} {playerNames[i]} {status}\n", ConsoleColor.Yellow);
				}
				else
				{
					Console.WriteLine($"{icon} {playerNames[i]} {status}");
				}
			}

			Console.WriteLine();
		}

		#endregion

		
		
		

		

		
		

		

		

		#region Helper Methods

		
		private string TruncateString(string text, int maxLength)
		{
			if (string.IsNullOrEmpty(text))
				return new string(' ', maxLength);

			if (text.Length <= maxLength)
				return text;

			return text.Substring(0, maxLength - 3) + "...";
		}

		
		public void ShowLoadingAnimation(string message, int durationMs = 2000)
		{
			string[] frames = { "⠋", "⠙", "⠹", "⠸", "⠼", "⠴", "⠦", "⠧", "⠇", "⠏" };
			int iterations = durationMs / 100;

			for (int i = 0; i < iterations; i++)
			{
				Console.Write($"\r{frames[i % frames.Length]} {message}");
				Thread.Sleep(100);
			}
			Console.WriteLine();
		}

	
		public void ShowSuccessMessage(string message)
		{
			Console.WriteLine();
			ConsoleUI.WriteLineColored($"✓ {message}", ConsoleColor.Green);
			Thread.Sleep(1500);
		}

		
		public void ShowErrorMessage(string message)
		{
			Console.WriteLine();
			ConsoleUI.WriteLineColored($"✗ {message}", ConsoleColor.Red);
			Thread.Sleep(2000);
		}

		#endregion
	}

	#region Data Classes

	
	public class CreateRoomData
	{
		public string RoomName { get; set; } = "";
		public string PlayerName { get; set; } = "";
		public string Theme { get; set; } = "Desert";
		public int MaxPlayers { get; set; } = 2;
	}

	
	public class RoomInfo
	{
		public string Id { get; set; } = "";
		public string Name { get; set; } = "";
		public string Theme { get; set; } = "Desert";
		public string State { get; set; } = "Waiting";
		public int CurrentPlayers { get; set; }
		public int MaxPlayers { get; set; } = 2;
		public List<string> PlayerNames { get; set; } = new();
		public bool IsFull => CurrentPlayers >= MaxPlayers;
	}

	#endregion
}