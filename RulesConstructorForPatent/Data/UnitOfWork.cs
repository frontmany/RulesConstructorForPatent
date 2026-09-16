using Microsoft.EntityFrameworkCore.Storage;

namespace RulesConstructorForPatent.Data
{
    public class UnitOfWork(RuleDbContext db) : IUnitOfWork
    {
        private readonly RuleDbContext m_db = db;
        private IRuleRepository? m_rules;
        private IProfilePropertyKindRepository? m_profilePropertyKinds;

        // Репозиторий создаётся при первом обращении — там, где он действительно нужен.
        public IRuleRepository Rules => m_rules ??= new RuleRepository(m_db);
        public IProfilePropertyKindRepository ProfilePropertyKinds =>
            m_profilePropertyKinds ??= new ProfilePropertyKindRepository(m_db);

        public IDbContextTransaction BeginTransaction()
        {
            return m_db.Database.BeginTransaction();
        }

        public int Save()
        {
            return m_db.SaveChanges();
        }
    }
}
