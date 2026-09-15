using Microsoft.EntityFrameworkCore;
using RulesConstructorForPatent.Entities;

namespace RulesConstructorForPatent.Data
{
    public class RuleRepository(RuleDbContext db) : IRuleRepository
    {
        private readonly RuleDbContext m_db = db;

        public void Add(Rule rule)
        {
            m_db.Rules.Add(rule);
        }

        // Список нужен только для показа пользователю, поэтому изменения не отслеживаются.
        public List<Rule> GetAll()
        {
            return m_db.Rules.AsNoTracking().OrderBy(r => r.Id).ToList();
        }

        // С отслеживанием: на эти правила сошлётся новое, и EF должен знать, что они уже есть в базе.
        public List<Rule> GetByIds(List<int> ids)
        {
            return m_db.Rules.Where(r => ids.Contains(r.Id)).ToList();
        }
    }
}
