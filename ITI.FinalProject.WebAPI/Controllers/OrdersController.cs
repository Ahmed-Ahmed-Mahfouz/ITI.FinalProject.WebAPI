using Application.DTOs.DisplayDTOs;
using Application.DTOs.InsertDTOs;
using Application.DTOs.UpdateDTOs;
using Application.Interfaces.ApplicationServices;
using Application.Services;
using Domain.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ITI.FinalProject.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrdersController : ControllerBase
    {
        private readonly IGenericService<Order, DisplayOrderDTO, InsertOrderDTO, UpdateOrderDTO, int> _orderService;
        public OrdersController(IGenericService<Order, DisplayOrderDTO, InsertOrderDTO, UpdateOrderDTO, int> orderService)
        {
            _orderService = orderService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<DisplayOrderDTO>>> GetOrders()
        {
            var orders = await _orderService.GetAllObjects();
            return Ok(orders);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<DisplayOrderDTO>> GetOrder(int id)
        {
            var order = await _orderService.GetObject(o => o.Id == id);

            if (order == null)
            {
                return NotFound();
            }

            return order;
        }

        [HttpPost]
        public async Task<IActionResult> PostOrder(InsertOrderDTO orderDTO)
        {
            var success = await _orderService.InsertObject(orderDTO);

            if (!success)
            {
                return BadRequest();
            }

            return StatusCode(201);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutOrder(int id, UpdateOrderDTO orderDTO)
        {
            if (id != orderDTO.Id)
            {
                return BadRequest();
            }

            var success = await _orderService.UpdateObject(orderDTO);

            if (!success)
            {
                return BadRequest();
            }

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteOrder(int id)
        {
            var success = await _orderService.DeleteObject(id);

            if (!success)
            {
                return NotFound();
            }

            return NoContent();
        }
    }
}
