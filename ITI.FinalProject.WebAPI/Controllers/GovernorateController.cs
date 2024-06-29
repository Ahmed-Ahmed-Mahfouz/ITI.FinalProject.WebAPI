using Application.DTOs.DisplayDTOs;
using Application.DTOs.InsertDTOs;
using Application.DTOs.UpdateDTOs;
using Application.Interfaces.ApplicationServices;
using Application.Services;
using Domain.Entities;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace ITI.FinalProject.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GovernorateController : ControllerBase
    {
        private readonly IGenericService<Governorate, GovernorateDTO, GovernorateInsertDTO, GovernorateUpdateDTO, int> service;

        public GovernorateController(IGenericService<Governorate, GovernorateDTO, GovernorateInsertDTO, GovernorateUpdateDTO, int> service)
        {
            this.service = service;
        }

        // GET: api/Governorate
        [SwaggerOperation(
        Summary = "This Endpoint returns a list of governorates",
            Description = ""
        )]
        [SwaggerResponse(404, "There weren't any governorates in the database", Type = typeof(void))]
        [SwaggerResponse(200, "Returns A list of governorates", Type = typeof(List<GovernorateDTO>))]
        [HttpGet]
        public async Task<ActionResult<List<GovernorateDTO>>> GetAllGovernorates()
        {
            var governorates = await service.GetAllObjects();

            if (governorates == null || governorates.Count == 0)
            {
                return NotFound();
            }

            return Ok(governorates);
        }

        // GET api/Governorate/5
        [SwaggerOperation(
        Summary = "This Endpoint returns the specified governorate",
            Description = ""
        )]
        [SwaggerResponse(404, "The id that was given doesn't exist in the db", Type = typeof(void))]
        [SwaggerResponse(200, "Returns the specified governorate", Type = typeof(GovernorateDTO))]
        [HttpGet("{id}")]
        public async Task<ActionResult<GovernorateDTO>> GetGovernorateById(int id)
        {
            var governorate = await service.GetObject(g => g.id == id);

            if (governorate == null)
            {
                return NotFound();
            }

            return Ok(governorate);
        }

        // POST api/Governorate
        [SwaggerOperation(
        Summary = "This Endpoint inserts a governorate element in the db",
            Description = ""
        )]
        [SwaggerResponse(202, "Something went wrong, please try again later", Type = typeof(void))]
        //[SwaggerResponse(201, "Returns the inserted governorate element and the url you can use to get it", Type = typeof(GovernorateDTO))]
        [SwaggerResponse(204, "Confirms that the governorate was inserted successfully", Type = typeof(void))]
        [HttpPost]
        public async Task<IActionResult> PostGovernorate([FromBody] GovernorateInsertDTO governorateInsertDTO)
        {
            if (await service.InsertObject(governorateInsertDTO))
            {
                if (await service.SaveChangesForObject())
                {
                    return NoContent();
                }
            }

            return Accepted();
        }

        // PUT api/Governorate/5
        [SwaggerOperation(
        Summary = "This Endpoint updates the specified governorate",
            Description = ""
        )]
        [SwaggerResponse(404, "The id that was given doesn't exist in the db", Type = typeof(void))]
        [SwaggerResponse(400, "The id that was given doesn't equal the id in the given governorate object", Type = typeof(void))]
        [SwaggerResponse(202, "Something went wrong, please try again later", Type = typeof(void))]
        [SwaggerResponse(204, "Confirms that the governorate was updated successfully", Type = typeof(void))]
        [HttpPut("{id}")]
        public async Task<IActionResult> PutGovernorate(int id, [FromBody] GovernorateUpdateDTO governorateUpdateDTO)
        {
            if (id != governorateUpdateDTO.id)
            {
                return BadRequest();
            }

            var governorate = await service.GetObjectWithoutTracking(g => g.id == id);

            if (governorate == null)
            {
                return NotFound();
            }

            if(await service.UpdateObject(governorateUpdateDTO))
            {
                if (await service.SaveChangesForObject())
                {
                    return NoContent();
                }
            }

            return Accepted();
        }

        // DELETE api/Governorate/5
        [SwaggerOperation(
        Summary = "This Endpoint deletes the specified governorate from the db",
            Description = ""
        )]
        [SwaggerResponse(404, "The id that was given doesn't exist in the db", Type = typeof(void))]
        [SwaggerResponse(202, "Something went wrong, please try again later", Type = typeof(void))]
        [SwaggerResponse(204, "Confirms that the governorate was deleted successfully", Type = typeof(void))]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteGovernorate(int id)
        {
            var governorate = await service.GetObjectWithoutTracking(g => g.id == id);

            if (governorate == null)
            {
                return NotFound();
            }

            if (await service.DeleteObject(id))
            {
                if (await service.SaveChangesForObject())
                {
                    return NoContent();
                }
            }

            return Accepted();
        }
    }
}
