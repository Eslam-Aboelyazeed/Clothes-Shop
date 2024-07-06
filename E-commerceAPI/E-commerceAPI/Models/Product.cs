using System.ComponentModel.DataAnnotations.Schema;

namespace E_commerceAPI.Models
{
    public class Product
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public ProductTypes Type { get; set; }
        public string Description { get; set; }
        [Column(TypeName = "money")]
        public decimal Price { get; set; }
        public int Quantity { get; set; }
        public double Rating { get; set; }
        public string? ImageUrl { get; set; }

        public List<SpecialOffer> specialOffers { get; set; }
        public List<OrderProducts> orderProducts { get; set; }
        public List<ProductReviews> productReviews { get; set; }
    }
}
