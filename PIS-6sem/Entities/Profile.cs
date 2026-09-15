namespace PIS_6sem.Entities
{
    public class Profile
    {
        public int Id { get; set; }
        public int Days { get; set; }

        public List<ProfileProperty> Properties { get; set; } = [];
    }
}