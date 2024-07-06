namespace E_commerceAPI.DTOs.UpdateDTOs
{
    public class CouponUpdateDTO
    {
        public int Id { get; set; }
        public decimal TotalRedection { get; set; }
        public DateTime ExpireDate { get; set; }
        public int TotalProductsRequired { get; set; }
    }
}
