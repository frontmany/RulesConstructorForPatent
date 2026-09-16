namespace RulesConstructorForPatent.Services
{
    public record RuleDetails(
        int Id,
        string Name,
        IReadOnlyList<string> TargetDocumentNames,
        string GuidanceDescription,
        string Refusal,
        IReadOnlyList<OrganizationDetails> Organizations,
        IReadOnlyList<RuleSummary> RequiredRules,
        IReadOnlyList<ProfileDetails> Profiles);
}
