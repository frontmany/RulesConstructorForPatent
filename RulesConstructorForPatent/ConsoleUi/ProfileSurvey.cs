using RulesConstructorForPatent.Catalogs;

namespace RulesConstructorForPatent.ConsoleUi
{
    public static class ProfileSurvey
    {
        public static ProfileSurveyAnswers Ask(int profileNumber)
        {
            Screen.SubHeader($"Профиль {profileNumber}");

            var entryPurposes = AskValues(ProfilePropertyCatalog.EntryPurpose, "любая цель");
            var citizenships = AskValues(ProfilePropertyCatalog.Citizenship, "любое гражданство");

            // Цель въезда и гражданство у фабрики — отдельные параметры, остальные условия
            // передаются парами «название свойства — значение».
            var specialStatus = ProfilePropertyCatalog.SpecialStatus;
            var propertyValues = AskValues(specialStatus, "любой статус");
            var propertyNames = Enumerable.Repeat(specialStatus.Name, propertyValues.Count).ToList();

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
                EntryPurposes = entryPurposes,
                Citizenships = citizenships,
                PropertyNames = propertyNames,
                PropertyValues = propertyValues
            };
        }

        // Несколько выбранных значений одного условия объединяются через «или».
        private static List<string> AskValues(ProfilePropertyKind kind, string emptyAnswerMeaning)
        {
            var chosenIndexes = Prompt.ChooseMany(kind.Name, kind.Values, emptyAnswerMeaning);
            return chosenIndexes.Select(index => kind.Values[index]).ToList();
        }
    }
}
