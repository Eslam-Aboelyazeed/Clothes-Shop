namespace E_commerceAPI.DTOs.UpdateDTOs
{
    public class OrderCouponUpdateDTO
    {
        public int OrderId { get; set; }
        public int CouponId { get; set; }
        public decimal TotalPrice { get; set; }
    }
}
