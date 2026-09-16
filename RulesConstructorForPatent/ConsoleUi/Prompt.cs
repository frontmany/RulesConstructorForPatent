namespace RulesConstructorForPatent.ConsoleUi
{
    // Ввод с проверкой: если ответ не подходит, объясняем почему и спрашиваем снова.
    public static class Prompt
    {
        public static string Text(string label, string hint, bool isRequired = true)
        {
            Ask(label, hint);

            while (true)
            {
                string answer = ReadAnswer();
                if (answer.Length > 0 || !isRequired)
                    return answer;

                Screen.Error($"Заполните поле «{label}»");
            }
        }

        public static int Integer(string label, string hint, int min, int max)
        {
            Ask(label, hint);

            while (true)
            {
                if (int.TryParse(ReadAnswer(), out int number) && number >= min && number <= max)
                    return number;

                Screen.Error($"Введите целое число от {min} до {max}");
            }
        }

        // Подсказка печатается над вопросом: ответ вводится в той же строке, что и вопрос.
        public static bool YesNo(string question, string hint = "")
        {
            Console.WriteLine();
            PrintHint(hint);

            while (true)
            {
                Screen.Write($"  {question} (д/н) ", ConsoleColor.White);
                Screen.Write("> ", ConsoleColor.Cyan);

                string answer = ReadLine().ToLowerInvariant();
                if (answer is "д" or "да")
                    return true;
                if (answer is "н" or "нет")
                    return false;

                Screen.Error("Ответьте «д» или «н»");
            }
        }

        // Выбор нескольких пунктов списка по номерам. Возвращает индексы выбранных пунктов.
        // emptyAnswerMeaning — что означает пустой ответ; null, если выбрать нужно хотя бы один пункт.
        public static List<int> ChooseMany(string label, IReadOnlyList<string> options, string? emptyAnswerMeaning)
        {
            Screen.ShowLabel(label);
            for (int i = 0; i < options.Count; i++)
            {
                Screen.Write($"  {i + 1,3}  ", ConsoleColor.Cyan);
                Console.WriteLine(options[i]);
            }

            Screen.Hint(emptyAnswerMeaning == null
                ? "Номера через запятую"
                : $"Номера через запятую; Enter — {emptyAnswerMeaning}");

            while (true)
            {
                string answer = ReadAnswer();

                if (answer.Length == 0 && emptyAnswerMeaning != null)
                    return [];

                if (TryParseNumbers(answer, options.Count, out var indexes))
                    return indexes;

                Screen.Error(options.Count == 1
                    ? "Введите номер 1"
                    : $"Введите номера от 1 до {options.Count} через запятую, например: 1, 2");
            }
        }

        private static void Ask(string label, string hint)
        {
            Screen.ShowLabel(label);
            PrintHint(hint);
        }

        // Подсказка может состоять из нескольких строк, разделённых \n.
        private static void PrintHint(string hint)
        {
            if (hint.Length == 0)
                return;

            foreach (var line in hint.Split('\n'))
                Screen.Hint(line);
        }

        private static string ReadAnswer()
        {
            Screen.Write("  > ", ConsoleColor.Cyan);
            return ReadLine();
        }

        // null от Console.ReadLine означает, что ввод закончился (например, файл с ответами исчерпан).
        private static string ReadLine()
        {
            return (Console.ReadLine() ?? throw new EndOfStreamException()).Trim();
        }

        private static bool TryParseNumbers(string answer, int optionCount, out List<int> indexes)
        {
            indexes = [];

            var parts = answer.Split([',', ';', ' '], StringSplitOptions.RemoveEmptyEntries);
            foreach (var part in parts)
            {
                if (!int.TryParse(part, out int number) || number < 1 || number > optionCount)
                    return false;

                if (!indexes.Contains(number - 1))
                    indexes.Add(number - 1);
            }

            return indexes.Count > 0;
        }
    }
}
