namespace PIS_6sem.Entities
{
    public class Guidance
    {
        public int Id { get; set; }

        // Что нужно сделать — столбец «Что нужно сделать» таблицы ТЗ.
        public string Description { get; set; } = "";

        // Что стоит попробовать при отказе; пустая строка — не указано.
        public string Refusal { get; set; } = "";

        // Организации, куда обращаться; хотя бы одна (проверяется в RuleBuilder).
        public List<Organization> Organizations { get; set; } = [];

        public int RuleId { get; set; }    
        public Rule Rule { get; set; } = null!;
    }
}