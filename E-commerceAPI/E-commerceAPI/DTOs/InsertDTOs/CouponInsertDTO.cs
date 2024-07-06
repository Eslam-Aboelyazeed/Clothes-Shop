namespace E_commerceAPI.DTOs.InsertDTOs
{
    public class CouponInsertDTO
    {
        public decimal TotalRedection { get; set; }
        public DateTime ExpireDate { get; set; }
        public int TotalProductsRequired { get; set; }
    }
}
