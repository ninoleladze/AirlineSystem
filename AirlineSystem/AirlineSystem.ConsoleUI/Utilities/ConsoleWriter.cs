namespace AirlineSystem.ConsoleUI.Utilities
{
    internal static class ConsoleWriter
    {
        public static void PrintTitle(string title)
        {
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine();
            Console.WriteLine(new string('=', 55));
            Console.WriteLine($"  {title}");
            Console.WriteLine(new string('=', 55));
            Console.ResetColor();
        }

        public static void PrintSuccess(string msg)
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine($"[OK] {msg}");
            Console.ResetColor();
        }

        public static void PrintError(string msg)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine($"[ERROR] {msg}");
            Console.ResetColor();
        }

        public static void PrintInfo(string msg)
        {
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine(msg);
            Console.ResetColor();
        }

        public static string ReadPassword()
        {
            var password = new System.Text.StringBuilder();
            ConsoleKeyInfo key;
            while ((key = Console.ReadKey(intercept: true)).Key != ConsoleKey.Enter)
            {
                if (key.Key == ConsoleKey.Backspace && password.Length > 0)
                {
                    password.Remove(password.Length - 1, 1);
                    Console.Write("\b \b");
                }
                else if (key.Key != ConsoleKey.Backspace)
                {
                    password.Append(key.KeyChar);
                    Console.Write('*');
                }
            }
            Console.WriteLine();
            return password.ToString();
        }

        public static void PrintSeparator()
        {
            Console.ForegroundColor = ConsoleColor.DarkGray;
            Console.WriteLine(new string('-', 55));
            Console.ResetColor();
        }
    }
}