namespace ProductSelector.Models
{
    public class User
    {
        public int Id { get; set; }
        public string Username { get; set; } = string.Empty;

        public ICollection<UserProduct> UserProducts { get; set; } = new List<UserProduct>();
    }
}
