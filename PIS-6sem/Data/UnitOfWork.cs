using Microsoft.EntityFrameworkCore.Storage;

namespace PIS_6sem.Data
{
    public class UnitOfWork(RuleDbContext db) : IUnitOfWork
    {
        private readonly RuleDbContext _db = db;
        public IRuleRepository Rules { get; private set; } = new RuleRepository(db);

        public IDbContextTransaction BeginTransaction()
        {
            return _db.Database.BeginTransaction();
        }

        public int Save()
        {
            return _db.SaveChanges();
        }
    }
}