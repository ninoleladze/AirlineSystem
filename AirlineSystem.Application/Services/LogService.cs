using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AirlineSystem.AirlineSystem.Application.Services
{
    internal static class LogService
    {
        private static readonly string LogPath =
            Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "airline.log");

        public static void LogInfo(string message) => Write(message, "INFO");
        public static void LogWarning(string message) => Write(message, "WARN");
        public static void LogError(string message) => Write(message, "ERROR");

        private static void Write(string message, string level)
        {
            string entry = $"[{DateTime.Now:F}][{level}] {message}";
            try { File.AppendAllText(LogPath, entry + Environment.NewLine); }
            catch { }
        }


        public static void ViewLogs(int lines = 30)
        {
            if (!File.Exists(LogPath))
            {
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine("No log file found yet.");
                Console.ResetColor();
                return;
            }

            var recent = File.ReadAllLines(LogPath).TakeLast(lines).ToArray();
            Console.WriteLine($"=== Last {recent.Length} Log Entries ===");
            foreach (var line in recent)
            {
                if (line.Contains("[ERROR]")) Console.ForegroundColor = ConsoleColor.Red;
                else if (line.Contains("[WARN]")) Console.ForegroundColor = ConsoleColor.Yellow;
                else Console.ForegroundColor = ConsoleColor.Gray;
                Console.WriteLine(line);
            }
            Console.ResetColor();
        }

        public static void ClearLogs()
        {
            if(File.Exists(LogPath)) File.Delete(LogPath);
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("Logs cleared.");
            Console.ResetColor();
        }

    }


    }

