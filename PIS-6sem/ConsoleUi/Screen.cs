namespace PIS_6sem.ConsoleUi
{
    // Оформление вывода: рамки, заголовки шагов, подсказки и сообщения разных цветов.
    public static class Screen
    {
        private const int Width = 64;
        private const string Indent = "  ";

        public static void Banner()
        {
            WriteLine(new string('═', Width), ConsoleColor.Cyan);
            WriteLine(Indent + "КОНСТРУКТОР ПРАВИЛ", ConsoleColor.Cyan);
            WriteLine(new string('═', Width), ConsoleColor.Cyan);
            Hint("Серым показаны подсказки и примеры");
        }

        public static void Title(string text)
        {
            // Две пустые строки, как между шагами.
            Console.WriteLine();
            Console.WriteLine();
            WriteLine(new string('═', Width), ConsoleColor.Green);
            WriteLine(Indent + text, ConsoleColor.Green);
            WriteLine(new string('═', Width), ConsoleColor.Green);
        }

        public static void Step(int number, int count, string title)
        {
            string header = $"── Шаг {number} из {count}: {title} ";

            // Две пустые строки отделяют шаги друг от друга.
            Console.WriteLine();
            Console.WriteLine();
            WriteLine(header.PadRight(Width, '─'), ConsoleColor.Yellow);
        }

        public static void SubHeader(string text)
        {
            Console.WriteLine();
            WriteLine(Indent + text, ConsoleColor.Cyan);
        }

        public static void Label(string text)
        {
            Console.WriteLine();
            WriteLine(Indent + text, ConsoleColor.White);
        }

        public static void Hint(string text) => WriteLine(Indent + text, ConsoleColor.DarkGray);

        public static void Progress(string text) => WriteLine(Indent + text, ConsoleColor.DarkCyan);

        public static void Warning(string text) => WriteLine(Indent + text, ConsoleColor.DarkYellow);

        public static void Error(string text) => WriteLine(Indent + text, ConsoleColor.Red);

        public static void WaitForExit()
        {
            // При перенаправленном вводе (например, из файла) клавишу ждать неоткуда.
            if (Console.IsInputRedirected)
                return;

            Console.WriteLine();
            Hint("Нажмите любую клавишу, чтобы закрыть окно");
            Console.ReadKey(intercept: true);
        }

        public static void Write(string text, ConsoleColor color)
        {
            Console.ForegroundColor = color;
            Console.Write(text);
            Console.ResetColor();
        }

        public static void WriteLine(string text, ConsoleColor color)
        {
            Write(text, color);
            Console.WriteLine();
        }
    }
}
