using Application.DTOs.DisplayDTOs;
using Application.DTOs.InsertDTOs;
using Application.DTOs.UpdateDTOs;
using Application.Interfaces.ApplicationServices;
using Domain.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ITI.FinalProject.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ShippingController : ControllerBase
    {
        private readonly IGenericService<Shipping, DisplayShippingDTO, InsertShippingDTO, UpdateShippingDTO, int> _shippingService;
        public ShippingController(IGenericService<Shipping, DisplayShippingDTO, InsertShippingDTO, UpdateShippingDTO, int> shippingService)
        {
            _shippingService = shippingService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<DisplayShippingDTO>>> GetShippings()
        {
            var shippings = await _shippingService.GetAllObjects();
            return Ok(shippings);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<DisplayShippingDTO>> GetShipping(int id)
        {
            var shipping = await _shippingService.GetObject(s => s.Id == id);

            if (shipping == null)
            {
                return NotFound();
            }

            return shipping;
        }

        [HttpPost]
        public async Task<IActionResult> PostShipping(InsertShippingDTO shippingDTO)
        {
            var success = await _shippingService.InsertObject(shippingDTO);

            if (!success)
            {
                return BadRequest();
            }

            return StatusCode(201);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutShipping(int id, UpdateShippingDTO shippingDTO)
        {
            if (id != shippingDTO.Id)
            {
                return BadRequest();
            }

            var success = await _shippingService.UpdateObject(shippingDTO);

            if (!success)
            {
                return BadRequest();
            }

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteShipping(int id)
        {
            var success = await _shippingService.DeleteObject(id);

            if (!success)
            {
                return BadRequest();
            }

            return NoContent();
        }
    }
}
