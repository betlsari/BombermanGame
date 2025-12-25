
using System;

namespace BombermanGame.src.UI
{
    public class ConsoleUI
    {
        
       
     
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

       

        
       
        
        public static void AddSpacing(int lines = 1)
        {
            for (int i = 0; i < lines; i++)
            {
                Console.WriteLine();
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

     

       
    }
}