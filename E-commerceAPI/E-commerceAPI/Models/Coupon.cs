namespace E_commerceAPI.Models
{
    public class Coupon
    {
        public int Id { get; set; }
        public decimal TotalRedection { get; set; }
        public DateTime ExpireDate { get; set; }
        public int TotalProductsRequired { get; set; }

        public List<OrderCoupon> orderCoupons { get; set; }
    }
}
