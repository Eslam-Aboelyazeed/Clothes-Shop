using E_commerceAPI.DTOs.DisplayDTOs;
using E_commerceAPI.DTOs.InsertDTOs;
using E_commerceAPI.DTOs.UpdateDTOs;
using E_commerceAPI.Models;
using E_commerceAPI.UnitsOfWork;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace E_commerceAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class OrderCouponsController : ControllerBase
    {
        private readonly UnitOfWork unit;

        public OrderCouponsController(UnitOfWork unit)
        {
            this.unit = unit;
        }

        // PUT: api/OrderCoupons/5
        [HttpPut("{oid}")]
        [SwaggerOperation(
        Summary = "This Endpoint updates the specified orderCoupon element",
            Description = ""
        )]
        [SwaggerResponse(404, "The id that was given doesn't exist in the db", Type = typeof(void))]
        [SwaggerResponse(400, "The ids that ware given doesn't equal the ids in the given orderCoupon object", Type = typeof(void))]
        [SwaggerResponse(202, "Something went wrong, please try again later", Type = typeof(void))]
        [SwaggerResponse(204, "Confirms that the orderCoupon element was updated successfully", Type = typeof(void))]
        public async Task<IActionResult> PutOrderCoupon(int oid, OrderCouponUpdateDTO orderCouponDTO)
        {
            var orderCoupon = await unit.OrderCouponRepository.GetElement(oc => oc.CouponId == orderCouponDTO.CouponId && oc.OrderId == orderCouponDTO.OrderId);

            if (orderCoupon == null)
            {
                return NotFound();
            }

            if (oid != orderCoupon.OrderId)
            {
                return BadRequest();
            }

            orderCoupon.TotalPrice = orderCouponDTO.TotalPrice;

            if (unit.OrderCouponRepository.Edit(orderCoupon))
            {
                if (await unit.OrderCouponRepository.SaveChanges()) return NoContent();
            }

            return Accepted();
        }

        // POST: api/OrderCoupons
        [HttpPost]
        [SwaggerOperation(
        Summary = "This Endpoint inserts an orderCoupon element in the db",
            Description = ""
        )]
        [SwaggerResponse(202, "Something went wrong, please try again later", Type = typeof(void))]
        [SwaggerResponse(200, "Returns the inserted orderCoupon element", Type = typeof(OrderCouponDTO))]
        [Consumes("application/json")]
        [Produces("application/json")]
        public async Task<ActionResult<CouponDTO>> PostOrderCoupon(OrderCouponInsertDTO orderCouponInsertDTO)
        {
            var oCoupon = new OrderCoupon()
            {
                CouponId = orderCouponInsertDTO.CouponId,
                OrderId = orderCouponInsertDTO.OrderId,
                TotalPrice = orderCouponInsertDTO.TotalPrice
            };

            if (unit.OrderCouponRepository.Add(oCoupon))
            {
                if (await unit.OrderCouponRepository.SaveChanges())
                {
                    var ocdto = new OrderCouponDTO()
                    {
                        CouponId = oCoupon.CouponId,
                        OrderId = oCoupon.OrderId,
                        TotalPrice = oCoupon.TotalPrice
                    };

                    return Ok(ocdto);
                }
            }

            return Accepted();
        }

        // DELETE: api/OrderCoupons/5
        [HttpDelete("{oid}/{cid}")]
        [SwaggerOperation(
        Summary = "This Endpoint deletes an orderCoupon element from the db",
            Description = ""
        )]
        [SwaggerResponse(404, "The id that was given doesn't exist in the db", Type = typeof(void))]
        [SwaggerResponse(202, "Something went wrong, please try again later", Type = typeof(void))]
        [SwaggerResponse(204, "Confirms that the orderCoupon element was deleted successfully", Type = typeof(void))]
        public async Task<IActionResult> DeleteOrderCoupon(int oid, int cid)
        {
            var orderCoupon = await unit.OrderCouponRepository.GetElement(oc => oc.OrderId == oid && oc.CouponId == cid);
            if (orderCoupon == null)
            {
                return NotFound();
            }

            if (unit.OrderCouponRepository.Delete(orderCoupon))
            {
                if (await unit.OrderCouponRepository.SaveChanges()) return NoContent();
            }

            return Accepted();
        }
    }
}
