namespace RulesConstructorForPatent.Entities
{
    public class Profile
    {
        public int Id { get; set; }

        // Срок выполнения в днях; 0 — срок не установлен.
        public int Days { get; set; }

        // Условия профиля: свойства с одинаковым Name объединяются через «или», с разными — через «и».
        public List<ProfileProperty> Properties { get; set; } = [];
    }
}
