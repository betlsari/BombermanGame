
using System;

namespace BombermanGame.src.UI
{
    public class ConsoleUI
    {
        
        public static void Initialize()
        {
            Console.Clear();
            Console.CursorVisible = false;
        }

        
        public static void SetCursorPosition(int x, int y)
        {
            if (x >= 0 && x < Console.WindowWidth && y >= 0 && y < Console.WindowHeight)
            {
                Console.SetCursorPosition(x, y);
            }
        }

        
        public static void WriteColored(string text, ConsoleColor color)
        {
            Console.ForegroundColor = color;
            Console.Write(text);
            Console.ResetColor();
        }

        
        public static void WriteLineColored(string text, ConsoleColor color)
        {
            Console.ForegroundColor = color;
            Console.WriteLine(text);
            Console.ResetColor();
        }

        
        public static void DrawTitle(string title)
        {
            int padding = (62 - title.Length) / 2;
            Console.WriteLine("╔══════════════════════════════════════════════════════════════╗");
            Console.WriteLine($"║{new string(' ', padding)}{title}{new string(' ', 62 - padding - title.Length)}║");
            Console.WriteLine("╚══════════════════════════════════════════════════════════════╝");
        }

        
        public static void DrawHeader(string header)
        {
            Console.WriteLine(header);
            Console.WriteLine(new string('─', header.Length));
        }

        
        public static bool ShowConfirmation(string message)
        {
            Console.Write($"\n{message} (Y/N): ");
            var key = Console.ReadKey(true);
            Console.WriteLine(key.KeyChar);
            return key.Key == ConsoleKey.Y;
        }

        
        public static void WaitForKey(string message = "Press any key to continue...")
        {
            Console.WriteLine($"\n{message}");
            Console.ReadKey(true);
        }

        
        public static void ShowSuccess(string message)
        {
            WriteLineColored($"\n✓ {message}", ConsoleColor.Green);
        }

       
        public static void ShowError(string message)
        {
            WriteLineColored($"\n✗ {message}", ConsoleColor.Red);
        }

       
        public static void ShowWarning(string message)
        {
            WriteLineColored($"\n⚠ {message}", ConsoleColor.Yellow);
        }

        
        public static void ShowInfo(string message)
        {
            WriteLineColored($"\nℹ {message}", ConsoleColor.Cyan);
        }

        
        public static void DrawProgressBar(int current, int total, int width = 40)
        {
            int filled = (int)((double)current / total * width);
            int empty = width - filled;

            Console.Write("[");
            WriteColored(new string('█', filled), ConsoleColor.Green);
            Console.Write(new string('░', empty));
            Console.Write($"] {current}/{total}");
        }

        
        public static void DrawLine(char character = '─', int length = 62)
        {
            Console.WriteLine(new string(character, length));
        }

        
        public static void AddSpacing(int lines = 1)
        {
            for (int i = 0; i < lines; i++)
            {
                Console.WriteLine();
            }
        }

        
        public static void ClearWithTitle(string title)
        {
            Console.Clear();
            DrawTitle(title);
            Console.WriteLine();
        }

        
        public static void DrawMenuOption(int number, string text, bool isSelected = false)
        {
            if (isSelected)
            {
                WriteColored($"► {number}. {text}\n", ConsoleColor.Yellow);
            }
            else
            {
                Console.WriteLine($"  {number}. {text}");
            }
        }

        
        public static void DrawBox(string[] lines)
        {
            int maxLength = 0;
            foreach (var line in lines)
            {
                if (line.Length > maxLength)
                    maxLength = line.Length;
            }

            Console.WriteLine("╔" + new string('═', maxLength + 2) + "╗");
            foreach (var line in lines)
            {
                Console.WriteLine($"║ {line.PadRight(maxLength)} ║");
            }
            Console.WriteLine("╚" + new string('═', maxLength + 2) + "╝");
        }

        
        public static string ReadPassword()
        {
            string password = "";
            ConsoleKeyInfo key;

            do
            {
                key = Console.ReadKey(true);

                if (key.Key != ConsoleKey.Backspace && key.Key != ConsoleKey.Enter)
                {
                    password += key.KeyChar;
                    Console.Write("*");
                }
                else if (key.Key == ConsoleKey.Backspace && password.Length > 0)
                {
                    password = password.Substring(0, password.Length - 1);
                    Console.Write("\b \b");
                }
            }
            while (key.Key != ConsoleKey.Enter);

            Console.WriteLine();
            return password;
        }

        
        public static void ShowLoading(string message, int durationMs = 1000)
        {
            string[] spinner = { "⠋", "⠙", "⠹", "⠸", "⠼", "⠴", "⠦", "⠧", "⠇", "⠏" };
            int iterations = durationMs / 100;

            Console.Write(message + " ");

            for (int i = 0; i < iterations; i++)
            {
                Console.Write(spinner[i % spinner.Length]);
                Thread.Sleep(100);
                Console.Write("\b");
            }

            Console.WriteLine("✓");
        }
    }
}