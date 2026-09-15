using PIS_6sem.Entities;

namespace PIS_6sem.Services
{
    public class RuleDirector
    {
        public Rule Construct(
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
            List<List<string>> propertyValues,
            RuleBuilder ruleBuilder,
            ProfileFactory profileFactory)
        {
            ruleBuilder.Reset();
            ruleBuilder.AddName(ruleName);

            foreach (var doc in targetDocs)
                ruleBuilder.AddTargetDocument(doc);

            for (int i = 0; i < daysList.Count; i++)
            {
                var profile = profileFactory.CreateProfile(
                    daysList[i],
                    purposeNamesList[i],
                    citizenshipNamesList[i],
                    propertyNames[i],
                    propertyValues[i]);

                ruleBuilder.AddProfile(profile);
            }

            ruleBuilder.AddGuidance(guidanceDescription, refusal, orgNames, orgAddresses);

            return ruleBuilder.GetResult();
        }
    }
}