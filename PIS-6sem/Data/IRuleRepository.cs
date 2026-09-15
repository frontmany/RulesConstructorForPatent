using PIS_6sem.Entities;

namespace PIS_6sem.Data
{
    public interface IRuleRepository
    {
        void Add(Rule rule);

        List<Rule> GetAll();

        List<Rule> GetByIds(List<int> ids);
    }
}
