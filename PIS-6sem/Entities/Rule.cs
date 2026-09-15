namespace PIS_6sem.Entities
{
    public class Rule
    {
        public int Id { get; set; }
        public string Name { get; set; } = "";

        public List<Profile> Profiles { get; set; } = [];
        public List<TargetDocument> TargetDocuments { get; set; } = [];
        public Guidance Guidance { get; set; } = new();
    }
}