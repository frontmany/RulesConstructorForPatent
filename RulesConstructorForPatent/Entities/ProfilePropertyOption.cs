namespace RulesConstructorForPatent.Entities
{
    public class ProfilePropertyOption
    {
        public int Id { get; set; }
        public string Value { get; set; } = "";

        public int KindId { get; set; }
        public ProfilePropertyKind Kind { get; set; } = null!;
    }
}
