using PIS_6sem.Data;
using PIS_6sem.Entities;
using PIS_6sem.Services;

namespace PIS_6sem
{
    class Program
    {
        static void Main()
        {
            Console.OutputEncoding = System.Text.Encoding.UTF8;

            // Консоль Windows может читать ввод в кодировке без кириллицы (например, 850),
            // и тогда русские буквы приходят как «?». В режиме UTF-16 .NET получает символы
            // напрямую. Для ввода из файла кодировку не трогаем.
            if (OperatingSystem.IsWindows() && !Console.IsInputRedirected)
                Console.InputEncoding = System.Text.Encoding.Unicode;

            var db = new RuleDbContext();
            db.Database.EnsureCreated();

            var unitOfWork = new UnitOfWork(db);
            var director = new RuleDirector();
            var ruleService = new RuleService(unitOfWork, director);


            Console.Write("Название правила: ");
            string ruleName = Console.ReadLine()!;

            Console.Write("Целевые документы (через ;): ");
            var targetDocumentNames = Console.ReadLine()!.Split(';').ToList();

            Console.Write("Описание руководства: ");
            string guidanceDescription = Console.ReadLine()!;

            Console.Write("Описание отказа: ");
            string refusal = Console.ReadLine()!;

            Console.Write("Названия организаций (через ;): ");
            var organizationNames = Console.ReadLine()!.Split(';').ToList();

            Console.Write("Адреса организаций (через ;): ");
            var organizationAddresses = Console.ReadLine()!.Split(';').ToList();


            var profileDays = new List<int>();
            var profileEntryPurposes = new List<List<string>>();
            var profileCitizenships = new List<List<string>>();
            var profilePropertyNames = new List<List<string>>();
            var profilePropertyValues = new List<List<string>>();

            while (true)
            {
                Console.WriteLine("Профиль");
                Console.Write("Количество дней: ");
                profileDays.Add(int.Parse(Console.ReadLine()!));

                Console.Write("Цели въезда (через ;): ");
                profileEntryPurposes.Add([.. Console.ReadLine()!.Split(';')]);

                Console.Write("Гражданства (через ;): ");
                profileCitizenships.Add([.. Console.ReadLine()!.Split(';')]);

                var propertyNames = new List<string>();
                var propertyValues = new List<string>();

                while (true)
                {
                    Console.Write("Добавить свойство? (д/н): ");
                    if (Console.ReadLine()?.ToLower() != "д") break;

                    Console.Write("Название свойства: ");
                    propertyNames.Add(Console.ReadLine()!);

                    Console.Write("Значение свойства: ");
                    propertyValues.Add(Console.ReadLine()!);
                }

                profilePropertyNames.Add(propertyNames);
                profilePropertyValues.Add(propertyValues);

                Console.Write("Добавить ещё профиль? (д/н): ");
                if (Console.ReadLine()?.ToLower() != "д") break;
            }


            var rule = ruleService.CreateRule(
                ruleName, targetDocumentNames,
                guidanceDescription, refusal,
                organizationNames, organizationAddresses,
                profileDays, profileEntryPurposes, profileCitizenships,
                profilePropertyNames, profilePropertyValues);


            PrintRule(rule);
            Console.ReadKey();
        }

        static void PrintRule(Rule rule)
        {
            Console.WriteLine($"\nПравило #{rule.Id}: {rule.Name}");

            Console.WriteLine("\nДокументы:");
            foreach (var document in rule.TargetDocuments)
                Console.WriteLine($"  - {document.Name}");

            Console.WriteLine("\nПрофили:");
            int i = 1;
            foreach (var profile in rule.Profiles)
            {
                Console.WriteLine($"  Профиль #{i++} | Дни: {profile.Days}");
                foreach (var property in profile.Properties)
                    Console.WriteLine($"    {property.Name} = {property.Value}");
            }

            if (rule.Guidance != null)
            {
                Console.WriteLine($"\nРуководство: {rule.Guidance.Description}");
                Console.WriteLine($"Отказ: {rule.Guidance.Refusal}");
                foreach (var organization in rule.Guidance.Organizations)
                    Console.WriteLine($"  {organization.Name} — {organization.Address}");
            }
        }
    }
}
