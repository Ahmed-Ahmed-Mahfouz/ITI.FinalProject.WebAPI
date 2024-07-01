using Application.DTOs.DisplayDTOs;
using Application.DTOs.InsertDTOs;
using Application.DTOs.UpdateDTOs;
using Application.Interfaces;
using Domain.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
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

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            IEnumerable<MerchantResponseDto>? response = await _MerchantService.GetAllObjects();
            return Ok(response?.ToList());
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(string id)
        {
            MerchantResponseDto? response = await _MerchantService.GetObject(m=>m.Id == id);
            if (response == null)
                return NotFound();
            return Ok(response);
        }

        //[HttpGet("paginated")]
        //public async Task<ActionResult<PaginationResponse<MerchantResponseDto>>> GetMerchants([FromQuery] PaginationParameters paginationParameters)
        //{
        //    var Merchants = _MerchantService.GetMerchantsPaginated();
        //    _logger.LogError("Merchants", Merchants);
        //    int totalRecords = await Merchants.CountAsync();

        //    List<MerchantResponseDto>? listOfTrsders = await Merchants
        //        .Skip((paginationParameters.PageNumber - 1) * paginationParameters.PageSize)
        //        .Take(paginationParameters.PageSize)
        //        .ToListAsync();
        //    PaginationResponse<MerchantResponseDto> result =
        //        new PaginationResponse<MerchantResponseDto>()
        //        {
        //            Data = listOfTrsders,
        //            PageNo = paginationParameters.PageNumber,
        //            PageSize = paginationParameters.PageSize,
        //            TotalRecords = totalRecords
        //        };
        //    return Ok(result);
        //}

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

        [HttpPost]
        public async Task<ActionResult> AddMerchant(MerchantAddDto MerchantAddDto)
        {
            var errors = await _MerchantService.InsertObject(MerchantAddDto);
            if (errors?.Count == 0)
                return Ok(MerchantAddDto);
            return BadRequest(string.Join(", ", errors.Select(err => err.ErrorMessage)));
        }

        [HttpPut("{MerchantId}")]
        public async Task<IActionResult> UpdateMerchant(string MerchantId, MerchantUpdateDto MerchantUpdateDto)
        {
            List<ValidationResult>? errors = await _MerchantService.UpdateObject(MerchantUpdateDto);
            if (errors?.Count == 0)
            {
                MerchantResponseDto? updatedMerchant = await _MerchantService.GetObject(m=> m.Id==MerchantId);
                return Ok(updatedMerchant);
            }
            else
                return BadRequest(string.Join(", ", errors.Select(err => err.ErrorMessage)));
        }

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
