using Microsoft.EntityFrameworkCore.Storage;

namespace PIS_6sem.Data
{
    public interface IUnitOfWork
    {
        IRuleRepository Rules { get; }

        IDbContextTransaction BeginTransaction();
        
        int Save();
    }
}