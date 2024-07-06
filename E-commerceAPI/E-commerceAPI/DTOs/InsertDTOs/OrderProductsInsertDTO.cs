using System.ComponentModel.DataAnnotations.Schema;

namespace E_commerceAPI.DTOs.InsertDTOs
{
    public class OrderProductsInsertDTO
    {
        public int OrderId { get; set; }
        public int ProductId { get; set; }
        public decimal TotalPrice { get; set; }
        public int Quantity { get; set; }
    }
}
