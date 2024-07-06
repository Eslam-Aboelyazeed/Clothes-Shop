using System.ComponentModel.DataAnnotations.Schema;

namespace E_commerceAPI.Models
{
    public class ContactUs
    {
        public int Id { get; set; }
        [ForeignKey("user")]
        public string UserId { get; set; }
        public string Email { get; set; }
        public string Message { get; set; }
        public DateTime Date { get; set; }
        public char Status { get; set; }

        public ApplicationUser user { get; set; }
    }
}
