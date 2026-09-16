using RulesConstructorForPatent.Entities;

namespace RulesConstructorForPatent.Data
{
    public interface IProfilePropertyKindRepository
    {
        List<ProfilePropertyKind> GetAll();

        List<ProfilePropertyOption> GetOptionsByIds(List<int> ids);
    }
}
