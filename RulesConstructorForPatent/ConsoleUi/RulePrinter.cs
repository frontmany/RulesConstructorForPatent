using RulesConstructorForPatent.Services;

namespace RulesConstructorForPatent.ConsoleUi
{
    public static class RulePrinter
    {
        private const int LabelWidth = 34;

        public static void Print(RuleDetails rule)
        {
            Screen.Title($"Правило #{rule.Id} сохранено");

            PrintField("Название", [rule.Name]);
            PrintField("Целевой документ", rule.TargetDocumentNames);
            PrintField("Что нужно сделать", [rule.GuidanceDescription]);
            PrintField("Организации", rule.Organizations.Select(FormatOrganization));
            PrintField("Что стоит попробовать при отказе", [rule.Refusal]);
            PrintField("Зависит от", rule.RequiredRules.Select(required => $"#{required.Id} {required.Name}"));

            Screen.Label("Профили");
            Screen.Hint("Правило действует, если мигрант подходит хотя бы под один");
            int number = 1;
            foreach (var profile in rule.Profiles)
            {
                Screen.SubHeader($"Профиль {number++}, {FormatDays(profile.Days)}");

                if (profile.Conditions.Count == 0)
                    Screen.Hint("  Без условий — подходит всем мигрантам");

                foreach (var condition in profile.Conditions)
                    Console.WriteLine($"    {condition.Name}: {JoinWithOr(condition.Values)}");
            }
        }

        private static void PrintField(string label, IEnumerable<string> values)
        {
            var lines = values.Where(value => value.Length > 0).ToList();
            if (lines.Count == 0)
                lines.Add("—");

            Screen.Write("  " + label.PadRight(LabelWidth), ConsoleColor.DarkGray);
            Console.WriteLine(lines[0]);
            foreach (var line in lines.Skip(1))
                Console.WriteLine("  " + new string(' ', LabelWidth) + line);
        }

        private static string FormatOrganization(OrganizationDetails organization)
        {
            return organization.Address.Length == 0
                ? organization.Name
                : $"{organization.Name} — {organization.Address}";
        }

        private static string FormatDays(int days)
        {
            if (days == 0)
                return "срок не установлен";

            return $"срок {days} {ChoosePluralForm(days, "день", "дня", "дней")}";
        }

        // 1 день, 2 дня, 5 дней, 11 дней, 21 день.
        private static string ChoosePluralForm(int number, string one, string few, string many)
        {
            int lastTwoDigits = number % 100;
            int lastDigit = number % 10;

            if (lastTwoDigits is >= 11 and <= 14)
                return many;
            if (lastDigit == 1)
                return one;
            if (lastDigit is >= 2 and <= 4)
                return few;
            return many;
        }

        // «Беларусь», «Беларусь или Украина», «Армения, Беларусь или Украина».
        private static string JoinWithOr(IEnumerable<string> values)
        {
            var list = values.ToList();
            if (list.Count == 1)
                return list[0];

            return string.Join(", ", list.Take(list.Count - 1)) + " или " + list[^1];
        }
    }
}
