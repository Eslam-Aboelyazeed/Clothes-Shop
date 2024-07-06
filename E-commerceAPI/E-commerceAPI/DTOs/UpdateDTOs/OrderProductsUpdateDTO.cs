namespace E_commerceAPI.DTOs.UpdateDTOs
{
    public class OrderProductsUpdateDTO
    {
        public int OrderId { get; set; }
        public int ProductId { get; set; }
        public decimal TotalPrice { get; set; }
        public int Quantity { get; set; }
    }
}
