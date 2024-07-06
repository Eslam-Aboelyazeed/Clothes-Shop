using E_commerceAPI.DTOs.DisplayDTOs;
using E_commerceAPI.DTOs.InsertDTOs;
using E_commerceAPI.DTOs.UpdateDTOs;
using E_commerceAPI.Models;
using E_commerceAPI.UnitsOfWork;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System.Security.Claims;

namespace E_commerceAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class CouponController : ControllerBase
    {
        private readonly UnitOfWork unit;

        public CouponController(UnitOfWork unit)
        {
            this.unit = unit;
        }

        // GET: api/Coupon
        [HttpGet]
        [SwaggerOperation(
        Summary = "This Endpoint returns a list of coupon elements",
            Description = ""
        )]
        [SwaggerResponse(404, "There weren't any coupon elements in the database", Type = typeof(void))]
        [SwaggerResponse(200, "Returns A list of coupon elements", Type = typeof(List<ContactUsDTO>))]
        public async Task<ActionResult<IEnumerable<CouponDTO>>> GetCouponList()
        {
            var list = await unit.CouponRepository.GetAllElements();

            if (list == null || list.Count == 0) return NotFound();

            var l = list.Select(c => new CouponDTO { Id = c.Id, ExpireDate = c.ExpireDate, TotalProductsRequired = c.TotalProductsRequired, TotalRedection = c.TotalRedection }).ToList();

            return Ok(l);
        }

        // GET: api/Coupon/5
        [HttpGet("{id}")]
        [SwaggerOperation(
        Summary = "This Endpoint returns the specified coupon element",
            Description = ""
        )]
        [SwaggerResponse(404, "The id that was given doesn't exist in the db", Type = typeof(void))]
        [SwaggerResponse(200, "Returns the specified coupon element", Type = typeof(CouponDTO))]
        public async Task<ActionResult<CouponDTO>> GetCouponElement(int id)
        {
            var coupon = await unit.CouponRepository.GetElementWithoutTracking(c => c.Id == id);

            if (coupon == null)
            {
                return NotFound();
            }

            var co = new CouponDTO()
            {
                Id = coupon.Id,
                ExpireDate = coupon.ExpireDate,
                TotalProductsRequired = coupon.TotalProductsRequired,
                TotalRedection = coupon.TotalRedection
            };

            return Ok(co);
        }

        // PUT: api/Coupon/5
        [HttpPut("{id}")]
        [SwaggerOperation(
        Summary = "This Endpoint updates the specified coupon element",
            Description = ""
        )]
        [SwaggerResponse(404, "The id that was given doesn't exist in the db", Type = typeof(void))]
        [SwaggerResponse(400, "The id that was given doesn't equal the id in the given coupon object", Type = typeof(void))]
        [SwaggerResponse(202, "Something went wrong, please try again later", Type = typeof(void))]
        [SwaggerResponse(204, "Confirms that the coupon element was updated successfully", Type = typeof(void))]
        [Authorize(Roles ="Admin")]
        public async Task<IActionResult> PutCoupon(int id, CouponUpdateDTO couponDTO)
        {
            var coupon = await unit.CouponRepository.GetElement(c => c.Id == couponDTO.Id);

            if (coupon == null)
            {
                return NotFound();
            }

            if (id != coupon.Id)
            {
                return BadRequest();
            }

            coupon.ExpireDate = couponDTO.ExpireDate;
            coupon.TotalProductsRequired = couponDTO.TotalProductsRequired;
            coupon.TotalRedection = couponDTO.TotalRedection;

            if (unit.CouponRepository.Edit(coupon))
            {
                if (await unit.CouponRepository.SaveChanges()) return NoContent();
            }

            return Accepted();
        }

        // POST: api/Coupon
        [HttpPost]
        [SwaggerOperation(
        Summary = "This Endpoint inserts a coupon element in the db",
            Description = ""
        )]
        [SwaggerResponse(202, "Something went wrong, please try again later", Type = typeof(void))]
        [SwaggerResponse(201, "Returns the inserted coupon element and the url you can use to get it", Type = typeof(CouponDTO))]
        [Consumes("application/json")]
        [Produces("application/json")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<CouponDTO>> PostCoupon(CouponInsertDTO coupon)
        {
            var c = new Coupon()
            {
                ExpireDate = coupon.ExpireDate,
                TotalProductsRequired = coupon.TotalProductsRequired,
                TotalRedection = coupon.TotalRedection
            };

            if (unit.CouponRepository.Add(c))
            {
                if (await unit.CouponRepository.SaveChanges())
                {
                    var co = await unit.CouponRepository.GetElementWithoutTracking(cou => cou.Id == c.Id);

                    if (co != null)
                    {
                        var codto = new CouponDTO()
                        {
                            Id = co.Id,
                            ExpireDate= co.ExpireDate,
                            TotalProductsRequired= co.TotalProductsRequired,
                            TotalRedection = co.TotalRedection
                        };

                        return CreatedAtAction("GetCouponElement", new { id = c.Id }, codto);
                    }
                }
            }

            return Accepted();
        }

        // DELETE: api/Coupon/5
        [HttpDelete("{id}")]
        [SwaggerOperation(
        Summary = "This Endpoint deletes a coupon element from the db",
            Description = ""
        )]
        [SwaggerResponse(404, "The id that was given doesn't exist in the db", Type = typeof(void))]
        [SwaggerResponse(202, "Something went wrong, please try again later", Type = typeof(void))]
        [SwaggerResponse(204, "Confirms that the coupon element was deleted successfully", Type = typeof(void))]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteCoupon(int id)
        {
            var coupon = await unit.CouponRepository.GetElement(c => c.Id == id);
            if (coupon == null)
            {
                return NotFound();
            }

            if (unit.CouponRepository.Delete(coupon))
            {
                if (await unit.CouponRepository.SaveChanges()) return NoContent();
            }

            return Accepted();
        }
    }
}
