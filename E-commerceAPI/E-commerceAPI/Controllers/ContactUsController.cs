using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using E_commerceAPI.Models;
using E_commerceAPI.UnitsOfWork;
using E_commerceAPI.DTOs.DisplayDTOs;
using System.Security.Claims;
using E_commerceAPI.DTOs.InsertDTOs;
using Swashbuckle.AspNetCore.Annotations;
using E_commerceAPI.DTOs.UpdateDTOs;
using Microsoft.AspNetCore.Authorization;

namespace E_commerceAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class ContactUsController : ControllerBase
    {
        private readonly UnitOfWork unit;

        public ContactUsController(UnitOfWork unit)
        {
            this.unit = unit;
        }

        // GET: api/ContactUs
        [HttpGet]
        [SwaggerOperation(
        Summary = "This Endpoint returns a list of contact us elements",
            Description = ""
        )]
        [SwaggerResponse(404, "There weren't any contact us elements in the database", Type = typeof(void))]
        [SwaggerResponse(200, "Returns A list of contact us elements", Type = typeof(List<ContactUsDTO>))]
        public async Task<ActionResult<IEnumerable<ContactUsDTO>>> GetContactUsList()
        {
            var list = await unit.ContactUsRepository.GetAllElements();

            if (list == null || list.Count == 0) return NotFound();

            var l = list.Select(c => new ContactUsDTO { UserName = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Name)?.Value, Email = c.Email, Date = c.Date, Message = c.Message }).ToList();

            return Ok(l);
        }

        // GET: api/ContactUs/5
        [HttpGet("{id}")]
        [SwaggerOperation(
        Summary = "This Endpoint returns the specified contact us element",
            Description = ""
        )]
        [SwaggerResponse(404, "The id that was given doesn't exist in the db", Type = typeof(void))]
        [SwaggerResponse(200, "Returns the specified contact us element", Type = typeof(ContactUsDTO))]
        public async Task<ActionResult<ContactUsDTO>> GetContactUsElement(int id)
        {
            var contactUs = await unit.ContactUsRepository.GetElementWithoutTracking(c => c.Id == id);

            if (contactUs == null)
            {
                return NotFound();
            }

            var cu = new ContactUsDTO()
            {
                Id = contactUs.Id,
                UserName = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Name)?.Value,
                Email = contactUs.Email,
                Date = contactUs.Date,
                Message = contactUs.Message
            };

            return Ok(cu);
        }

        // PUT: api/ContactUs/5
        [HttpPut("{id}")]
        [SwaggerOperation(
        Summary = "This Endpoint updates the specified contact us element",
            Description = ""
        )]
        [SwaggerResponse(404, "The id that was given doesn't exist in the db", Type = typeof(void))]
        [SwaggerResponse(400, "The id that was given doesn't equal the id in the given contact us object", Type = typeof(void))]
        [SwaggerResponse(202, "Something went wrong, please try again later", Type = typeof(void))]
        [SwaggerResponse(204, "Confirms that the contact us element was updated successfully", Type = typeof(void))]
        public async Task<IActionResult> PutContactUs(int id, ContactUsUpdateDTO contactUsDTO)
        {
            var contactUs = await unit.ContactUsRepository.GetElement(c => c.Id == contactUsDTO.Id);

            if (contactUs == null)
            {
                return NotFound();
            }

            if (id != contactUs?.Id)
            {
                return BadRequest();
            }

            contactUs.Email = contactUsDTO.Email;
            contactUs.Message = contactUsDTO.Message;
            contactUs.Status = contactUsDTO.Status;

            if (unit.ContactUsRepository.Edit(contactUs))
            {
                if (await unit.ContactUsRepository.SaveChanges()) return NoContent();
            }

            return Accepted();
        }

        // POST: api/ContactUs
        [HttpPost]
        [SwaggerOperation(
        Summary = "This Endpoint inserts a contact us element in the db",
            Description = ""
        )]
        [SwaggerResponse(202, "Something went wrong, please try again later", Type = typeof(void))]
        [SwaggerResponse(201, "Returns the inserted contact us element and the url you can use to get it", Type = typeof(ContactUsDTO))]
        [Consumes("application/json")]
        [Produces("application/json")]
        public async Task<ActionResult<ContactUsDTO>> PostContactUs(ContactUsInsertDTO contactUs)
        {
            var c = new ContactUs()
            {
                Date = DateTime.Now,
                Status = 'S',
                Email = contactUs.Email,
                UserId = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value,
                Message = contactUs.Message
            };

            if (unit.ContactUsRepository.Add(c))
            {
                if (await unit.ContactUsRepository.SaveChanges()) 
                {
                    var cu = await unit.ContactUsRepository.GetElementWithoutTracking(cu => cu.Id == c.Id);

                    if (cu != null)
                    {
                        var cudto = new ContactUsDTO()
                        {
                            Id = cu.Id,
                            UserName = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Name)?.Value,
                            Email = cu.Email,
                            Date = cu.Date,
                            Message = cu.Message
                        };

                        return CreatedAtAction("GetContactUsElement", new { id = c.Id }, cudto);
                    }
                }
            }

            return Accepted();
        }

        // DELETE: api/ContactUs/5
        [HttpDelete("{id}")]
        [SwaggerOperation(
        Summary = "This Endpoint deletes a contact us element from the db",
            Description = ""
        )]
        [SwaggerResponse(404, "The id that was given doesn't exist in the db", Type = typeof(void))]
        [SwaggerResponse(202, "Something went wrong, please try again later", Type = typeof(void))]
        [SwaggerResponse(204, "Confirms that the contact us element was deleted successfully", Type = typeof(void))]
        public async Task<IActionResult> DeleteContactUs(int id)
        {
            var contactUs = await unit.ContactUsRepository.GetElement(c => c.Id == id);
            if (contactUs == null)
            {
                return NotFound();
            }

            if (unit.ContactUsRepository.Delete(contactUs))
            {
                if (await unit.ContactUsRepository.SaveChanges()) return NoContent();
            }

            return Accepted();
        }

    }
}
