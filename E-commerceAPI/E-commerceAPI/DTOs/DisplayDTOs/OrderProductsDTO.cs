using E_commerceAPI.Models;
using System.ComponentModel.DataAnnotations.Schema;

namespace E_commerceAPI.DTOs.DisplayDTOs
{
    public class OrderProductsDTO
    {
        public int oId { get; set; }
        public int pId { get; set; }
        public string? ProductName { get; set; }
        public ProductTypes? ProductType { get; set; }
        public decimal? ProductPrice { get; set; }
        public string? ProductImage { get; set; }
        public int ProductQuantity { get; set; }
        public decimal TotalPrice { get; set; }
        public int Quantity { get; set; }
    }
}
