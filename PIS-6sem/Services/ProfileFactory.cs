using PIS_6sem.Entities;

namespace PIS_6sem.Services
{
    public class ProfileFactory
    {
        public Profile CreateProfile(
            int days,
            List<string> entryPurposes,
            List<string> citizenships,
            List<string> propertyNames,
            List<string> propertyValues)
        {
            var profile = new Profile { Days = days };

            foreach (var entryPurpose in entryPurposes)
            {
                profile.Properties.Add(new ProfileProperty
                {
                    Name = "Цель въезда",
                    Value = entryPurpose
                });
            }

            foreach (var citizenship in citizenships)
            {
                profile.Properties.Add(new ProfileProperty
                {
                    Name = "Гражданство",
                    Value = citizenship
                });
            }

            for (int i = 0; i < propertyNames.Count; i++)
            {
                profile.Properties.Add(new ProfileProperty
                {
                    Name = propertyNames[i],
                    Value = propertyValues[i]
                });
            }

            return profile;
        }
    }
}
