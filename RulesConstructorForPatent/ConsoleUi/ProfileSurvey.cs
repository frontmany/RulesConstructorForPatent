using RulesConstructorForPatent.Services;

namespace RulesConstructorForPatent.ConsoleUi
{
    public static class ProfileSurvey
    {
        public static ProfileSurveyAnswers Ask(int profileNumber, List<ProfileCondition> conditions)
        {
            Screen.SubHeader($"Профиль {profileNumber}");

            var propertyNames = new List<string>();
            var propertyValues = new List<string>();
            foreach (var condition in conditions)
            {
                foreach (var value in AskValues(condition))
                {
                    propertyNames.Add(condition.Name);
                    propertyValues.Add(value);
                }
            }

            // Срок спрашиваем последним: он относится к категории, заданной условиями выше.
            int days = Prompt.Integer(
                "Срок выполнения, дней",
                "Срок для этой категории мигрантов, считается со дня въезда в РФ\n" +
                "0 — срок не установлен",
                min: 0,
                max: 3650);

            return new ProfileSurveyAnswers
            {
                Days = days,
                PropertyNames = propertyNames,
                PropertyValues = propertyValues
            };
        }

        // Несколько выбранных значений одного условия объединяются через «или».
        private static List<string> AskValues(ProfileCondition condition)
        {
            var chosenIndexes = Prompt.ChooseMany(condition.Name, condition.Values, "любое значение");
            return chosenIndexes.Select(index => condition.Values[index]).ToList();
        }
    }
}
