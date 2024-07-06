using System.ComponentModel.DataAnnotations.Schema;

namespace E_commerceAPI.DTOs.DisplayDTOs
{
    public class OrderDTO
    {
        public int Id { get; set; }
        public string Status { get; set; }
        public decimal TotalPrice { get; set; }
        public DateTime OrderDate { get; set; }
        public string UserId { get; set; }
        public string UserName { get; set; }
        public string UserEmail { get; set; }
        public List<OrderProductsDTO> orderProducts { get; set; }
        public List<OrderCouponDTO> orderCoupons { get; set; }
    }
}
