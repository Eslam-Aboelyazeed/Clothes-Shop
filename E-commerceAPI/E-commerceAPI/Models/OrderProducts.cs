using System.ComponentModel.DataAnnotations.Schema;

namespace E_commerceAPI.Models
{
    public class OrderProducts
    {
        [ForeignKey("order")]
        public int OrderId { get; set; }
        [ForeignKey("product")]
        public int ProductId { get; set; }
        [Column(TypeName = "money")]
        public decimal TotalPrice { get; set; }
        public int Quantity { get; set; }

        public Order order { get; set; }
        public Product product { get; set; }
    }
}
