namespace RulesConstructorForPatent.ConsoleUi
{
    // Ответы на опрос о правиле — в том виде, в каком их принимает RuleService.CreateRule.
    public class RuleSurveyAnswers
    {
        public required string RuleName { get; init; }
        public required List<string> TargetDocumentNames { get; init; }

        public required string GuidanceDescription { get; init; }
        public required string Refusal { get; init; }
        public required List<string> OrganizationNames { get; init; }
        public required List<string> OrganizationAddresses { get; init; }

        // null — правило ни от чего не зависит.
        public required List<int>? RequiredRuleIds { get; init; }

        public required List<int> ProfileDays { get; init; }
        public required List<List<string>> ProfileEntryPurposes { get; init; }
        public required List<List<string>> ProfileCitizenships { get; init; }
        public required List<List<string>> ProfilePropertyNames { get; init; }
        public required List<List<string>> ProfilePropertyValues { get; init; }
    }
}
