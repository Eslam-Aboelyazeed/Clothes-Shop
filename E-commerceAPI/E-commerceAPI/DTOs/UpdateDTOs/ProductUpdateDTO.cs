using E_commerceAPI.Models;
using System.ComponentModel.DataAnnotations.Schema;

namespace E_commerceAPI.DTOs.UpdateDTOs
{
    public class ProductUpdateDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public ProductTypes Type { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public int Quantity { get; set; }
        public double Rating { get; set; }
    }
}
