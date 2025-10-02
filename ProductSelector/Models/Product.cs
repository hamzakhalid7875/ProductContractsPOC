namespace ProductSelector.Models
{
    public class Product
    {
        public int Id { get; set; }
        public string ProductId { get; set; } = string.Empty;  // from contract.yaml
        public string DisplayName { get; set; } = string.Empty;
        public string Version { get; set; } = string.Empty;
        public string ProjectPath { get; set; } = string.Empty;

        public ICollection<UserProduct> UserProducts { get; set; } = new List<UserProduct>();
    }
}
