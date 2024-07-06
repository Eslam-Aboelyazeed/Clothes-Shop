namespace E_commerceAPI.DTOs.UpdateDTOs
{
    public class OrderUpdateDTO
    {
        public int Id { get; set; }
        public decimal TotalPrice { get; set; }
        public string Status { get; set; }
    }
}
