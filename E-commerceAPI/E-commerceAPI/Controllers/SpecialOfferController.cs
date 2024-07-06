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
    public class SpecialOfferController : ControllerBase
    {
        private readonly UnitOfWork unit;

        public SpecialOfferController(UnitOfWork unit)
        {
            this.unit = unit;
        }
        
        // PUT: api/SpecialOffer/5
        [HttpPut("{id}")]
        [SwaggerOperation(
        Summary = "This Endpoint updates the specified specialOffer element",
            Description = ""
        )]
        [SwaggerResponse(404, "The id that was given doesn't exist in the db", Type = typeof(void))]
        [SwaggerResponse(400, "The id that was given doesn't equal the id in the given specialOffer object", Type = typeof(void))]
        [SwaggerResponse(202, "Something went wrong, please try again later", Type = typeof(void))]
        [SwaggerResponse(204, "Confirms that the specialOffer element was updated successfully", Type = typeof(void))]
        public async Task<IActionResult> PutSpecialOffer(int id, SpecialOfferUpdateDTO specialOfferUpdateDTO)
        {
            var sOffer = await unit.SpecialOfferRepository.GetElement(so => so.Id == specialOfferUpdateDTO.Id);

            if (sOffer == null)
            {
                return NotFound();
            }

            if (id != sOffer.Id)
            {
                return BadRequest();
            }

            sOffer.NewPrice = specialOfferUpdateDTO.NewPrice;
            sOffer.ExpireDate = specialOfferUpdateDTO.ExpireDate;

            if (unit.SpecialOfferRepository.Edit(sOffer))
            {
                if (await unit.SpecialOfferRepository.SaveChanges()) return NoContent();
            }

            return Accepted();
        }

        // POST: api/SpecialOffer
        [HttpPost]
        [SwaggerOperation(
        Summary = "This Endpoint inserts a specialOffer element in the db",
            Description = ""
        )]
        [SwaggerResponse(202, "Something went wrong, please try again later", Type = typeof(void))]
        [SwaggerResponse(204, "Confirms that the specialOffer element was Inserted successfully", Type = typeof(void))]
        [Consumes("application/json")]
        [Produces("application/json")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> PostSpecialOffer(SpecialOfferInsertDTO specialOfferInsertDTO)
        {
            var speOffer = new SpecialOffer()
            {
                ExpireDate = specialOfferInsertDTO.ExpireDate,
                NewPrice = specialOfferInsertDTO.NewPrice,
                Id = specialOfferInsertDTO.Id,
            };

            if (unit.SpecialOfferRepository.Add(speOffer))
            {
                if (await unit.SpecialOfferRepository.SaveChanges()) return NoContent();
            }

            return Accepted();
        }

        // DELETE: api/SpecialOffer/5
        [HttpDelete("{id}")]
        [SwaggerOperation(
        Summary = "This Endpoint deletes a specialOffer element from the db",
            Description = ""
        )]
        [SwaggerResponse(404, "The id that was given doesn't exist in the db", Type = typeof(void))]
        [SwaggerResponse(202, "Something went wrong, please try again later", Type = typeof(void))]
        [SwaggerResponse(204, "Confirms that the specialOffer element was deleted successfully", Type = typeof(void))]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteSpecialOffer(int id)
        {
            var specialOffer = await unit.SpecialOfferRepository.GetElement(so => so.Id == id);
            if (specialOffer == null)
            {
                return NotFound();
            }

            if (unit.SpecialOfferRepository.Delete(specialOffer))
            {
                if (await unit.SpecialOfferRepository.SaveChanges()) return NoContent();
            }

            return Accepted();
        }
    }
}
