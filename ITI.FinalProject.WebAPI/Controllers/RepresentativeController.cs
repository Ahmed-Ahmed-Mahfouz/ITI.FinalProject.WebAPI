using Application.DTOs;
using Application.DTOs.DisplayDTOs;
using Application.DTOs.InsertDTOs;
using Application.DTOs.UpdateDTOs;
using Application.Interfaces.ApplicationServices;
using Domain.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using NuGet.Protocol.Core.Types;
using Swashbuckle.AspNetCore.Annotations;
using System.Transactions;

namespace ITI.FinalProject.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RepresentativeController : ControllerBase
    {
        private readonly IPaginationService<Representative, RepresentativeDisplayDTO, RepresentativeInsertDTO, RepresentativeUpdateDTO, string> service;

        public RepresentativeController(IPaginationService<Representative,RepresentativeDisplayDTO,RepresentativeInsertDTO,RepresentativeUpdateDTO,string> service)
        {
            this.service = service;
        }

        // GET: api/Representative
        [SwaggerOperation(
        Summary = "This Endpoint returns a list of representative",
            Description = ""
        )]
        [SwaggerResponse(404, "There weren't any representative in the database", Type = typeof(void))]
        [SwaggerResponse(200, "Returns A list of representative", Type = typeof(List<RepresentativeDisplayDTO>))]
        [HttpGet]
        public async Task<ActionResult<List<RepresentativeDisplayDTO>>> GetAllRepresentative()
        {
            var Representatives=await service.GetAllObjects();

            if (Representatives == null || Representatives.Count == 0)
            {
                return NotFound();
            }

            return Ok(Representatives);
        }

        // GET: api/Representative/owcmwmece51cwe5
        [SwaggerOperation(
        Summary = "This Endpoint returns the specified representative",
            Description = ""
        )]
        [SwaggerResponse(404, "The id that was given doesn't exist in the db", Type = typeof(void))]
        [SwaggerResponse(200, "Returns the specified representative", Type = typeof(RepresentativeDisplayDTO))]
        [HttpGet("id")]
        public async Task<ActionResult<RepresentativeDisplayDTO>> GetRepresentativeById(string id)
        {
            var representative = await service.GetObject(r => r.userId == id);

            if (representative == null)
            {
                return NotFound();
            }

            return Ok(representative);
        }


        // POST api/Representative
        [SwaggerOperation(
        Summary = "This Endpoint inserts a representative element in the db",
            Description = ""
        )]
        [SwaggerResponse(202, "Something went wrong, please try again later", Type = typeof(void))]
        [SwaggerResponse(204, "Confirms that the representative was inserted successfully", Type = typeof(void))]
        [HttpPost]
        public async Task<ActionResult> AddRepresentative([FromBody] RepresentativeInsertDTO RepresentativeInsertDTO)
        {
            using (var transaction = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
            {

                var result = await service.InsertObject(RepresentativeInsertDTO);
                //Transaction inside true condition or before it.
                transaction.Complete();

                if (result.Succeeded)
                {
                    return NoContent();
                }

                return Accepted(result.Message);
            }

        }

        // PUT api/Representative/owcmwmece51cwe5
        [SwaggerOperation(
        Summary = "This Endpoint updates the specified representative",
            Description = ""
        )]
        [SwaggerResponse(404, "The id that was given doesn't exist in the db", Type = typeof(void))]
        [SwaggerResponse(400, "The id that was given doesn't equal the id in the given representative object", Type = typeof(void))]
        [SwaggerResponse(202, "Something went wrong, please try again later", Type = typeof(void))]
        [SwaggerResponse(204, "Confirms that the representative was updated successfully", Type = typeof(void))]
        [HttpPut("{id}")]
        public async Task<IActionResult> PutRepresentative(string id, [FromBody] RepresentativeUpdateDTO representativeUpdateDTO)
        {
            if (id != representativeUpdateDTO.Id)
            {
                return BadRequest("Id doesn't match the id in the object");
            }

            var representative = await service.GetObjectWithoutTracking(r => r.userId == id);

            if (representative == null)
            {
                return NotFound("Representative doesn't exist in the db");
            }

            var result = await service.UpdateObject(representativeUpdateDTO);

            if (result.Succeeded)
            {
                return NoContent();
            }

            return Accepted(result.Message);
        }

        // DELETE api/Representative/owcmwmece51cwe5
        [SwaggerOperation(
        Summary = "This Endpoint deletes the specified representative from the db",
            Description = ""
        )]
        [SwaggerResponse(404, "The id that was given doesn't exist in the db", Type = typeof(void))]
        [SwaggerResponse(202, "Something went wrong, please try again later", Type = typeof(void))]
        [SwaggerResponse(204, "Confirms that the representative was deleted successfully", Type = typeof(void))]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteRepresentative(string id)
        {
            var representative = await service.GetObjectWithoutTracking(r => r.userId == id);

            if (representative == null)
            {
                return NotFound("Representative doesn't exist in the db");
            }

            var result = await service.DeleteObject(id);

            if (result.Succeeded)
            {
                return NoContent();
            }

            return Accepted(result.Message);
        }

    }
}
