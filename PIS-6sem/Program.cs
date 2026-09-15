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

            var db = new RuleDbContext();
            db.Database.EnsureCreated();

            var uow = new UnitOfWork(db);
            var director = new RuleDirector();
            var serviceRule = new ServiceRule(uow, director);


            Console.Write("Название правила: ");
            string ruleName = Console.ReadLine()!;

            Console.Write("Целевые документы (через ;): ");
            var targetDocs = Console.ReadLine()!.Split(';').ToList();

            Console.Write("Описание руководства: ");
            string guidanceDescription = Console.ReadLine()!;

            Console.Write("Описание отказа: ");
            string refusal = Console.ReadLine()!;

            Console.Write("Названия организаций (через ;): ");
            var orgNames = Console.ReadLine()!.Split(';').ToList();

            Console.Write("Адреса организаций (через ;): ");
            var orgAddresses = Console.ReadLine()!.Split(';').ToList();


            var daysList = new List<int>();
            var purposeNamesList = new List<List<string>>();
            var citizenshipNamesList = new List<List<string>>();
            var propertyNames = new List<List<string>>();
            var propertyValues = new List<List<string>>();

            while (true)
            {
                Console.WriteLine("Профиль");
                Console.Write("Количество дней: ");
                daysList.Add(int.Parse(Console.ReadLine()!));

                Console.Write("Цели въезда (через ;): ");
                purposeNamesList.Add([.. Console.ReadLine()!.Split(';')]);

                Console.Write("Гражданства (через ;): ");
                citizenshipNamesList.Add([.. Console.ReadLine()!.Split(';')]);

                var names = new List<string>();
                var values = new List<string>();

                while (true)
                {
                    Console.Write("Добавить свойство? (д/н): ");
                    if (Console.ReadLine()?.ToLower() != "д") break;

                    Console.Write("Название свойства: ");
                    names.Add(Console.ReadLine()!);

                    Console.Write("Значение свойства: ");
                    values.Add(Console.ReadLine()!);
                }

                propertyNames.Add(names);
                propertyValues.Add(values);

                Console.Write("Добавить ещё профиль? (д/н): ");
                if (Console.ReadLine()?.ToLower() != "д") break;
            }


            var rule = serviceRule.CreateRule(
                ruleName, targetDocs,
                guidanceDescription, refusal,
                orgNames, orgAddresses,
                daysList, purposeNamesList, citizenshipNamesList,
                propertyNames, propertyValues);


            PrintRule(rule);
            Console.ReadKey();
        }

        static void PrintRule(Rule rule)
        {
            Console.WriteLine($"\nПравило #{rule.Id}: {rule.Name}");

            Console.WriteLine("\nДокументы:");
            foreach (var doc in rule.TargetDocuments)
                Console.WriteLine($"  - {doc.Name}");

            Console.WriteLine("\nПрофили:");
            int i = 1;
            foreach (var p in rule.Profiles)
            {
                Console.WriteLine($"  Профиль #{i++} | Дни: {p.Days}");
                foreach (var prop in p.Properties)
                    Console.WriteLine($"    {prop.Name} = {prop.Value}");
            }

            if (rule.Guidance != null)
            {
                Console.WriteLine($"\nРуководство: {rule.Guidance.Description}");
                Console.WriteLine($"Отказ: {rule.Guidance.Refusal}");
                foreach (var org in rule.Guidance.Organizations)
                    Console.WriteLine($"  {org.Name} — {org.Address}");
            }
        }
    }
}