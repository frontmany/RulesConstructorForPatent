using Microsoft.EntityFrameworkCore;
using RulesConstructorForPatent.Entities;

namespace RulesConstructorForPatent.Data
{
    public class ProfilePropertyKindRepository(RuleDbContext db) : IProfilePropertyKindRepository
    {
        private readonly RuleDbContext m_db = db;

        public List<ProfilePropertyKind> GetAll()
        {
            return m_db.ProfilePropertyKinds
                .AsNoTracking()
                .Include(kind => kind.Options.OrderBy(option => option.Id))
                .OrderBy(kind => kind.Id)
                .ToList();
        }
    }
}
