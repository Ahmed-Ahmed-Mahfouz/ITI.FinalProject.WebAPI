using Application.DTOs.DisplayDTOs;
using Application.DTOs.InsertDTOs;
using Application.DTOs.UpdateDTOs;
using Application.Interfaces.ApplicationServices;
using Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace ITI.FinalProject.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "Merchant")]
    public class ShippingController : ControllerBase
    {
        private readonly IGenericService<Shipping, DisplayShippingDTO, InsertShippingDTO, UpdateShippingDTO, int> _shippingService;
        public ShippingController(IGenericService<Shipping, DisplayShippingDTO, InsertShippingDTO, UpdateShippingDTO, int> shippingService)
        {
            _shippingService = shippingService;
        }

        // GET: api/Shipping
        [SwaggerOperation(Summary = "This Endpoint returns a list of shippings", Description = "")]
        [SwaggerResponse(404, "There weren't any shippings in the database", Type = typeof(void))]
        [SwaggerResponse(401, "Unauthorized", Type = typeof(void))]
        [SwaggerResponse(200, "Returns A list of shippings", Type = typeof(List<DisplayShippingDTO>))]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<DisplayShippingDTO>>> GetShippings()
        {
            var shippings = await _shippingService.GetAllObjects();

            if (shippings == null || shippings.Count == 0)
            {
                return NotFound();
            }

            return Ok(shippings);
        }

        // GET: api/Shipping/5
        [SwaggerOperation(Summary = "This Endpoint returns the specified shipping")]
        [SwaggerResponse(404, "The id that was given doesn't exist in the db", Type = typeof(void))]
        [SwaggerResponse(401, "Unauthorized", Type = typeof(void))]
        [SwaggerResponse(200, "Returns the specified shipping", Type = typeof(DisplayShippingDTO))]
        [HttpGet("{id}")]
        public async Task<ActionResult<DisplayShippingDTO>> GetShipping(int id)
        {
            var shipping = await _shippingService.GetObject(s => s.Id == id);

            if (shipping == null)
            {
                return NotFound();
            }

            return Ok(shipping);
        }

        // POST: api/Shipping
        [SwaggerOperation(Summary = "This Endpoint inserts a new shipping in the db", Description = "")]
        [SwaggerResponse(401, "Unauthorized", Type = typeof(void))]
        [SwaggerResponse(202, "Something went wrong, please try again later", Type = typeof(void))]
        [SwaggerResponse(204, "Confirms that the shipping was inserted successfully", Type = typeof(void))]
        [HttpPost]
        public async Task<IActionResult> PostShipping(InsertShippingDTO shippingDTO)
        {
            var result = await _shippingService.InsertObject(shippingDTO);

            if (result.Succeeded)
            {
                if (await _shippingService.SaveChangesForObject())
                {
                    return NoContent();
                }
                else
                {
                    return Accepted("Error saving changes");
                }
            }

            return Accepted(result.Message);
        }

        // PUT: api/Shipping/5
        [SwaggerOperation(Summary = "This Endpoint updates the specified shipping", Description = "")]
        [SwaggerResponse(404, "The id that was given doesn't exist in the db", Type = typeof(void))]
        [SwaggerResponse(400, "The id that was given doesn't equal the id in the given shipping object", Type = typeof(void))]
        [SwaggerResponse(401, "Unauthorized", Type = typeof(void))]
        [SwaggerResponse(202, "Something went wrong, please try again later", Type = typeof(void))]
        [SwaggerResponse(204, "Confirms that the shipping was updated successfully", Type = typeof(void))]
        [HttpPut("{id}")]
        public async Task<IActionResult> PutShipping(int id, UpdateShippingDTO shippingDTO)
        {
            if (id != shippingDTO.Id)
            {
                return BadRequest("Id deosn't match the id in the object");
            }

            var success = await _shippingService.GetObjectWithoutTracking(s => s.Id == id);

            if (success == null)
            {
                return NotFound("Shipping object doesn't exist in the db");
            }

            var result = await _shippingService.UpdateObject(shippingDTO);

            if (result.Succeeded)
            {
                if (await _shippingService.SaveChangesForObject())
                {
                    return NoContent();
                }
                else
                {
                    return Accepted("Error saving changes");
                }
            }

            return Accepted(result.Message);
        }

        // DELETE: api/Shipping/5
        [SwaggerOperation(Summary = "This Endpoint deletes the specified shipping", Description = "")]
        [SwaggerResponse(404, "The id that was given doesn't exist in the db", Type = typeof(void))]
        [SwaggerResponse(401, "Unauthorized", Type = typeof(void))]
        [SwaggerResponse(202, "Something went wrong, please try again later", Type = typeof(void))]
        [SwaggerResponse(204, "Confirms that the shipping was deleted successfully", Type = typeof(void))]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteShipping(int id)
        {
            var success = await _shippingService.GetObjectWithoutTracking(s => s.Id == id);

            if (success == null)
            {
                return NotFound("Shipping object doesn't exist in the db");
            }

            var result = await _shippingService.DeleteObject(id);

            if (result.Succeeded)
            {
                if (await _shippingService.SaveChangesForObject())
                {
                    return NoContent();
                }
                else
                {
                    return Accepted("Error saving changes");
                }
            }

            return Accepted(result.Message);
        }
    }
}
