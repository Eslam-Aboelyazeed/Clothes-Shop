using E_commerceAPI.Models;
using System.ComponentModel.DataAnnotations.Schema;

namespace E_commerceAPI.DTOs.DisplayDTOs
{
    public class ProductDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public ProductTypes Type { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public int Quantity { get; set; }
        public double Rating { get; set; }
        public string? ImageUrl { get; set; }
        public bool IsSpecialOffer { get; set; }
        public decimal? NewPrice { get; set; }
        public DateTime? ExpireDate { get; set; }
        public List<ProductReviewsDTO> productReviews { get; set; }
    }
}
