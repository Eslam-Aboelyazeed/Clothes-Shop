using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using E_commerceAPI.DTOs.DisplayDTOs;
using E_commerceAPI.DTOs.InsertDTOs;
using E_commerceAPI.DTOs.UpdateDTOs;
using E_commerceAPI.Models;
using E_commerceAPI.UnitsOfWork;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Swashbuckle.AspNetCore.Annotations;
using System.Security.Claims;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace E_commerceAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class ProductController : ControllerBase
    {
        private readonly UnitOfWork unit;
        private readonly Cloudinary _cloudinary;
        public ProductController(UnitOfWork unit)
        {
            this.unit = unit;

            Account cloudinaryAccount = new Account("dljzei3ti", "171756447372972", "7eFZd9b9rOW8KdQj52mGuna1FQU");
                                        
            _cloudinary = new Cloudinary(cloudinaryAccount);
        }

        // GET: api/Product
        [HttpGet]
        [SwaggerOperation(
        Summary = "This Endpoint returns a list of product elements",
            Description = ""
        )]
        [SwaggerResponse(404, "There weren't any product elements in the database", Type = typeof(void))]
        [SwaggerResponse(200, "Returns A list of product elements", Type = typeof(List<ProductDTO>))]
        public async Task<ActionResult<IEnumerable<ProductDTO>>> GetProductList()
        {
            var list = await unit.ProductRepository.GetAllElements(p => p.specialOffers, p => p.productReviews);

            if (list == null || list.Count == 0) return NotFound();

            var l = list.Select(p => new ProductDTO { 
                Id = p.Id,
                Name = p.Name,
                Price = p.Price,
                Type = p.Type,
                Rating = p.Rating,
                Description = p.Description,
                ImageUrl = p.ImageUrl,
                Quantity = p.Quantity,
                IsSpecialOffer = (p.specialOffers.FirstOrDefault(sp => sp.ExpireDate > DateTime.Now) != null),
                ExpireDate = p.specialOffers.FirstOrDefault(sp => sp.ExpireDate < DateTime.Now)?.ExpireDate,
                NewPrice = p.specialOffers.FirstOrDefault(sp => sp.ExpireDate < DateTime.Now)?.NewPrice,
                productReviews = p.productReviews.Select(pr => new ProductReviewsDTO
                {
                    Id = pr.ProductId,
                    Rating = pr.Rating,
                    Review = pr.Review,
                    UserName = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Name)?.Value
                }).ToList()
            }).ToList();

            return Ok(l);
        }

        //POST api/products
        [HttpPost("/api/products")]
        [SwaggerOperation(
        Summary = "This Endpoint returns a list of products with the specified page size",
            Description = ""
        )]
        [SwaggerResponse(404, "There weren't any products in the database", Type = typeof(void))]
        [SwaggerResponse(200, "Returns A list of products", Type = typeof(List<ProductPageDTO>))]
        public async Task<ActionResult<IEnumerable<ProductPageDTO>>> GetPages([FromQuery] int page = 1, [FromQuery] int pageSize = 5, [FromBody] FilterOptions? filterOptions = null)
        {
            List<Product> productList;

            if (filterOptions != null)
            {
                productList = await unit.ProductRepository.GetAllElements(p => (p.Price >= filterOptions.MinPrice && p.Price < filterOptions.MaxPrice) && p.Name.Trim().ToLower().Contains(filterOptions.Name.Trim().ToLower()), p => p.specialOffers, p => p.productReviews);

                productList = productList.Where(p =>
                    ((p.Type.ToString() == "TShirts" && filterOptions.TShirts) ||
                        (p.Type.ToString() == "Shoes" && filterOptions.Shoes) ||
                        (p.Type.ToString() == "Jackets" && filterOptions.Jackets) ||
                        (p.Type.ToString() == "Other" && filterOptions.Other)) &&
                        (((int)p.Rating == 5 && filterOptions.FiveStars) ||
                        ((int)p.Rating == 4 && filterOptions.FourStars) ||
                        ((int)p.Rating == 3 && filterOptions.ThreeStars) ||
                        ((int)p.Rating == 2 && filterOptions.TwoStars) ||
                        ((int)p.Rating == 1 && filterOptions.OneStar))
                ).ToList();
            }
            else
            {
                productList = await unit.ProductRepository.GetAllElements(p => p.specialOffers, p => p.productReviews);
            }


            var products = productList.Select(p => new ProductDTO
            {
                Id = p.Id,
                Name = p.Name,
                Price = p.Price,
                Type = p.Type,
                Rating = p.Rating,
                Description = p.Description,
                ImageUrl = p.ImageUrl,
                Quantity = p.Quantity,
                IsSpecialOffer = (p.specialOffers.FirstOrDefault(sp => sp.ExpireDate > DateTime.Now) != null),
                ExpireDate = p.specialOffers.FirstOrDefault(sp => sp.ExpireDate < DateTime.Now)?.ExpireDate,
                NewPrice = p.specialOffers.FirstOrDefault(sp => sp.ExpireDate < DateTime.Now)?.NewPrice,
                productReviews = p.productReviews.Select(pr => new ProductReviewsDTO
                {
                    Id = pr.ProductId,
                    Rating = pr.Rating,
                    Review = pr.Review,
                    UserName = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Name)?.Value
                }).ToList(),
            });

            var query = new ProductPageDTO
            {
                products = products,
                TotalCount = productList.Count,
                TotalPages = (int)Math.Ceiling((double)(productList.Count) / pageSize)
            };

            var totalCount = query.products.Count();
            var totalPages = (int)Math.Ceiling((double)totalCount / pageSize);
            query.products = query.products.Skip((page - 1) * pageSize).Take(pageSize);
            return Ok(query);
        }

        [HttpGet("{q:alpha}")]
        [SwaggerOperation(
        Summary = "This Endpoint searches for a list of Products with the specified name",
            Description = ""
        )]
        [SwaggerResponse(400, "There wasn't a name given", Type = typeof(void))]
        [SwaggerResponse(404, "There weren't any products in the database with the specified name", Type = typeof(void))]
        [SwaggerResponse(200, "Returns A list of products with the specified name", Type = typeof(IEnumerable<ProductDTO>))]
        public async Task<ActionResult<IEnumerable<ProductDTO>>> SearchByName(string q)
        {
            if (string.IsNullOrEmpty(q))
            {
                return BadRequest();
            }

            var productList = await unit.ProductRepository.GetAllElements(p => p.Name.Trim().ToLower().Contains(q.Trim().ToLower()), p => p.specialOffers, p => p.productReviews);
            
            var results = productList.Select(p => new ProductDTO
            {
                Id = p.Id,
                Name = p.Name,
                Price = p.Price,
                Type = p.Type,
                Rating = p.Rating,
                Description = p.Description,
                ImageUrl = p.ImageUrl,
                Quantity = p.Quantity,
                IsSpecialOffer = (p.specialOffers.FirstOrDefault(sp => sp.ExpireDate > DateTime.Now) != null),
                ExpireDate = p.specialOffers.FirstOrDefault(sp => sp.ExpireDate < DateTime.Now)?.ExpireDate,
                NewPrice = p.specialOffers.FirstOrDefault(sp => sp.ExpireDate < DateTime.Now)?.NewPrice,
                productReviews = p.productReviews.Select(pr => new ProductReviewsDTO
                {
                    Id = pr.ProductId,
                    Rating = pr.Rating,
                    Review = pr.Review,
                    UserName = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Name)?.Value
                }).ToList(),
            });

            if (results.Count() == 0)
            {
                return NotFound();
            }

            return Ok(results);
        }

        // GET: api/Product/5
        [HttpGet("{id}")]
        [SwaggerOperation(
        Summary = "This Endpoint returns the specified product element",
            Description = ""
        )]
        [SwaggerResponse(404, "The id that was given doesn't exist in the db", Type = typeof(void))]
        [SwaggerResponse(200, "Returns the specified product element", Type = typeof(ProductDTO))]
        public async Task<ActionResult<ProductDTO>> GetProductElement(int id)
        {
            var product = await unit.ProductRepository.GetElementWithoutTracking(p => p.Id == id, p => p.specialOffers, p => p.productReviews);

            if (product == null)
            {
                return NotFound();
            }

            var p = new ProductDTO()
            {
                Id = product.Id,
                Name = product.Name,
                Price = product.Price,
                Type = product.Type,
                Rating = product.Rating,
                Description = product.Description,
                ImageUrl = product.ImageUrl,
                Quantity = product.Quantity,
                IsSpecialOffer = (product.specialOffers.FirstOrDefault(sp => sp.ExpireDate > DateTime.Now) != null),
                ExpireDate = product.specialOffers.FirstOrDefault(sp => sp.ExpireDate < DateTime.Now)?.ExpireDate,
                NewPrice = product.specialOffers.FirstOrDefault(sp => sp.ExpireDate < DateTime.Now)?.NewPrice,
                productReviews = product.productReviews.Select(pr => new ProductReviewsDTO
                {
                    Id = pr.ProductId,
                    Rating = pr.Rating,
                    Review = pr.Review,
                    UserName = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Name)?.Value
                }).ToList()
            };

            return Ok(p);
        }

        // PUT: api/Product/5
        [HttpPut("{id}")]
        [SwaggerOperation(
        Summary = "This Endpoint updates the specified product element",
            Description = ""
        )]
        [SwaggerResponse(404, "The id that was given doesn't exist in the db", Type = typeof(void))]
        [SwaggerResponse(400, "The id that was given doesn't equal the id in the given product object", Type = typeof(void))]
        [SwaggerResponse(202, "Something went wrong, please try again later", Type = typeof(void))]
        [SwaggerResponse(200, "Returns the updated product", Type = typeof(ProductDTO))]
        [Authorize(Roles ="Admin")]
        public async Task<IActionResult> PutProduct(int id, [FromForm] IFormFile? imageUrl, [FromForm] string productstring)
        {
            string iUrl = "";

            if (imageUrl != null)
            {
                iUrl = await UploadImage(imageUrl);
            }

            var productDTO = JsonConvert.DeserializeObject<ProductUpdateDTO>(productstring);

            var product = await unit.ProductRepository.GetElement(p => p.Id == productDTO.Id);

            if (product == null)
            {
                return NotFound();
            }

            if (id != product.Id)
            {
                return BadRequest();
            }

            product.Name = productDTO.Name;
            product.Description = productDTO.Description;
            product.Price = productDTO.Price;
            if (iUrl != "")
            {
                product.ImageUrl = iUrl;
            }
            product.Quantity = productDTO.Quantity;
            product.Rating = productDTO.Rating;
            product.Type = productDTO.Type;



            if (unit.ProductRepository.Edit(product))
            {
                if (await unit.ProductRepository.SaveChanges()) 
                {
                    var pro = await unit.ProductRepository.GetElementWithoutTracking(prod => prod.Id == product.Id, prod => prod.specialOffers, prod => prod.productReviews);

                    if (pro != null)
                    {
                        var pdto = new ProductDTO()
                        {
                            Id = product.Id,
                            Name = product.Name,
                            Price = product.Price,
                            Type = product.Type,
                            Rating = product.Rating,
                            Description = product.Description,
                            ImageUrl = product.ImageUrl,
                            Quantity = product.Quantity,
                            IsSpecialOffer = (product.specialOffers?.FirstOrDefault(sp => sp.ExpireDate > DateTime.Now) != null),
                            ExpireDate = product.specialOffers?.FirstOrDefault(sp => sp.ExpireDate < DateTime.Now)?.ExpireDate,
                            NewPrice = product.specialOffers?.FirstOrDefault(sp => sp.ExpireDate < DateTime.Now)?.NewPrice,
                            productReviews = product.productReviews?.Select(pr => new ProductReviewsDTO
                            {
                                Id = pr.ProductId,
                                Rating = pr.Rating,
                                Review = pr.Review,
                                UserName = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Name)?.Value
                            }).ToList()
                        };

                        return Ok(pdto);
                    }
                }
            }

            return Accepted();
        }

        // POST: api/Product
        [HttpPost]
        [SwaggerOperation(
        Summary = "This Endpoint inserts a product element in the db",
            Description = ""
        )]
        [SwaggerResponse(202, "Something went wrong, please try again later", Type = typeof(void))]
        [SwaggerResponse(201, "Returns the inserted product element and the url you can use to get it", Type = typeof(ProductDTO))]
        [Produces("application/json")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<ProductDTO>> PostProduct([FromForm] IFormFile imageUrl, [FromForm] string productstring)
        {
            string iUrl = await UploadImage(imageUrl);

            var product = JsonConvert.DeserializeObject<ProductInsertDTO>(productstring);

            var p = new Product()
            {
               Name = product.Name,
               Quantity = product.Quantity,
               ImageUrl =iUrl,
               Price = product.Price,
               Description = product.Description,
               Rating = product.Rating,
               Type = product.Type
            };

            if (unit.ProductRepository.Add(p))
            {
                if (await unit.ProductRepository.SaveChanges())
                {
                    var pro = await unit.ProductRepository.GetElementWithoutTracking(prod => prod.Id == p.Id, prod => prod.specialOffers, prod => prod.productReviews);

                    if (pro != null)
                    {
                        var pdto = new ProductDTO()
                        {
                            Id = p.Id,
                            Name = p.Name,
                            Price = p.Price,
                            Type = p.Type,
                            Rating = p.Rating,
                            Description = p.Description,
                            ImageUrl = p.ImageUrl,
                            Quantity = p.Quantity,
                            IsSpecialOffer = (p.specialOffers?.FirstOrDefault(sp => sp.ExpireDate > DateTime.Now) != null),
                            ExpireDate = p.specialOffers?.FirstOrDefault(sp => sp.ExpireDate < DateTime.Now)?.ExpireDate,
                            NewPrice = p.specialOffers?.FirstOrDefault(sp => sp.ExpireDate < DateTime.Now)?.NewPrice,
                            productReviews = p.productReviews?.Select(pr => new ProductReviewsDTO
                            {
                                Id = pr.ProductId,
                                Rating = pr.Rating,
                                Review = pr.Review,
                                UserName = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Name)?.Value
                            }).ToList()
                        };

                        return CreatedAtAction("GetProductElement", new { id = p.Id }, pdto);
                    }
                }
            }

            return Accepted();
        }

        // DELETE: api/Product/5
        [HttpDelete("{id}")]
        [SwaggerOperation(
        Summary = "This Endpoint deletes a product element from the db",
            Description = ""
        )]
        [SwaggerResponse(404, "The id that was given doesn't exist in the db", Type = typeof(void))]
        [SwaggerResponse(202, "Something went wrong, please try again later", Type = typeof(void))]
        [SwaggerResponse(204, "Confirms that the product element was deleted successfully", Type = typeof(void))]
        [Authorize(Roles ="Admin")]
        public async Task<IActionResult> DeleteProduct(int id)
        {
            var product = await unit.ProductRepository.GetElement(p => p.Id == id);
            if (product == null)
            {
                return NotFound();
            }

            if (unit.ProductRepository.Delete(product))
            {
                if (await unit.ProductRepository.SaveChanges()) return NoContent();
            }

            return Accepted();
        }

        private async Task<string> UploadImage(IFormFile? file)
        {
            if (file == null || file.Length == 0)
            {
                return "";
            }

            using (var stream = file.OpenReadStream())
            {
                var uploadParams = new ImageUploadParams
                {
                    File = new FileDescription(file.FileName, stream)
                };
                var uploadResult = await _cloudinary.UploadAsync(uploadParams);

                if (uploadResult.Error != null)
                {
                    return "";
                }

                return uploadResult.SecureUrl.AbsoluteUri;
            }
        }
    }
}
