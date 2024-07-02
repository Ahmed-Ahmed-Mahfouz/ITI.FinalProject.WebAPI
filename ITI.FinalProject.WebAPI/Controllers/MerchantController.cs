using Application.DTOs.DisplayDTOs;
using Application.DTOs.InsertDTOs;
using Application.DTOs.UpdateDTOs;
using Application.Interfaces;
using Domain.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System.ComponentModel.DataAnnotations;

namespace ITI.FinalProject.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MerchantController : ControllerBase
    {
        private readonly IMerchantService _MerchantService;
        private readonly ILogger<MerchantController> _logger;

        public MerchantController(IMerchantService MerchantService, ILogger<MerchantController> logger)
        {
            _MerchantService = MerchantService;
            _logger = logger;
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
            IEnumerable<MerchantResponseDto>? response = await _MerchantService.GetAllObjects();
            return Ok(response?.ToList());
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
            MerchantResponseDto? response = await _MerchantService.GetObject(m => m.Id == id);
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
            IEnumerable<MerchantResponseDto>? Merchants = await _MerchantService.GetFilteredMerchantsAsync(searchString);
            return Ok(Merchants?.ToList());
        }

        [SwaggerOperation(
        Summary = "This Endpoint inserts a merchant element in the db",
        Description = ""
        )]
        [SwaggerResponse(200, "Confirms that the merchant was inserted successfully", Type = typeof(MerchantAddDto))]
        [SwaggerResponse(400, "Validation errors occurred while inserting the merchant", Type = typeof(void))]
        [HttpPost]
        public async Task<ActionResult> AddMerchant(MerchantAddDto MerchantAddDto)
        {
            var errors = await _MerchantService.InsertObject(MerchantAddDto);
            if (errors?.Count == 0)
                return Ok(MerchantAddDto);
            return BadRequest(string.Join(", ", errors.Select(err => err.ErrorMessage)));
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
            List<ValidationResult>? errors = await _MerchantService.UpdateObject(MerchantUpdateDto);
            if (errors?.Count == 0)
            {
                MerchantResponseDto? updatedMerchant = await _MerchantService.GetObject(m => m.Id == MerchantId);
                return Ok(updatedMerchant);
            }
            else
                return BadRequest(string.Join(", ", errors.Select(err => err.ErrorMessage)));
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
            bool isDeleted = await _MerchantService.DeleteObject(MerchantId);
            if (isDeleted)
                return NoContent();

            return NotFound();
        }
    }
}
