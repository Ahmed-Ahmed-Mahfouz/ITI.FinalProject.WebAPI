using Application.DTOs.DisplayDTOs;
using Application.DTOs.InsertDTOs;
using Application.DTOs.UpdateDTOs;
using Application.Interfaces.ApplicationServices;
using Application.Services;
using Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace ITI.FinalProject.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "Admin")]
    public class RolePowersController : ControllerBase
    {
        private readonly IGenericService<RolePowers, RolePowersDTO, RolePowersInsertDTO, RolePowersUpdateDTO, string> service;

        public RolePowersController(IGenericService<RolePowers, RolePowersDTO, RolePowersInsertDTO, RolePowersUpdateDTO, string> service)
        {
            this.service = service;
        }

        // GET: api/RolePowers
        [SwaggerOperation(
        Summary = "This Endpoint returns a list of rolePowers",
            Description = ""
        )]
        [SwaggerResponse(404, "There weren't any rolePowers in the database", Type = typeof(void))]
        [SwaggerResponse(401, "Unauthorized", Type = typeof(void))]
        [SwaggerResponse(200, "Returns A list of rolePowers", Type = typeof(List<RolePowersDTO>))]
        [HttpGet]
        public async Task<ActionResult<List<RolePowersDTO>>> GetAllRolePowers()
        {
            var rolePowers = await service.GetAllObjects();

            if (rolePowers == null || rolePowers.Count == 0)
            {
                return NotFound();
            }

            return Ok(rolePowers);
        }

        // GET api/RolePowers/fkmc4a2wkmfkmq
        [SwaggerOperation(
        Summary = "This Endpoint returns the specified rolePower",
            Description = ""
        )]
        [SwaggerResponse(404, "The id that was given doesn't exist in the db", Type = typeof(void))]
        [SwaggerResponse(401, "Unauthorized", Type = typeof(void))]
        [SwaggerResponse(200, "Returns the specified rolePower", Type = typeof(RolePowersDTO))]
        [HttpGet("{id}")]
        public async Task<ActionResult<RolePowersDTO>> GetRolePowerById(string id)
        {
            var rolePower = await service.GetObject(rp => rp.RoleId == id);

            if (rolePower == null)
            {
                return NotFound();
            }

            return Ok(rolePower);
        }

        // POST api/RolePowers
        [SwaggerOperation(
        Summary = "This Endpoint inserts a rolePower element in the db",
            Description = ""
        )]
        [SwaggerResponse(401, "Unauthorized", Type = typeof(void))]
        [SwaggerResponse(202, "Something went wrong, please try again later", Type = typeof(void))]
        //[SwaggerResponse(400, "Role name or powers weren't given", Type = typeof(void))]
        [SwaggerResponse(204, "Confirms that the rolePower was inserted successfully", Type = typeof(void))]
        [HttpPost]
        public async Task<IActionResult> PostRolePower([FromBody] RolePowersInsertDTO rolePowersInsertDTO)
        {
            //if (rolePowersInsertDTO.RoleName == null || rolePowersInsertDTO.RoleName == "")
            //{
            //    return BadRequest("please enter group name");
            //}

            //if (rolePowersInsertDTO.Powers.Count == 0)
            //{
            //    return BadRequest("Please enter role powers before inserting");
            //}

            var result = await service.InsertObject(rolePowersInsertDTO);

            if (result.Succeeded)
            {
                if (await service.SaveChangesForObject())
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

        // PUT api/RolePowers/fkmc4a2wkmfkmq
        [SwaggerOperation(
        Summary = "This Endpoint updates the specified rolePower",
            Description = ""
        )]
        [SwaggerResponse(404, "The id that was given doesn't exist in the db", Type = typeof(void))]
        [SwaggerResponse(400, "The id that was given doesn't equal the id in the given rolePower object", Type = typeof(void))]
        [SwaggerResponse(401, "Unauthorized", Type = typeof(void))]
        [SwaggerResponse(202, "Something went wrong, please try again later", Type = typeof(void))]
        [SwaggerResponse(204, "Confirms that the rolePower was updated successfully", Type = typeof(void))]
        [HttpPut("{id}")]
        public async Task<IActionResult> PutRolePower(string id, [FromBody] RolePowersUpdateDTO rolePowersUpdateDTO)
        {
            if (id != rolePowersUpdateDTO.RoleId)
            {
                return BadRequest("Id doesn't match the id in the object");
            }

            var rolePower = await service.GetObjectWithoutTracking(rp => rp.RoleId == id);

            if (rolePower == null)
            {
                return NotFound("Role Power doesn't exist in the db");
            }

            var result = await service.UpdateObject(rolePowersUpdateDTO);

            if (result.Succeeded)
            {
                if (await service.SaveChangesForObject())
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

        // DELETE api/RolePowers/fkmc4a2wkmfkmq
        [SwaggerOperation(
        Summary = "This Endpoint deletes the specified rolePower from the db",
            Description = ""
        )]
        [SwaggerResponse(404, "The id that was given doesn't exist in the db", Type = typeof(void))]
        [SwaggerResponse(401, "Unauthorized", Type = typeof(void))]
        [SwaggerResponse(202, "Something went wrong, please try again later", Type = typeof(void))]
        [SwaggerResponse(204, "Confirms that the rolePower was deleted successfully", Type = typeof(void))]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteRolePower(string id)
        {
            var rolePower = await service.GetObjectWithoutTracking(rp => rp.RoleId == id);

            if (rolePower == null)
            {
                return NotFound("Role Power doesn't exist in the db");
            }

            var result = await service.DeleteObject(id);

            if (result.Succeeded)
            {
                if (await service.SaveChangesForObject())
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
