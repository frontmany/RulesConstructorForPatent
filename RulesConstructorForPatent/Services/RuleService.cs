using RulesConstructorForPatent.Data;
using RulesConstructorForPatent.Entities;

namespace RulesConstructorForPatent.Services
{
    public class RuleService(IUnitOfWork unitOfWork, RuleDirector director)
    {
        private readonly IUnitOfWork m_unitOfWork = unitOfWork;
        private readonly RuleDirector m_director = director;

        public List<RuleSummary> GetAllRules()
        {
            return m_unitOfWork.Rules.GetAll()
                .Select(rule => new RuleSummary(rule.Id, rule.Name))
                .ToList();
        }

        public List<ProfileCondition> GetProfileConditions()
        {
            return m_unitOfWork.ProfilePropertyKinds.GetAll()
                .Select(kind => new ProfileCondition(kind.Name, kind.Options.Select(option => option.Value).ToList()))
                .ToList();
        }

        public Rule CreateRule(
            string ruleName,
            List<string> targetDocumentNames,
            string guidanceDescription,
            string refusal,
            List<string> organizationNames,
            List<string> organizationAddresses,
            List<int>? requiredRuleIds,
            List<int> profileDays,
            List<List<string>> profilePropertyNames,
            List<List<string>> profilePropertyValues)
        {
            var requiredAccomplishedRules = LoadRequiredRules(requiredRuleIds);
            var ruleBuilder = new RuleBuilder();
            var profileFactory = new ProfileFactory();

            var rule = m_director.Construct(
                ruleName, targetDocumentNames,
                guidanceDescription, refusal,
                organizationNames, organizationAddresses,
                requiredAccomplishedRules,
                profileDays, profilePropertyNames, profilePropertyValues,
                ruleBuilder, profileFactory);

            using (var transaction = m_unitOfWork.BeginTransaction())
            {
                m_unitOfWork.Rules.Add(rule);
                m_unitOfWork.Save();
                transaction.Commit();
            }

            return rule;
        }

        // Пользователь выбирает зависимости по Id, а билдеру нужны сами правила из базы.
        private List<Rule>? LoadRequiredRules(List<int>? requiredRuleIds)
        {
            if (requiredRuleIds == null)
                return null;

            var requiredRules = m_unitOfWork.Rules.GetByIds(requiredRuleIds);

            var missingIds = requiredRuleIds.Except(requiredRules.Select(r => r.Id)).ToList();
            if (missingIds.Count > 0)
                throw new InvalidOperationException($"В базе нет правил с номерами: {string.Join(", ", missingIds)}");

            return requiredRules;
        }
    }
}
