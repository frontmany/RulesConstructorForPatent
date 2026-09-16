using RulesConstructorForPatent.Services;

namespace RulesConstructorForPatent.ConsoleUi
{
    public static class ProfileSurvey
    {
        public static ProfileSurveyAnswers Ask(int profileNumber, List<ProfileCondition> conditions)
        {
            Screen.SubHeader($"Профиль {profileNumber}");

            var optionIds = new List<int>();
            foreach (var condition in conditions)
                optionIds.AddRange(AskOptionIds(condition));

            // Срок спрашиваем последним: он относится к категории, заданной условиями выше.
            int days = Prompt.Integer(
                "Срок выполнения, дней",
                "Срок для этой категории мигрантов, считается со дня въезда в РФ\n" +
                "0 — срок не установлен",
                min: 0,
                max: 3650);

            return new ProfileSurveyAnswers { Days = days, OptionIds = optionIds };
        }

        // Несколько выбранных вариантов одного условия объединяются через «или».
        private static IEnumerable<int> AskOptionIds(ProfileCondition condition)
        {
            var values = condition.Options.Select(option => option.Value).ToList();
            var chosenIndexes = Prompt.ChooseMany(condition.Name, values, "любое значение");
            return chosenIndexes.Select(index => condition.Options[index].Id);
        }
    }
}
