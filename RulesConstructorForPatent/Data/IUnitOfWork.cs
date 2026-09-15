using Microsoft.EntityFrameworkCore.Storage;

namespace RulesConstructorForPatent.Data
{
    public interface IUnitOfWork
    {
        IRuleRepository Rules { get; }

        IDbContextTransaction BeginTransaction();
        
        int Save();
    }
}