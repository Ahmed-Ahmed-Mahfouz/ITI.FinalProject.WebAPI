using Application.DTOs.DisplayDTOs;
using Application.DTOs.InsertDTOs;
using Application.DTOs.UpdateDTOs;
using Application.Interfaces;
using Application.Interfaces.ApplicationServices;
using Domain.Entities;
using Domain.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

using Microsoft.EntityFrameworkCore;

using Swashbuckle.AspNetCore.Annotations;
using System.ComponentModel.DataAnnotations;
using System.Security.Claims;

namespace ITI.FinalProject.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MerchantController : ControllerBase

    {
        // private readonly IPaginationService _MerchantService;
        //private readonly ILogger<MerchantController> _logger;
        private readonly IPaginationService<Merchant, MerchantResponseDto, MerchantAddDto, MerchantUpdateDto, string> merchantService;

        

        public MerchantController( IPaginationService<Merchant, MerchantResponseDto, MerchantAddDto, MerchantUpdateDto, string> merchantService)
        {
            this.merchantService= merchantService;
         }

    { 
        private readonly IPaginationService<Merchant, MerchantResponseDto, MerchantAddDto, MerchantUpdateDto, string> merchantService;
        private readonly RoleManager<ApplicationRoles> roleManager;

        //private readonly UserManager<ApplicationUser> userManager;

        public MerchantController(IPaginationService<Merchant, MerchantResponseDto, MerchantAddDto, MerchantUpdateDto, string> merchantService, RoleManager<ApplicationRoles> roleManager)
        {
            this.merchantService = merchantService;
            this.roleManager = roleManager;
            //this.userManager = userManager;
        }


        [SwaggerOperation(
        Summary = "This Endpoint returns a list of merchants",
        Description = ""
        )]
        [SwaggerResponse(200, "Returns a list of merchants", Type = typeof(IEnumerable<MerchantResponseDto>))]
        [SwaggerResponse(404, "There weren't any merchants in the database", Type = typeof(void))]

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            IEnumerable<MerchantResponseDto> response = await merchantService.GetAllObjects();

        [SwaggerResponse(401, "Unauthorized", Type = typeof(void))]
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            if (await CheckRole(PowerTypes.Read))
            {
                return Unauthorized();
            }

            //var user = await userManager.FindByIdAsync(User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value);
            //var user = User;
            //if ((user.Claims.ToList())[0]?.Value != "Admin")
            //{
            //    return Unauthorized();
            //}

            //if (User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Role)?.Value != "Admin")
            //{
            //    return Unauthorized();
            //}

            IEnumerable<MerchantResponseDto> response = await merchantService.GetAllObjects(m => m.governorate, m => m.city, m => m.user, m => m.SpecialPackages);

            if (response != null)
            {

                return Ok(response.ToList());
            }
            else
            {
                return NotFound();
            }
        }


        [SwaggerOperation(
        Summary = "This Endpoint returns a list of merchants with the specified page size",
            Description = ""
        )]
        [SwaggerResponse(401, "Unauthorized", Type = typeof(void))]
        [SwaggerResponse(200, "Returns A list of merchants", Type = typeof(PaginationDTO<MerchantResponseDto>))]
        [HttpGet("/api/MerchantPage")]
        public async Task<ActionResult<PaginationDTO<MerchantResponseDto>>> GetPage([FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10, [FromQuery] string name = "")
        {
            if (await CheckRole(PowerTypes.Read))
            {
                return Unauthorized();
            }

            var paginationDTO = await merchantService.GetPaginatedOrders(pageNumber, pageSize, m => m.user.UserName.Trim().ToLower().Contains(name.Trim().ToLower()));
            //paginationDTO.List = paginationDTO.List.Where(m => m.UserName.Trim().ToLower().Contains(name.Trim().ToLower())).ToList();

            return Ok(paginationDTO);
        }


        [SwaggerOperation(
        Summary = "This Endpoint returns the specified merchant",
        Description = ""
        )]
        [SwaggerResponse(200, "Returns the specified merchant", Type = typeof(MerchantResponseDto))]
        [SwaggerResponse(404, "The id that was given doesn't exist in the db", Type = typeof(void))]

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(string id)
        {
            MerchantResponseDto? response = await merchantService.GetObject(m => m.Id == id);

        [SwaggerResponse(401, "Unauthorized", Type = typeof(void))]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(string id)
        {
            if (await CheckRole(PowerTypes.Read))
            {
                return Unauthorized();
            }

            MerchantResponseDto? response = await merchantService.GetObject(m => m.userId == id, m => m.governorate, m => m.city, m => m.user, m => m.SpecialPackages);

            if (response == null)
                return NotFound();
            return Ok(response);
        }



        [SwaggerOperation(
        Summary = "This Endpoint returns a list of merchants filtered by a search string",
        Description = ""
        )]
        [SwaggerResponse(200, "Returns a list of filtered merchants", Type = typeof(IEnumerable<MerchantResponseDto>))]
        [SwaggerResponse(400, "The search string is empty or null", Type = typeof(void))]
        [HttpGet("filtered")]
        public async Task<IActionResult> GetFilteredMerchants([FromQuery] string searchString)
        {
            if (string.IsNullOrEmpty(searchString) || string.IsNullOrWhiteSpace(searchString))
            {
                return BadRequest();
            }
            IEnumerable<MerchantResponseDto>? Merchants = await merchantService.GetAllObjects(o=>searchString);
            return Ok(Merchants?.ToList());
        }

        //[SwaggerOperation(
        //Summary = "This Endpoint returns a list of merchants filtered by a search string",
        //Description = ""
        //)]
        //[SwaggerResponse(200, "Returns a list of filtered merchants", Type = typeof(IEnumerable<MerchantResponseDto>))]
        //[SwaggerResponse(400, "The search string is empty or null", Type = typeof(void))]
        //[SwaggerResponse(401, "Unauthorized", Type = typeof(void))]
        //[HttpGet("filtered")]
        //public async Task<IActionResult> GetFilteredMerchants([FromQuery] string searchString)
        //{
        //    if (await CheckRole(PowerTypes.Read))
        //    {
        //        return Unauthorized();
        //    }

        //    if (string.IsNullOrEmpty(searchString) || string.IsNullOrWhiteSpace(searchString))
        //    {
        //        return BadRequest();
        //    }
        //    IEnumerable<MerchantResponseDto>? Merchants = await merchantService.GetAllObjects(o => searchString);
        //    return Ok(Merchants?.ToList());
        //}


        [SwaggerOperation(
        Summary = "This Endpoint inserts a merchant element in the db",
        Description = ""
        )]
        [SwaggerResponse(200, "Confirms that the merchant was inserted successfully", Type = typeof(MerchantAddDto))]

        [SwaggerResponse(400, "Validation errors occurred while inserting the merchant", Type = typeof(void))]
        [HttpPost]
        public async Task<ActionResult> AddMerchant(MerchantAddDto MerchantAddDto)
        {

        [SwaggerResponse(400, "Validation errors occurred while inserting the merchant", Type = typeof(string))]
        [SwaggerResponse(401, "Unauthorized", Type = typeof(void))]
        [HttpPost]
        public async Task<ActionResult> AddMerchant(MerchantAddDto MerchantAddDto)
        {
            if (await CheckRole(PowerTypes.Create))
            {
                return Unauthorized();
            }

            var errors = await merchantService.InsertObject(MerchantAddDto);
            if (errors.Succeeded == true)
                return Ok(MerchantAddDto);
            return BadRequest(string.Join(", ", errors.Message));
        }

        [SwaggerOperation(
        Summary = "This Endpoint updates the specified merchant",
        Description = ""
        )]
        [SwaggerResponse(200, "Confirms that the merchant was updated successfully", Type = typeof(MerchantResponseDto))]

        [SwaggerResponse(400, "Validation errors occurred while updating the merchant", Type = typeof(void))]
        [HttpPut("{MerchantId}")]
        public async Task<IActionResult> UpdateMerchant(string MerchantId, MerchantUpdateDto MerchantUpdateDto)
        {
            ModificationResultDTO error = await merchantService.UpdateObject(MerchantUpdateDto);
            if (error.Succeeded == true)
            {
                MerchantResponseDto? updatedMerchant = await merchantService.GetObject(m => m.Id == MerchantId);

        [SwaggerResponse(400, "Validation errors occurred while updating the merchant", Type = typeof(string))]
        [SwaggerResponse(401, "Unauthorized", Type = typeof(void))]
        [HttpPut("{MerchantId}")]
        public async Task<IActionResult> UpdateMerchant(string MerchantId, MerchantUpdateDto MerchantUpdateDto)
        {
            if (await CheckRole(PowerTypes.Update))
            {
                return Unauthorized();
            }

            ModificationResultDTO error = await merchantService.UpdateObject(MerchantUpdateDto);
            if (error.Succeeded == true)
            {
                MerchantResponseDto? updatedMerchant = await merchantService.GetObject(m => m.userId == MerchantId);

                return Ok(updatedMerchant);
            }
            else
                return BadRequest(string.Join(", ", error.Message));
        }

        [SwaggerOperation(
        Summary = "This Endpoint deletes the specified merchant from the db",
        Description = ""
        )]
        [SwaggerResponse(204, "Confirms that the merchant was deleted successfully", Type = typeof(void))]

        [SwaggerResponse(404, "The id that was given doesn't exist in the db", Type = typeof(void))]
        [HttpDelete("{MerchantId}")]
        public async Task<IActionResult> DeleteMerchant(string MerchantId)
        {
            ModificationResultDTO isDeleted = await merchantService.DeleteObject(MerchantId);
            if (isDeleted.Succeeded==false)
                return NoContent();
            else
            return Ok();
        [SwaggerResponse(400, "The id that was given doesn't exist in the db", Type = typeof(string))]
        [SwaggerResponse(401, "Unauthorized", Type = typeof(void))]
        [HttpDelete("{MerchantId}")]
        public async Task<IActionResult> DeleteMerchant(string MerchantId)
        {
            if (await CheckRole(PowerTypes.Delete))
            {
                return Unauthorized();
            }

            ModificationResultDTO isDeleted = await merchantService.DeleteObject(MerchantId);
            if (isDeleted.Succeeded == false)
                return BadRequest(isDeleted.Message);
            else
                return NoContent();
        }

        private async Task<bool> CheckRole(PowerTypes powerType)
        {
            var role = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Role)?.Value;

            if (role == null)
            {
                return true;
            }

            if (role == "Admin")
            {
                return false;
            }

            var rolePowers = await roleManager.Roles.Include(r => r.RolePowers).Where(r => r.Name == role).FirstOrDefaultAsync();

            if (rolePowers == null)
            {
                return true;
            }

            string controllerName = ControllerContext.ActionDescriptor.ControllerName;

            switch (powerType)
            {
                case PowerTypes.Create:
                    if ((!rolePowers.RolePowers.FirstOrDefault(rp => rp.TableName.ToString() == controllerName)?.Create) ?? false)
                    {
                        return true;
                    }
                    break;
                case PowerTypes.Read:
                    if ((!rolePowers.RolePowers.FirstOrDefault(rp => rp.TableName.ToString() == controllerName)?.Read) ?? false)
                    {
                        return true;
                    }
                    break;
                case PowerTypes.Update:
                    if ((!rolePowers.RolePowers.FirstOrDefault(rp => rp.TableName.ToString() == controllerName)?.Update) ?? false)
                    {
                        return true;
                    }
                    break;
                case PowerTypes.Delete:
                    if ((!rolePowers.RolePowers.FirstOrDefault(rp => rp.TableName.ToString() == controllerName)?.Delete) ?? false)
                    {
                        return true;
                    }
                    break;
            }

            return false;

        }
    }
        }
        
