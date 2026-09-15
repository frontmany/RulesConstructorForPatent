using PIS_6sem.Entities;

namespace PIS_6sem.Services
{
    public class ProfileFactory
    {
        public Profile CreateProfile(
            int days,
            List<string> purposeNames,
            List<string> citizenshipNames,
            List<string> propertyNames,
            List<string> propertyValues)
        {
            var profile = new Profile { Days = days };

            foreach (var purpose in purposeNames)
            {
                profile.Properties.Add(new ProfileProperty
                {
                    Name = "Цель въезда",
                    Value = purpose
                });
            }

            foreach (var citizenship in citizenshipNames)
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