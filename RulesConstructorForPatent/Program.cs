using System.Text;
using Microsoft.EntityFrameworkCore;
using RulesConstructorForPatent.ConsoleUi;
using RulesConstructorForPatent.Data;
using RulesConstructorForPatent.Services;

namespace RulesConstructorForPatent
{
    static class Program
    {
        static void Main()
        {
            Console.OutputEncoding = Encoding.UTF8;
            Console.InputEncoding = Encoding.Unicode;

            Screen.Banner();

            try
            {
                using var dbContext = new RuleDbContext();
                dbContext.Database.EnsureCreated();

                var ruleService = new RuleService(new UnitOfWork(dbContext), new RuleDirector());
                var answers = RuleSurvey.Ask(ruleService);

                Console.WriteLine();
                Screen.Progress("Сохраняем правило...");
                var rule = ruleService.CreateRule(
                    answers.RuleName, answers.TargetDocumentNames,
                    answers.GuidanceDescription, answers.Refusal,
                    answers.OrganizationNames, answers.OrganizationAddresses,
                    answers.RequiredRuleIds,
                    answers.ProfileDays, answers.ProfileEntryPurposes, answers.ProfileCitizenships,
                    answers.ProfilePropertyNames, answers.ProfilePropertyValues);

                RulePrinter.Print(rule);
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
