using PIS_6sem.Entities;

namespace PIS_6sem.Data
{
    public class RuleRepository(RuleDbContext db) : IRuleRepository
    {
        private readonly RuleDbContext _db = db;

        public void Add(Rule rule)
        {
            _db.Rules.Add(rule);
        }
    }
}