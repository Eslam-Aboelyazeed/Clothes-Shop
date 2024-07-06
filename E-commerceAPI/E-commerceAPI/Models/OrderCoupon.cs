using System.ComponentModel.DataAnnotations.Schema;

namespace E_commerceAPI.Models
{
    public class OrderCoupon
    {
        [ForeignKey("order")]
        public int OrderId { get; set; }
        [ForeignKey("coupon")]
        public int CouponId { get; set; }
        [Column(TypeName = "money")]
        public decimal TotalPrice { get; set; }

        public Order order { get; set; }
        public Coupon coupon { get; set; }
    }
}
