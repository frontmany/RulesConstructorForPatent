namespace RulesConstructorForPatent.Entities
{
    public class Profile
    {
        public int Id { get; set; }

        // Срок выполнения в днях; 0 — срок не установлен.
        public int Days { get; set; }

        // Выбранные варианты условий: варианты одного вида объединяются через «или», разных видов — через «и».
        public List<ProfilePropertyOption> Options { get; set; } = [];
    }
}
