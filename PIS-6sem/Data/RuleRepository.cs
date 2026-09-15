using PIS_6sem.Entities;

namespace PIS_6sem.Data
{
    public class RuleRepository(RuleDbContext db) : IRuleRepository
    {
        private readonly RuleDbContext m_db = db;

        public void Add(Rule rule)
        {
            m_db.Rules.Add(rule);
        }
    }
}