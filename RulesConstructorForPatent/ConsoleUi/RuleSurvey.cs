using RulesConstructorForPatent.Services;

namespace RulesConstructorForPatent.ConsoleUi
{
    public static class RuleSurvey
    {
        private const int StepCount = 4;

        public static RuleSurveyAnswers Ask(RuleService ruleService, List<ProfileCondition> conditions)
        {
            Screen.Step(1, StepCount, "Что нужно получить");
            string ruleName = Prompt.Text(
                "Название правила",
                "Например: Получение ИНН");

            string targetDocumentName = Prompt.Text(
                "Целевой документ",
                "Документ, который можно получить при выполнении правила\n" +
                "Например: ИНН");

            Screen.Step(2, StepCount, "Руководство");
            string guidanceDescription = Prompt.Text(
                "Что нужно сделать",
                "Например: обратиться в инспекцию ФНС");
            var (organizationNames, organizationAddresses) = AskOrganizations();
            string refusal = Prompt.Text(
                "Что стоит попробовать при отказе",
                "Например: исправить ошибки и подать заявление повторно\n" +
                "Enter — пропустить",
                isRequired: false);

            Screen.Step(3, StepCount, "Зависимость от других правил");
            var requiredRuleIds = AskRequiredRuleIds(ruleService);

            Screen.Step(4, StepCount, "Для кого и в какой срок");
            Screen.Hint("Профиль — категория мигрантов и срок для неё. Правило действует,");
            Screen.Hint("если мигрант подходит хотя бы под один профиль");

            int profileNumber = 1;
            var profileDays = new List<int>();
            var profileOptionIds = new List<List<int>>();
            do
            {
                var profile = ProfileSurvey.Ask(profileNumber, conditions);

                profileDays.Add(profile.Days);
                profileOptionIds.Add(profile.OptionIds);
                profileNumber++;
            }
            while (Prompt.YesNo("Добавить ещё профиль?"));

            return new RuleSurveyAnswers
            {
                RuleName = ruleName,
                TargetDocumentNames = [targetDocumentName],
                GuidanceDescription = guidanceDescription,
                Refusal = refusal,
                OrganizationNames = organizationNames,
                OrganizationAddresses = organizationAddresses,
                RequiredRuleIds = requiredRuleIds,
                ProfileDays = profileDays,
                ProfileOptionIds = profileOptionIds
            };
        }

        private static (List<string> Names, List<string> Addresses) AskOrganizations()
        {
            var names = new List<string>();
            var addresses = new List<string>();
            do
            {
                string name = Prompt.Text(
                    $"Организация {names.Count + 1} — название",
                    names.Count == 0
                        ? "Куда обратиться за документом\nНапример: УМВД России по Тюменской области"
                        : "Куда ещё можно обратиться за документом");
                string address = Prompt.Text("Адрес", "Enter — без адреса", isRequired: false);

                names.Add(name);
                addresses.Add(address);
            }
            while (Prompt.YesNo("Добавить ещё организацию?"));

            return (names, addresses);
        }

        // Возвращает Id правил, от которых зависит новое, или null, если зависимостей нет.
        private static List<int>? AskRequiredRuleIds(RuleService ruleService)
        {
            bool dependsOnOtherRules = Prompt.YesNo(
                "Правило зависит от других правил?",
                "Некоторые правила действуют только после других: например,\n" +
                "патент оформляют после получения ИНН и сертификата о русском языке");
            if (!dependsOnOtherRules)
                return null;

            Screen.Progress("Загружаем правила из базы...");
            var existingRules = ruleService.GetAllRules();
            if (existingRules.Count == 0)
            {
                Screen.Warning("В базе пока нет правил — выбрать не из чего, шаг пропущен");
                return null;
            }

            var chosenIndexes = Prompt.ChooseMany(
                "От каких правил зависит",
                existingRules.Select(rule => rule.Name).ToList(),
                "без зависимостей");

            if (chosenIndexes.Count == 0)
                return null;

            return chosenIndexes.Select(index => existingRules[index].Id).ToList();
        }
    }
}
