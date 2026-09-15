using PIS_6sem.Entities;

namespace PIS_6sem.Services
{
    public class RuleBuilder
    {
        private string m_name = "";
        private readonly List<string> m_targetDocumentNames = [];
        private readonly List<Profile> m_profiles = [];
        private List<Rule>? m_requiredAccomplishedRules;
        private Guidance? m_guidance;

        public void Reset()
        {
            m_name = "";
            m_targetDocumentNames.Clear();
            m_profiles.Clear();
            m_requiredAccomplishedRules = null;
            m_guidance = null;
        }

        public void SetName(string name) => m_name = name;

        public void AddTargetDocument(string documentName)
            => m_targetDocumentNames.Add(documentName);

        public void AddProfile(Profile profile)
            => m_profiles.Add(profile);

        // Список заводится только при первой зависимости: если их нет, у правила останется null.
        public void AddRequiredAccomplishedRule(Rule requiredRule)
        {
            m_requiredAccomplishedRules ??= [];
            m_requiredAccomplishedRules.Add(requiredRule);
        }

        public void SetGuidance(
            string description,
            string refusal,
            List<string> organizationNames,
            List<string> organizationAddresses)
        {
            if (organizationNames.Count == 0)
                throw new ArgumentException("В руководстве должна быть хотя бы одна организация", nameof(organizationNames));

            if (organizationNames.Count != organizationAddresses.Count)
                throw new ArgumentException("Названий и адресов организаций должно быть поровну", nameof(organizationAddresses));

            var guidance = new Guidance
            {
                Description = description,
                Refusal = refusal
            };

            for (int i = 0; i < organizationNames.Count; i++)
            {
                guidance.Organizations.Add(new Organization
                {
                    Name = organizationNames[i],
                    Address = organizationAddresses[i]
                });
            }

            m_guidance = guidance;
        }

        public Rule GetResult()
        {
            if (string.IsNullOrWhiteSpace(m_name))
                throw new InvalidOperationException("У правила должно быть название");

            if (m_targetDocumentNames.Count == 0)
                throw new InvalidOperationException("У правила должен быть хотя бы один целевой документ");

            var rule = new Rule
            {
                Name = m_name,
                RequiredAccomplishedRules = m_requiredAccomplishedRules?.ToList()
            };

            foreach (var profile in m_profiles)
                rule.Profiles.Add(profile);

            foreach (var documentName in m_targetDocumentNames)
                rule.TargetDocuments.Add(new TargetDocument { Name = documentName });

            if (m_guidance != null)
                rule.Guidance = m_guidance;

            return rule;
        }
    }
}
