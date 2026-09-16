namespace RulesConstructorForPatent.Services
{
    public record ProfileDetails(int Days, IReadOnlyList<ProfileCondition> Conditions);
}
