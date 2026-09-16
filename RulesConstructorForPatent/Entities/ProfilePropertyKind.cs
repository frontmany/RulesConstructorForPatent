namespace RulesConstructorForPatent.Entities
{
    // Вид условия профиля, например «Гражданство», и значения, из которых его выбирают.
    public class ProfilePropertyKind
    {
        public int Id { get; set; }
        public string Name { get; set; } = "";

        public List<ProfilePropertyOption> Options { get; set; } = [];
    }
}
