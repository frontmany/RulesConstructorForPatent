using PIS_6sem.Data;
using PIS_6sem.Entities;

namespace PIS_6sem.Services
{
    public class ServiceRule(IUnitOfWork unitOfWork, RuleDirector director)
    {
        private readonly IUnitOfWork m_unitOfWork = unitOfWork;
        private readonly RuleDirector m_director = director;

        public Rule CreateRule(
            string ruleName,
            List<string> targetDocs,
            string guidanceDescription,
            string refusal,
            List<string> orgNames,
            List<string> orgAddresses,
            List<int> daysList,
            List<List<string>> purposeNamesList,
            List<List<string>> citizenshipNamesList,
            List<List<string>> propertyNames,
            List<List<string>> propertyValues)
        {
            var ruleBuilder = new RuleBuilder();
            var profileFactory = new ProfileFactory();

            var rule = m_director.Construct(
                ruleName, targetDocs,
                guidanceDescription, refusal,
                orgNames, orgAddresses,
                daysList, purposeNamesList, citizenshipNamesList,
                propertyNames, propertyValues,
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