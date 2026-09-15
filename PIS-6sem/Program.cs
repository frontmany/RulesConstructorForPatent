using System.Text;
using Microsoft.EntityFrameworkCore;
using PIS_6sem.ConsoleUi;
using PIS_6sem.Data;
using PIS_6sem.Services;

namespace PIS_6sem
{
    static class Program
    {
        static void Main()
        {
            Console.OutputEncoding = Encoding.UTF8;

            // Консоль Windows может читать ввод в кодировке без кириллицы (например, 850),
            // и тогда русские буквы приходят как «?». В режиме UTF-16 .NET получает символы
            // напрямую. Для ввода из файла кодировку не трогаем.
            if (OperatingSystem.IsWindows() && !Console.IsInputRedirected)
                Console.InputEncoding = Encoding.Unicode;

            Screen.Banner();

            try
            {
                using var dbContext = new RuleDbContext();
                dbContext.Database.EnsureCreated();

                var ruleService = new RuleService(new UnitOfWork(dbContext), new RuleDirector());
                var answers = RuleSurvey.Ask(ruleService);

                Console.WriteLine();
                Screen.Progress("Сохраняем правило…");
                var rule = ruleService.CreateRule(
                    answers.RuleName, answers.TargetDocumentNames,
                    answers.GuidanceDescription, answers.Refusal,
                    answers.OrganizationNames, answers.OrganizationAddresses,
                    answers.RequiredRuleIds,
                    answers.ProfileDays, answers.ProfileEntryPurposes, answers.ProfileCitizenships,
                    answers.ProfilePropertyNames, answers.ProfilePropertyValues);

                RulePrinter.Print(rule);
            }
            catch (EndOfStreamException)
            {
                Console.WriteLine();
                Screen.Error("Ввод прервался до конца опроса — правило не сохранено");
            }
            catch (DbUpdateException exception)
            {
                Console.WriteLine();
                Screen.Error($"Не удалось сохранить правило: {exception.InnerException?.Message ?? exception.Message}");
                Screen.Hint("Если файл rules.db остался от прошлой версии программы, удалите его:");
                Screen.Hint("структура базы изменилась, и программа создаст новую");
            }

            Screen.WaitForExit();
        }
    }
}
