using RulesConstructorForPatent.Entities;

namespace RulesConstructorForPatent.Services
{
    public class ProfileFactory
    {
        public Profile CreateProfile(
            int days,
            List<string> propertyNames,
            List<string> propertyValues)
        {
            if (days < 0)
                throw new ArgumentOutOfRangeException(nameof(days), "Срок профиля не может быть отрицательным");

            if (propertyNames.Count != propertyValues.Count)
                throw new ArgumentException("У каждого свойства профиля должно быть одно значение", nameof(propertyValues));

            var profile = new Profile { Days = days };

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
