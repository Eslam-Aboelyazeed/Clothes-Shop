using Microsoft.AspNetCore.Identity;

namespace E_commerceAPI.Models
{
    public class ApplicationUser:IdentityUser
    {
        public string Address { get; set; }

        public List<Order> orders { get; set; }
        public List<ProductReviews> productReviews { get; set; }
        public List<ContactUs> contactUsMessages { get; set; }
    }
}
