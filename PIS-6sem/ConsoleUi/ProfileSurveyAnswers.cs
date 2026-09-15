namespace PIS_6sem.ConsoleUi
{
    // Ответы об одном профиле — аргументы для ProfileFactory.CreateProfile.
    public class ProfileSurveyAnswers
    {
        public required int Days { get; init; }
        public required List<string> EntryPurposes { get; init; }
        public required List<string> Citizenships { get; init; }
        public required List<string> PropertyNames { get; init; }
        public required List<string> PropertyValues { get; init; }
    }
}
