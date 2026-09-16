namespace RulesConstructorForPatent.Data
{
    public interface IUnitOfWork
    {
        IRuleRepository Rules { get; }
        IProfilePropertyKindRepository ProfilePropertyKinds { get; }

        int Save();
    }
}
