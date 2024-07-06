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
    public class ProductReviewsController : ControllerBase
    {
        private readonly UnitOfWork unit;

        public ProductReviewsController(UnitOfWork unit)
        {
            this.unit = unit;
        }

        // GET: api/ProductReviews
        [HttpGet]
        [SwaggerOperation(
        Summary = "This Endpoint returns a list of productReviews elements",
            Description = ""
        )]
        [SwaggerResponse(404, "There weren't any productReviews elements in the database", Type = typeof(void))]
        [SwaggerResponse(200, "Returns A list of ProductReviews elements", Type = typeof(List<ProductReviewsDTO>))]
        public async Task<ActionResult<IEnumerable<ProductReviewsDTO>>> GetProductReviewsList()
        {
            var list = await unit.ProductReviewsRepository.GetAllElements(pr => pr.user);

            if (list == null || list.Count == 0) return NotFound();

            var l = list.Select(pr => new ProductReviewsDTO
            {
                Id = pr.ProductId,
                UserName = pr.user.UserName,
                Rating = pr.Rating,
                Review = pr.Review
            }).ToList();

            return Ok(l);
        }

        // GET: api/ProductReviews/5
        [HttpGet("{id}")]
        [SwaggerOperation(
        Summary = "This Endpoint returns the specified productReviews element",
            Description = ""
        )]
        [SwaggerResponse(404, "The id that was given doesn't exist in the db", Type = typeof(void))]
        [SwaggerResponse(200, "Returns the specified productReviews element", Type = typeof(ProductReviewsDTO))]
        public async Task<ActionResult<ProductReviewsDTO>> GetProductReviewsElement(int id)
        {
            var proReview = await unit.ProductReviewsRepository.GetElementWithoutTracking(pr => pr.ProductId == id && pr.UserId == User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier).Value, pr => pr.user);
            
            if (proReview == null)
            {
                return NotFound();
            }

            var pr = new ProductReviewsDTO()
            {
                Id = proReview.ProductId,
                UserName = proReview.user.UserName,
                Rating = proReview.Rating,
                Review = proReview.Review
            };

            return Ok(pr);
        }

        // PUT: api/ProductReviews/5
        [HttpPut("{id}")]
        [SwaggerOperation(
        Summary = "This Endpoint updates the specified productReviews element",
            Description = ""
        )]
        [SwaggerResponse(404, "The id that was given doesn't exist in the db", Type = typeof(void))]
        [SwaggerResponse(400, "The id that was given doesn't equal the id in the given productReviews object", Type = typeof(void))]
        [SwaggerResponse(202, "Something went wrong, please try again later", Type = typeof(void))]
        [SwaggerResponse(204, "Confirms that the productReviews element was updated successfully", Type = typeof(void))]
        public async Task<IActionResult> PutProductReview(int id, ProductReviewsUpdateDTO productReviewsDTO)
        {
            var productReview = await unit.ProductReviewsRepository.GetElement(pr => pr.ProductId == productReviewsDTO.ProductId && pr.UserId == User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier).Value);

            if (productReview == null)
            {
                return NotFound();
            }

            if (id != productReview.ProductId)
            {
                return BadRequest();
            }

            productReview.Rating = productReviewsDTO.Rating;
            productReview.Review = productReviewsDTO.Review;

            if (unit.ProductReviewsRepository.Edit(productReview))
            {
                if (await unit.ProductReviewsRepository.SaveChanges()) return NoContent();
            }

            return Accepted();
        }

        // POST: api/ProductReviews
        [HttpPost]
        [SwaggerOperation(
        Summary = "This Endpoint inserts a productReviews element in the db",
            Description = ""
        )]
        [SwaggerResponse(202, "Something went wrong, please try again later", Type = typeof(void))]
        [SwaggerResponse(201, "Returns the inserted productReviews element and the url you can use to get it", Type = typeof(ProductReviewsDTO))]
        [Consumes("application/json")]
        [Produces("application/json")]
        public async Task<ActionResult<ProductReviewsDTO>> PostCoupon(ProductReviewsInsertDTO productReviewsInsertDTO)
        {
            var proReview = new ProductReviews()
            {
                Review = productReviewsInsertDTO.Review,
                Rating = productReviewsInsertDTO.Rating,
                ProductId = productReviewsInsertDTO.ProductId,
                UserId = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier).Value
            };

            if (unit.ProductReviewsRepository.Add(proReview))
            {
                if (await unit.ProductReviewsRepository.SaveChanges())
                {
                    var prdto = new ProductReviewsDTO()
                    {
                        Id = proReview.ProductId,
                        Rating = proReview.Rating,
                        Review = proReview.Review,
                        UserName = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Name).Value
                    };

                    return CreatedAtAction("GetProductReviewsElement", new { id = proReview.ProductId }, prdto);
                }
            }

            return Accepted();
        }

        // DELETE: api/ProductReviews/5
        [HttpDelete("{id}")]
        [SwaggerOperation(
        Summary = "This Endpoint deletes a productReviews element from the db",
            Description = ""
        )]
        [SwaggerResponse(404, "The product id that was given doesn't exist in the db", Type = typeof(void))]
        [SwaggerResponse(202, "Something went wrong, please try again later", Type = typeof(void))]
        [SwaggerResponse(204, "Confirms that the productReviews element was deleted successfully", Type = typeof(void))]
        public async Task<IActionResult> DeleteProductReview(int id)
        {
            var productReview = await unit.ProductReviewsRepository.GetElement(pr => pr.ProductId == id && pr.UserId == User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier).Value);
            
            if (productReview == null)
            {
                return NotFound();
            }

            if (unit.ProductReviewsRepository.Delete(productReview))
            {
                if (await unit.ProductReviewsRepository.SaveChanges()) return NoContent();
            }

            return Accepted();
        }
    }
}
