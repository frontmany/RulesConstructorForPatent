using System.Text;
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

            using var dbContext = new RuleDbContext();
            dbContext.Database.EnsureCreated();

            var ruleService = new RuleService(new UnitOfWork(dbContext), new RuleDirector());
            if (ruleService.GetProfileConditions().Count == 0)
            {
                Console.WriteLine();
                Screen.Error("Справочник условий пуст: заполните его скриптом tools/init_db.py");
                Screen.WaitForExit();
                return;
            }

            var answers = RuleSurvey.Ask(ruleService);

            Console.WriteLine();
            Screen.Progress("Сохраняем правило...");
            var rule = ruleService.CreateRule(
                answers.RuleName, answers.TargetDocumentNames,
                answers.GuidanceDescription, answers.Refusal,
                answers.OrganizationNames, answers.OrganizationAddresses,
                answers.RequiredRuleIds,
                answers.ProfileDays, answers.ProfileOptionIds);

            RulePrinter.Print(rule);

            Screen.WaitForExit();
        }
    }
}
