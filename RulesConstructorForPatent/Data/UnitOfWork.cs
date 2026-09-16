namespace RulesConstructorForPatent.Data
{
    public class UnitOfWork(RuleDbContext db) : IUnitOfWork
    {
        private readonly RuleDbContext m_db = db;
        private IRuleRepository? m_rules;
        private IProfilePropertyKindRepository? m_profilePropertyKinds;

        public IRuleRepository Rules => m_rules ??= new RuleRepository(m_db);
        public IProfilePropertyKindRepository ProfilePropertyKinds =>
            m_profilePropertyKinds ??= new ProfilePropertyKindRepository(m_db);

        public int Save()
        {
            return m_db.SaveChanges();
        }
    }
}
