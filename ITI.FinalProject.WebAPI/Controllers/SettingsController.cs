using Application.DTOs.DisplayDTOs;
using Application.DTOs.InsertDTOs;
using Application.DTOs.UpdateDTOs;
using Application.Interfaces.ApplicationServices;
using Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Swashbuckle.AspNetCore.Annotations;
using System.Security.Claims;
using System.Text;

namespace ITI.FinalProject.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SettingsController : ControllerBase
    {
        private readonly IGenericService<Settings, SettingsDTO, SettingsInsertDTO, SettingsUpdateDTO, int> service;
        private readonly RoleManager<ApplicationRoles> roleManager;
        public SettingsController(IGenericService<Settings, SettingsDTO, SettingsInsertDTO, SettingsUpdateDTO, int> service, RoleManager<ApplicationRoles> roleManager)
        {
            this.service = service;
            this.roleManager = roleManager;
        }

        // GET: api/Settings
        [SwaggerOperation(Summary = "This Endpoint returns a list of settings", Description = "")]
        [SwaggerResponse(404, "There weren't any settings in the database", Type = typeof(void))]
        [SwaggerResponse(401, "Unauthorized", Type = typeof(void))]
        [SwaggerResponse(200, "Returns A list of settings", Type = typeof(List<SettingsDTO>))]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<SettingsDTO>>> GetSettingsList()
        {
            var roles = await GetRoles();

            if (roles.FirstOrDefault(r => r.Name == User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Role)?.Value) == null || roles.FirstOrDefault(r => r.Name == User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Role)?.Value)?.RolePowers.FirstOrDefault(rp => rp.Power == Domain.Enums.PowerTypes.Read) == null)
            {
                return Unauthorized();
            }

            var settingsList = await service.GetAllObjects();

            if (settingsList == null || settingsList.Count == 0)
            {
                return NotFound();
            }

            return Ok(settingsList);
        }


        // GET: api/Settings/5
        [SwaggerOperation(Summary = "This Endpoint returns the specified settings")]
        [SwaggerResponse(404, "The id that was given doesn't exist in the db", Type = typeof(void))]
        [SwaggerResponse(401, "Unauthorized", Type = typeof(void))]
        [SwaggerResponse(200, "Returns the specified settings", Type = typeof(SettingsDTO))]
        [HttpGet("{id}")]
        public async Task<ActionResult<SettingsDTO>> GetSettings(int id)
        {
            var roles = await GetRoles();

            if (roles.FirstOrDefault(r => r.Name == User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Role)?.Value) == null || roles.FirstOrDefault(r => r.Name == User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Role)?.Value)?.RolePowers.FirstOrDefault(rp => rp.Power == Domain.Enums.PowerTypes.Read) == null)
            {
                return Unauthorized();
            }

            var settings = await service.GetObject(s => s.Id == id);

            if (settings == null)
            {
                return NotFound();
            }

            return Ok(settings);
        }

        // POST: api/Settings
        [SwaggerOperation(Summary = "This Endpoint inserts a new settings in the db", Description = "")]
        [SwaggerResponse(401, "Unauthorized", Type = typeof(void))]
        [SwaggerResponse(202, "Something went wrong, please try again later", Type = typeof(void))]
        [SwaggerResponse(204, "Confirms that the settings was inserted successfully", Type = typeof(void))]
        [HttpPost]
        public async Task<IActionResult> PostSettings([FromBody] SettingsInsertDTO settingsInsertDTO)
        {
            var roles = await GetRoles();

            if (roles.FirstOrDefault(r => r.Name == User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Role)?.Value) == null || roles.FirstOrDefault(r => r.Name == User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Role)?.Value)?.RolePowers.FirstOrDefault(rp => rp.Power == Domain.Enums.PowerTypes.Create) == null)
            {
                return Unauthorized();
            }

            var result = await service.InsertObject(settingsInsertDTO);

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

        // PUT: api/Settings/5
        [SwaggerOperation(Summary = "This Endpoint updates the specified settings", Description = "")]
        [SwaggerResponse(404, "The id that was given doesn't exist in the db", Type = typeof(void))]
        [SwaggerResponse(400, "The id that was given doesn't equal the id in the given settings object", Type = typeof(void))]
        [SwaggerResponse(401, "Unauthorized", Type = typeof(void))]
        [SwaggerResponse(202, "Something went wrong, please try again later", Type = typeof(void))]
        [SwaggerResponse(204, "Confirms that the settings was updated successfully", Type = typeof(void))]
        [HttpPut("{id}")]
        public async Task<IActionResult> PutSettings(int id, SettingsUpdateDTO settingsUpdateDTO)
        {
            var roles = await GetRoles();

            if (roles.FirstOrDefault(r => r.Name == User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Role)?.Value) == null || roles.FirstOrDefault(r => r.Name == User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Role)?.Value)?.RolePowers.FirstOrDefault(rp => rp.Power == Domain.Enums.PowerTypes.Update) == null)
            {
                return Unauthorized();
            }

            if (id != settingsUpdateDTO.Id)
            {
                return BadRequest("Id doesn't match the id in the object");
            }

            var success = await service.GetObjectWithoutTracking(s => s.Id == id);

            if (success == null)
            {
                return NotFound("Settings doesn't exist in the db");
            }

            var result = await service.UpdateObject(settingsUpdateDTO);

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

        // DELETE: api/Settings/5
        [SwaggerOperation(Summary = "This Endpoint deletes the specified settings", Description = "")]
        [SwaggerResponse(404, "The id that was given doesn't exist in the db", Type = typeof(void))]
        [SwaggerResponse(401, "Unauthorized", Type = typeof(void))]
        [SwaggerResponse(202, "Something went wrong, please try again later", Type = typeof(void))]
        [SwaggerResponse(204, "Confirms that the settings was deleted successfully", Type = typeof(void))]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteSettings(int id)
        {
            var roles = await GetRoles();

            if (roles.FirstOrDefault(r => r.Name == User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Role)?.Value) == null || roles.FirstOrDefault(r => r.Name == User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Role)?.Value)?.RolePowers.FirstOrDefault(rp => rp.Power == Domain.Enums.PowerTypes.Delete) == null)
            {
                return Unauthorized();
            }

            var success = await service.GetObjectWithoutTracking(s => s.Id == id);

            if (success == null)
            {
                return NotFound("Settings doesn't exist in the db");
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

        private async Task<List<ApplicationRoles>> GetRoles()
        {
            var roleList = await roleManager.Roles.Include(r => r.RolePowers).Where(r => r.Name != "Admin" && r.Name != "Merchant" && r.Name != "Representative").ToListAsync();

            //var rolesStringBuilder = new StringBuilder();

            //rolesStringBuilder.Append(roleList[0].Name);

            //for (int i = 1; i < roleList.Count; i++)
            //{
            //    rolesStringBuilder.Append($",{roleList[i].Name}");
            //}

            return roleList;
        }
    }
}
