using RulesConstructorForPatent.Entities;

namespace RulesConstructorForPatent.Services
{
    public class RuleDirector
    {
        public Rule Construct(
            string ruleName,
            List<string> targetDocumentNames,
            string guidanceDescription,
            string refusal,
            List<string> organizationNames,
            List<string> organizationAddresses,
            List<Rule>? requiredAccomplishedRules,
            List<int> profileDays,
            List<List<ProfilePropertyOption>> profileOptions,
            RuleBuilder ruleBuilder,
            ProfileFactory profileFactory)
        {
            ruleBuilder.Reset();
            ruleBuilder.SetName(ruleName);

            foreach (var documentName in targetDocumentNames)
                ruleBuilder.AddTargetDocument(documentName);

            if (requiredAccomplishedRules != null)
            {
                foreach (var requiredRule in requiredAccomplishedRules)
                    ruleBuilder.AddRequiredAccomplishedRule(requiredRule);
            }

            for (int i = 0; i < profileDays.Count; i++)
            {
                var profile = profileFactory.CreateProfile(profileDays[i], profileOptions[i]);
                ruleBuilder.AddProfile(profile);
            }

            ruleBuilder.SetGuidance(guidanceDescription, refusal, organizationNames, organizationAddresses);

            return ruleBuilder.GetResult();
        }
    }
}
