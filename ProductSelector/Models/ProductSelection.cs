using System.ComponentModel.DataAnnotations;

namespace ProductSelector.Models
{
    public class ProductSelection
    {
        [Key]
        public int Id { get; set; }

        public string ProductId { get; set; } = string.Empty;

        public string DisplayName { get; set; } = string.Empty;

        public string Version { get; set; } = string.Empty;

        public DateTime SelectedAt { get; set; } = DateTime.UtcNow;
    }
}
