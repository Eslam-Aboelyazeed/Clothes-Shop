using System.ComponentModel.DataAnnotations.Schema;

namespace E_commerceAPI.Models
{
    public class Order
    {
        public int Id { get; set; }
        public char Status { get; set; }
        [Column(TypeName = "money")]
        public decimal TotalPrice { get; set; }
        public DateTime OrderDate { get; set; }
        [ForeignKey("user")]
        public string UserId { get; set; }

        public ApplicationUser user { get; set; }

        public List<OrderProducts> orderProducts { get; set; }
        public List<OrderCoupon> orderCoupons { get; set; }
    }
}
