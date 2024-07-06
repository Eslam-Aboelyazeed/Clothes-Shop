using System.ComponentModel.DataAnnotations.Schema;

namespace E_commerceAPI.DTOs.InsertDTOs
{
    public class OrderCouponInsertDTO
    {
        public int OrderId { get; set; }
        public int CouponId { get; set; }
        public decimal TotalPrice { get; set; }
    }
}
