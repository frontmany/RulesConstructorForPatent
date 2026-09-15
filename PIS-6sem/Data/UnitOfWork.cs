using Microsoft.EntityFrameworkCore.Storage;

namespace PIS_6sem.Data
{
    public class UnitOfWork(RuleDbContext db) : IUnitOfWork
    {
        private readonly RuleDbContext m_db = db;
        public IRuleRepository Rules { get; private set; } = new RuleRepository(db);

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