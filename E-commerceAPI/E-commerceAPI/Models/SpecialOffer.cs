using System.ComponentModel.DataAnnotations.Schema;

namespace E_commerceAPI.Models
{
    public class SpecialOffer
    {
        [ForeignKey("product")]
        public int Id { get; set; }
        [Column(TypeName = "money")]
        public decimal NewPrice { get; set; }
        public DateTime ExpireDate { get; set; }

        public Product product { get; set; }
    }
}
