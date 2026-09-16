using RulesConstructorForPatent.Entities;

namespace RulesConstructorForPatent.Services
{
    public class ProfileFactory
    {
        public Profile CreateProfile(int days, List<ProfilePropertyOption> options)
        {
            if (days < 0)
                throw new ArgumentOutOfRangeException(nameof(days), "Срок профиля не может быть отрицательным");

            return new Profile { Days = days, Options = options };
        }
    }
}
