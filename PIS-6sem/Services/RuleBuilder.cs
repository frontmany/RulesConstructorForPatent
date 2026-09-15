using PIS_6sem.Entities;

namespace PIS_6sem.Services
{
    public class RuleBuilder
    {
        private string _name = "";
        private readonly List<string> _targetDocNames = [];
        private readonly List<Profile> _profiles = [];
        private Guidance? _guidance;

        public void Reset()
        {
            _name = "";
            _targetDocNames.Clear();
            _profiles.Clear();
            _guidance = null;
        }

        public void AddName(string name) => _name = name;

        public void AddTargetDocument(string targetDoc)
            => _targetDocNames.Add(targetDoc);

        public void AddProfile(Profile profile)
            => _profiles.Add(profile);

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

            _guidance = guidance;
        }

        public Rule GetResult()
        {
            var rule = new Rule { Name = _name };

            foreach (var profile in _profiles)
                rule.Profiles.Add(profile);

            foreach (var docName in _targetDocNames)
            {
                rule.TargetDocuments.Add(new TargetDocument { Name = docName });
            }

            if (_guidance != null)
                rule.Guidance = _guidance;

            return rule;
        }
    }
}