namespace E_commerceAPI.DTOs.DisplayDTOs
{
    public class OrderCouponDTO
    {
        public int? OrderId { get; set; }
        public int CouponId { get; set; }
        public decimal TotalPrice { get; set; }
    }
}
