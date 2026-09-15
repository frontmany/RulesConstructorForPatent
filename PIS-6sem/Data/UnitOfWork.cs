using Microsoft.EntityFrameworkCore.Storage;

namespace PIS_6sem.Data
{
    public class UnitOfWork(RuleDbContext db) : IUnitOfWork
    {
        private readonly RuleDbContext m_db = db;
        private IRuleRepository? m_rules;

        // Репозиторий создаётся при первом обращении — там, где он действительно нужен.
        public IRuleRepository Rules => m_rules ??= new RuleRepository(m_db);

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
