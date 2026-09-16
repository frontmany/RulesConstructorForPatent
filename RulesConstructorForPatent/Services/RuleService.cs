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
                .Select(kind => new ProfileCondition(
                    kind.Name,
                    kind.Options.Select(option => new ProfileOption(option.Id, option.Value)).ToList()))
                .ToList();
        }

        public RuleDetails CreateRule(
            string ruleName,
            List<string> targetDocumentNames,
            string guidanceDescription,
            string refusal,
            List<string> organizationNames,
            List<string> organizationAddresses,
            List<int>? requiredRuleIds,
            List<int> profileDays,
            List<List<int>> profileOptionIds)
        {
            var requiredAccomplishedRules = LoadRequiredRules(requiredRuleIds);
            var profileOptions = LoadProfileOptions(profileOptionIds);
            var ruleBuilder = new RuleBuilder();
            var profileFactory = new ProfileFactory();

            var rule = m_director.Construct(
                ruleName, targetDocumentNames,
                guidanceDescription, refusal,
                organizationNames, organizationAddresses,
                requiredAccomplishedRules,
                profileDays, profileOptions,
                ruleBuilder, profileFactory);

            m_unitOfWork.Rules.Add(rule);
            m_unitOfWork.Save();

            return ToDetails(rule);
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

        // Варианты всех профилей загружаются одним запросом; несуществующий Id — ошибка, а не потерянное условие.
        private List<List<ProfilePropertyOption>> LoadProfileOptions(List<List<int>> profileOptionIds)
        {
            var allIds = profileOptionIds.SelectMany(optionIds => optionIds).Distinct().ToList();
            var optionsById = m_unitOfWork.ProfilePropertyKinds.GetOptionsByIds(allIds)
                .ToDictionary(option => option.Id);

            var missingIds = allIds.Except(optionsById.Keys).ToList();
            if (missingIds.Count > 0)
                throw new InvalidOperationException($"В базе нет вариантов условий с номерами: {string.Join(", ", missingIds)}");

            return profileOptionIds
                .Select(optionIds => optionIds.Select(id => optionsById[id]).OrderBy(option => option.Id).ToList())
                .ToList();
        }

        private static RuleDetails ToDetails(Rule rule)
        {
            return new RuleDetails(
                rule.Id,
                rule.Name,
                rule.TargetDocuments.Select(document => document.Name).ToList(),
                rule.Guidance.Description,
                rule.Guidance.Refusal,
                rule.Guidance.Organizations
                    .Select(organization => new OrganizationDetails(organization.Name, organization.Address))
                    .ToList(),
                rule.RequiredAccomplishedRules?
                    .Select(required => new RuleSummary(required.Id, required.Name))
                    .ToList() ?? [],
                rule.Profiles
                    .Select(profile => new ProfileDetails(
                        profile.Days,
                        profile.Options
                            .GroupBy(option => option.Kind.Name)
                            .Select(group => new ProfileCondition(
                                group.Key,
                                group.Select(option => new ProfileOption(option.Id, option.Value)).ToList()))
                            .ToList()))
                    .ToList());
        }
    }
}
