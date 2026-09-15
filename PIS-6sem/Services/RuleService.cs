using PIS_6sem.Data;
using PIS_6sem.Entities;

namespace PIS_6sem.Services
{
    public class RuleService(IUnitOfWork unitOfWork, RuleDirector director)
    {
        private readonly IUnitOfWork m_unitOfWork = unitOfWork;
        private readonly RuleDirector m_director = director;

        public Rule CreateRule(
            string ruleName,
            List<string> targetDocumentNames,
            string guidanceDescription,
            string refusal,
            List<string> organizationNames,
            List<string> organizationAddresses,
            List<int> profileDays,
            List<List<string>> profileEntryPurposes,
            List<List<string>> profileCitizenships,
            List<List<string>> profilePropertyNames,
            List<List<string>> profilePropertyValues)
        {
            var ruleBuilder = new RuleBuilder();
            var profileFactory = new ProfileFactory();

            var rule = m_director.Construct(
                ruleName, targetDocumentNames,
                guidanceDescription, refusal,
                organizationNames, organizationAddresses,
                profileDays, profileEntryPurposes, profileCitizenships,
                profilePropertyNames, profilePropertyValues,
                ruleBuilder, profileFactory);

            using (var transaction = m_unitOfWork.BeginTransaction())
            {
                m_unitOfWork.Rules.Add(rule);
                m_unitOfWork.Save();
                transaction.Commit();
            }

            return rule;
        }
    }
}
