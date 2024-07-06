using System.ComponentModel.DataAnnotations.Schema;

namespace E_commerceAPI.Models
{
    public class ProductReviews
    {
        [ForeignKey("product")]
        public int ProductId { get; set; }
        [ForeignKey("user")]
        public string UserId { get; set; }
        public int Rating { get; set; }
        public string Review { get; set; }

        public Product product { get; set; }
        public ApplicationUser user { get; set; }
    }
}
