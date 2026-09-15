using PIS_6sem.Entities;

namespace PIS_6sem.Services
{
    public class RuleBuilder
    {
        private string m_name = "";
        private readonly List<string> m_targetDocumentNames = [];
        private readonly List<Profile> m_profiles = [];
        private Guidance? m_guidance;

        public void Reset()
        {
            m_name = "";
            m_targetDocumentNames.Clear();
            m_profiles.Clear();
            m_guidance = null;
        }

        public void SetName(string name) => m_name = name;

        public void AddTargetDocument(string documentName)
            => m_targetDocumentNames.Add(documentName);

        public void AddProfile(Profile profile)
            => m_profiles.Add(profile);

        public void SetGuidance(
            string description,
            string refusal,
            List<string> organizationNames,
            List<string> organizationAddresses)
        {
            var guidance = new Guidance
            {
                Description = description,
                Refusal = refusal
            };

            int count = Math.Min(organizationNames.Count, organizationAddresses.Count);
            for (int i = 0; i < count; i++)
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
            var rule = new Rule { Name = m_name };

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
