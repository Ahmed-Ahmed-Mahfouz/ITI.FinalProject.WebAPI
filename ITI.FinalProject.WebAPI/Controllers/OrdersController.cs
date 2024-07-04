using Application.DTOs.DisplayDTOs;
using Application.DTOs.InsertDTOs;
using Application.DTOs.UpdateDTOs;
using Application.Interfaces.ApplicationServices;
using Domain.Entities;
using ITI.FinalProject.WebAPI.DTOs;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Swashbuckle.AspNetCore.Annotations;
using System.Security.Claims;

namespace ITI.FinalProject.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrdersController : ControllerBase
    {
        private readonly IPaginationService<Order, DisplayOrderDTO, InsertOrderDTO, UpdateOrderDTO, int> _orderService;
        private readonly RoleManager<ApplicationRoles> roleManager;

        public OrdersController(IPaginationService<Order, DisplayOrderDTO, InsertOrderDTO, UpdateOrderDTO, int> orderService, RoleManager<ApplicationRoles> roleManager)
        {
            _orderService = orderService;
            this.roleManager = roleManager;
        }

        // GET: api/Orders
        [SwaggerOperation(Summary = "This Endpoint returns a list of orders",Description = "")]
        [SwaggerResponse(404, "There weren't any orders in the database", Type = typeof(void))]
        [SwaggerResponse(401, "Unauthorized", Type = typeof(void))]
        [SwaggerResponse(200, "Returns A list of orders", Type = typeof(PaginationDTO<DisplayOrderDTO>))]
        [HttpGet]
        public async Task<ActionResult<PaginationDTO<DisplayOrderDTO>>> GetOrders([FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10, OrderFilterDTO? orderFilterDTO = null)
        {
            var roles = await roleManager.Roles.Include(r => r.RolePowers).ToListAsync();

            if (roles.FirstOrDefault(r => r.Name == User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Role)?.Value) == null || roles.FirstOrDefault(r => r.Name == User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Role)?.Value)?.RolePowers.FirstOrDefault(rp => rp.Power == Domain.Enums.PowerTypes.Read) == null)
            {
                return Unauthorized();
            }

            PaginationDTO<DisplayOrderDTO>? orderPaginationDTO;

            if (orderFilterDTO == null)
            {
                orderPaginationDTO = await _orderService.GetPaginatedOrders(pageNumber, pageSize, o => 1 == 1);
            }
            else
            {
                orderPaginationDTO = await _orderService.GetPaginatedOrders(pageNumber, pageSize, o => o.Status == orderFilterDTO.OrderStatus && o.Date > orderFilterDTO.StartDate && o.Date < orderFilterDTO.EndDate);
            }

            if (orderPaginationDTO == null || orderPaginationDTO.List.Count == 0)
            {
                return NotFound();
            }

            //var totalPages = (int)Math.Ceiling(totalOrders / (double)pageSize);
            //Response.Headers.Add("X-Total-Count", totalOrders.ToString());
            //Response.Headers.Add("X-Total-Pages", totalPages.ToString());

            return Ok(orderPaginationDTO);
        }


        // GET: api/Orders/5
        [SwaggerOperation(Summary = "This Endpoint returns the specified order")]
        [SwaggerResponse(404, "The id that was given doesn't exist in the db", Type = typeof(void))]
        [SwaggerResponse(401, "Unauthorized", Type = typeof(void))]
        [SwaggerResponse(200, "Returns the specified order", Type = typeof(DisplayOrderDTO))]
        [HttpGet("{id}")]
        public async Task<ActionResult<DisplayOrderDTO>> GetOrder(int id)
        {
            var roles = await roleManager.Roles.Include(r => r.RolePowers).ToListAsync();

            if (roles.FirstOrDefault(r => r.Name == User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Role)?.Value) == null || roles.FirstOrDefault(r => r.Name == User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Role)?.Value)?.RolePowers.FirstOrDefault(rp => rp.Power == Domain.Enums.PowerTypes.Read) == null)
            {
                return Unauthorized();
            }

            var order = await _orderService.GetObject(o => o.Id == id);

            if (order == null)
            {
                return NotFound();
            }

            return Ok(order);
        }

        // POST: api/Orders
        [SwaggerOperation(Summary = "This Endpoint inserts a new order in the db", Description = "")]
        [SwaggerResponse(401, "Unauthorized", Type = typeof(void))]
        [SwaggerResponse(202, "Something went wrong, please try again later", Type = typeof(void))]
        [SwaggerResponse(204, "Confirms that the order was inserted successfully", Type = typeof(void))]
        [HttpPost]
        public async Task<IActionResult> PostOrder([FromBody] InsertOrderDTO orderDTO)
        {
            if (!User.IsInRole("Merchant"))
            {
                return Unauthorized();
            }

            var result = await _orderService.InsertObject(orderDTO);

            if (result.Succeeded)
            {
                if (await _orderService.SaveChangesForObject())
                {
                    return NoContent();
                }
                else
                {
                    return Accepted("Error saving changes");
                }
            }

            return Accepted(result.Message);
            //return StatusCode(StatusCodes.Status500InternalServerError, result.Message);
        }

        // PUT: api/Orders/5
        [SwaggerOperation(Summary = "This Endpoint updates the specified order", Description = "")]
        [SwaggerResponse(404, "The id that was given doesn't exist in the db", Type = typeof(void))]
        [SwaggerResponse(400, "The id that was given doesn't equal the id in the given order object", Type = typeof(void))]
        [SwaggerResponse(401, "Unauthorized", Type = typeof(void))]
        [SwaggerResponse(202, "Something went wrong, please try again later", Type = typeof(void))]
        [SwaggerResponse(204, "Confirms that the order was updated successfully", Type = typeof(void))]
        [HttpPut("{id}")]
        public async Task<IActionResult> PutOrder(int id, UpdateOrderDTO orderDTO)
        {
            var roles = await roleManager.Roles.Include(r => r.RolePowers).ToListAsync();

            if (roles.FirstOrDefault(r => r.Name == User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Role)?.Value) == null || roles.FirstOrDefault(r => r.Name == User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Role)?.Value)?.RolePowers.FirstOrDefault(rp => rp.Power == Domain.Enums.PowerTypes.Update) == null)
            {
                return Unauthorized();
            }

            if (id != orderDTO.Id)
            {
                return BadRequest("Id doesn't match the id in the object");
            }

            var success = await _orderService.GetObjectWithoutTracking(o => o.Id == id);

            if (success == null)
            {
                return NotFound("Order doesn't exist in the db");
            }

            var result = await _orderService.UpdateObject(orderDTO);

            if (result.Succeeded)
            {
                if (await _orderService.SaveChangesForObject())
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

        // DELETE: api/Orders/5
        [SwaggerOperation(Summary = "This Endpoint deletes the specified order", Description = "")]
        [SwaggerResponse(404, "The id that was given doesn't exist in the db", Type = typeof(void))]
        [SwaggerResponse(401, "Unauthorized", Type = typeof(void))]
        [SwaggerResponse(202, "Something went wrong, please try again later", Type = typeof(void))]
        [SwaggerResponse(204, "Confirms that the order was deleted successfully", Type = typeof(void))]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteOrder(int id)
        {
            var roles = await roleManager.Roles.Include(r => r.RolePowers).ToListAsync();

            if (roles.FirstOrDefault(r => r.Name == User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Role)?.Value) == null || roles.FirstOrDefault(r => r.Name == User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Role)?.Value)?.RolePowers.FirstOrDefault(rp => rp.Power == Domain.Enums.PowerTypes.Delete) == null)
            {
                return Unauthorized();
            }

            var success = await _orderService.GetObjectWithoutTracking(o => o.Id == id);

            if (success == null)
            {
                return NotFound("Order doesn't exist in the db");
            }

            var result = await _orderService.DeleteObject(id);

            if (result.Succeeded)
            {
                if (await _orderService.SaveChangesForObject())
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
