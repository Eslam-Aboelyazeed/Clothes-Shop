using E_commerceAPI.Models;

namespace E_commerceAPI.DTOs.DisplayDTOs
{
    public class ProductPageDTO
    {
        public IEnumerable<ProductDTO> products { get; set; }
        public int TotalCount { get; set; }
        public int TotalPages { get; set; }
    }
}
