using Microsoft.EntityFrameworkCore.Storage;

namespace RulesConstructorForPatent.Data
{
    public interface IUnitOfWork
    {
        IRuleRepository Rules { get; }
        IProfilePropertyKindRepository ProfilePropertyKinds { get; }

        IDbContextTransaction BeginTransaction();
        
        int Save();
    }
}