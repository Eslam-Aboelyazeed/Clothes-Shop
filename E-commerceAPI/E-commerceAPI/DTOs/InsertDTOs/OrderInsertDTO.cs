using System.ComponentModel.DataAnnotations.Schema;

namespace E_commerceAPI.DTOs.InsertDTOs
{
    public class OrderInsertDTO
    {
        public decimal TotalPrice { get; set; }
        public string UserId { get; set; }
    }
}
