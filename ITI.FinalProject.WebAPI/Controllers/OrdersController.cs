using Application.DTOs.DisplayDTOs;
using Application.DTOs.InsertDTOs;
using Application.DTOs.UpdateDTOs;
using Application.Interfaces;
using Application.Interfaces.ApplicationServices;
using Domain.Entities;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace ITI.FinalProject.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrdersController : ControllerBase
    {
        private readonly IGenericService<Order, DisplayOrderDTO, InsertOrderDTO, UpdateOrderDTO, int> _orderService;
        private readonly IOrderService _orderServicePagination;
        public OrdersController(IGenericService<Order, DisplayOrderDTO, InsertOrderDTO, UpdateOrderDTO, int> orderService, IOrderService orderServicePagination)
        {
            _orderService = orderService;
            _orderServicePagination = orderServicePagination;
        }

        // GET: api/Orders
        [SwaggerOperation(Summary = "This Endpoint returns a list of orders",Description = "")]
        [SwaggerResponse(404, "There weren't any orders in the database", Type = typeof(void))]
        [SwaggerResponse(200, "Returns A list of orders", Type = typeof(List<DisplayOrderDTO>))]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<DisplayOrderDTO>>> GetOrders([FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10)
        {
            var (orders, totalOrders) = await _orderServicePagination.GetPaginatedOrders(pageNumber, pageSize);

            if (orders == null || orders.Count == 0)
            {
                return NotFound();
            }

            var totalPages = (int)Math.Ceiling(totalOrders / (double)pageSize);
            Response.Headers.Add("X-Total-Count", totalOrders.ToString());
            Response.Headers.Add("X-Total-Pages", totalPages.ToString());

            return Ok(orders);
        }


        // GET: api/Orders/5
        [SwaggerOperation(Summary = "This Endpoint returns the specified order")]
        [SwaggerResponse(404, "The id that was given doesn't exist in the db", Type = typeof(void))]
        [SwaggerResponse(200, "Returns the specified order", Type = typeof(DisplayOrderDTO))]
        [HttpGet("{id}")]
        public async Task<ActionResult<DisplayOrderDTO>> GetOrder(int id)
        {
            var order = await _orderService.GetObject(o => o.Id == id);

            if (order == null)
            {
                return NotFound();
            }

            return Ok(order);
        }

        // POST: api/Orders
        [SwaggerOperation(Summary = "This Endpoint inserts a new order in the db", Description = "")]
        [SwaggerResponse(202, "Something went wrong, please try again later", Type = typeof(void))]
        [SwaggerResponse(204, "Confirms that the order was inserted successfully", Type = typeof(void))]
        [HttpPost]
        public async Task<IActionResult> PostOrder([FromBody] InsertOrderDTO orderDTO)
        {
            if (await _orderService.InsertObject(orderDTO))
            {
                if (await _orderService.SaveChangesForObject())
                {
                    return NoContent();
                }
            }

            return Accepted();
        }

        // PUT: api/Orders/5
        [SwaggerOperation(Summary = "This Endpoint updates the specified order", Description = "")]
        [SwaggerResponse(404, "The id that was given doesn't exist in the db", Type = typeof(void))]
        [SwaggerResponse(400, "The id that was given doesn't equal the id in the given order object", Type = typeof(void))]
        [SwaggerResponse(202, "Something went wrong, please try again later", Type = typeof(void))]
        [SwaggerResponse(204, "Confirms that the order was updated successfully", Type = typeof(void))]
        [HttpPut("{id}")]
        public async Task<IActionResult> PutOrder(int id, UpdateOrderDTO orderDTO)
        {
            if (id != orderDTO.Id)
            {
                return BadRequest();
            }

            var success = await _orderService.GetObjectWithoutTracking(o => o.Id == id);

            if (success == null)
            {
                return NotFound();
            }

            if (await _orderService.UpdateObject(orderDTO))
            {
                if (await _orderService.SaveChangesForObject())
                {
                    return NoContent();
                }
            }

            return Accepted();
        }

        // DELETE: api/Orders/5
        [SwaggerOperation(Summary = "This Endpoint deletes the specified order", Description = "")]
        [SwaggerResponse(404, "The id that was given doesn't exist in the db", Type = typeof(void))]
        [SwaggerResponse(202, "Something went wrong, please try again later", Type = typeof(void))]
        [SwaggerResponse(204, "Confirms that the order was deleted successfully", Type = typeof(void))]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteOrder(int id)
        {
            var success = await _orderService.GetObjectWithoutTracking(o => o.Id == id);

            if (success == null)
            {
                return NotFound();
            }

            if (await _orderService.DeleteObject(id))
            {
                if (await _orderService.SaveChangesForObject())
                {
                    return NoContent();
                }
            }

            return Accepted();
        }
    }
}
