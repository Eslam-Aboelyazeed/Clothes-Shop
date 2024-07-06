namespace E_commerceAPI.DTOs.DisplayDTOs
{
    public class CouponDTO
    {
        public int Id { get; set; }
        public decimal TotalRedection { get; set; }
        public DateTime ExpireDate { get; set; }
        public int TotalProductsRequired { get; set; }
    }
}
