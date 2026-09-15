using RulesConstructorForPatent.Entities;

namespace RulesConstructorForPatent.Data
{
    public interface IRuleRepository
    {
        void Add(Rule rule);

        List<Rule> GetAll();

        List<Rule> GetByIds(List<int> ids);
    }
}
