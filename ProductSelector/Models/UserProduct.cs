using System.ComponentModel.DataAnnotations.Schema;

namespace ProductSelector.Models
{
    public class UserProduct
    {
        public int Id { get; set; }

        public int UserId { get; set; }
        [ForeignKey("UserId")]
        public User User { get; set; }

        public int ProductIdRef { get; set; }

        [ForeignKey("ProductIdRef")]
        public Product Product { get; set; }

        public DateTime SelectedAt { get; set; } = DateTime.UtcNow;
    }
}
