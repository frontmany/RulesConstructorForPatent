using PIS_6sem.Entities;

namespace PIS_6sem.Services
{
    public class RuleBuilder
    {
        private string m_name = "";
        private readonly List<string> m_targetDocNames = [];
        private readonly List<Profile> m_profiles = [];
        private Guidance? m_guidance;

        public void Reset()
        {
            m_name = "";
            m_targetDocNames.Clear();
            m_profiles.Clear();
            m_guidance = null;
        }

        public void AddName(string name) => m_name = name;

        public void AddTargetDocument(string targetDoc)
            => m_targetDocNames.Add(targetDoc);

        public void AddProfile(Profile profile)
            => m_profiles.Add(profile);

        public void AddGuidance(
            string description,
            string refusal,
            List<string> orgNames,
            List<string> orgAddresses)
        {
            var guidance = new Guidance
            {
                Description = description,
                Refusal = refusal
            };

            int count = Math.Min(orgNames.Count, orgAddresses.Count);
            for (int i = 0; i < count; i++)
            {
                guidance.Organizations.Add(new Organization
                {
                    Name = orgNames[i],
                    Address = orgAddresses[i]
                });
            }

            m_guidance = guidance;
        }

        public Rule GetResult()
        {
            var rule = new Rule { Name = m_name };

            foreach (var profile in m_profiles)
                rule.Profiles.Add(profile);

            foreach (var docName in m_targetDocNames)
            {
                rule.TargetDocuments.Add(new TargetDocument { Name = docName });
            }

            if (m_guidance != null)
                rule.Guidance = m_guidance;

            return rule;
        }
    }
}