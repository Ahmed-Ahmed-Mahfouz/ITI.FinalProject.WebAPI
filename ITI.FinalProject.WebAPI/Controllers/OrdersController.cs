﻿using Application.DTOs.DisplayDTOs;
using Application.DTOs.InsertDTOs;
using Application.DTOs.UpdateDTOs;
using Application.Interfaces.ApplicationServices;
using Domain.Entities;
using Domain.Enums;
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
        [SwaggerOperation(Summary = "This Endpoint returns a list of all orders", Description = "")]
        [SwaggerResponse(404, "There weren't any orders in the database", Type = typeof(void))]
        [SwaggerResponse(401, "Unauthorized", Type = typeof(void))]
        [SwaggerResponse(200, "Returns a list of all orders", Type = typeof(List<DisplayOrderDTO>))]
        [HttpGet]
        public async Task<ActionResult<List<DisplayOrderDTO>>> GetAllOrders()
        {
            if (await CheckRole(PowerTypes.Read, true, true))
            {
                return Unauthorized();
            }
            var orders = await _orderService.GetAllObjects(o => o.merchant, o => o.governorate, o => o.city, o => o.branch, o => o.representative, o => o.Products);
            if (orders == null || !orders.Any())
            {
                return NotFound("There weren't any orders in the database");
            }
            return Ok(orders);
        }

        // GET: api/OrderPage
        [SwaggerOperation(Summary = "This Endpoint returns a list of orders with pagination", Description = "")]
        [SwaggerResponse(404, "There weren't any orders in the database", Type = typeof(void))]
        [SwaggerResponse(401, "Unauthorized", Type = typeof(void))]
        [SwaggerResponse(200, "Returns a list of orders with pagination", Type = typeof(PaginationDTO<DisplayOrderDTO>))]
        [HttpGet("/api/OrderPage")]
        public async Task<ActionResult<PaginationDTO<DisplayOrderDTO>>> GetPage([FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10, [FromQuery] OrderStatus? orderStatus = null, [FromQuery] DateTime? startDate = null, [FromQuery] DateTime? endDate = null)
        {
            if (await CheckRole(PowerTypes.Read, true, true))
            {
                return Unauthorized();
            }

            //PaginationDTO<DisplayOrderDTO>? orderPaginationDTO;

            //if (orderFilterDTO == null)
            //{
            //    orderPaginationDTO = await _orderService.GetPaginatedOrders(pageNumber, pageSize, o => 1 == 1);
            //}
            //else
            //{
            //    orderPaginationDTO = await _orderService.GetPaginatedOrders(pageNumber, pageSize, o => o.Status == orderFilterDTO.OrderStatus && o.Date > orderFilterDTO.StartDate && o.Date < orderFilterDTO.EndDate);
            //}

            //if (orderStatus != null)
            //{
            //    orderPaginationDTO = await _orderService.GetPaginatedOrders(pageNumber, pageSize, o => o.Status == orderStatus && o.Date > startDate && o.Date < endDate);
            //}
            //else
            //{
            //    orderPaginationDTO = await _orderService.GetPaginatedOrders(pageNumber, pageSize, o => o.Date > startDate && o.Date < endDate);
            //}

            var orderPaginationDTO = await _orderService.GetPaginatedOrders(pageNumber, pageSize, o => (orderStatus != null ? o.Status == orderStatus : true) && (startDate != null ? o.Date > startDate : true) && (endDate != null ? o.Date < endDate : true));


            //if (orderPaginationDTO == null || orderPaginationDTO.List.Count == 0)
            //{
            //    return NotFound();
            //}

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
            if (await CheckRole(PowerTypes.Read, true, true))
            {
                return Unauthorized();
            }

            var order = await _orderService.GetObject(o => o.Id == id, o => o.merchant, o => o.governorate, o => o.city, o => o.branch, o => o.representative, o => o.Products);

            if (order == null)
            {
                return NotFound();
            }

            return Ok(order);
        }

        // POST: api/Orders
        [SwaggerOperation(Summary = "This Endpoint inserts a new order in the db", Description = "")]
        [SwaggerResponse(401, "Unauthorized", Type = typeof(void))]
        [SwaggerResponse(202, "Something went wrong, please try again later", Type = typeof(string))]
        [SwaggerResponse(204, "Confirms that the order was inserted successfully", Type = typeof(void))]
        [HttpPost]
        public async Task<IActionResult> PostOrder([FromBody] InsertOrderDTO orderDTO)
        {
            if (await CheckRole(PowerTypes.Create, true, false))
            {
                return Unauthorized();
            }

            var result = await _orderService.InsertObject(orderDTO);

            if (result.Succeeded)
            {
                //if (await _orderService.SaveChangesForObject())
                //{
                //    return NoContent();
                //}
                //else
                //{
                //    return Accepted("Error saving changes");
                //}
                return NoContent();
            }

            return Accepted(result.Message);
            //return StatusCode(StatusCodes.Status500InternalServerError, result.Message);
        }

        // PUT: api/Orders/5
        [SwaggerOperation(Summary = "This Endpoint updates the specified order", Description = "")]
        [SwaggerResponse(404, "The id that was given doesn't exist in the db", Type = typeof(string))]
        [SwaggerResponse(400, "The id that was given doesn't equal the id in the given order object", Type = typeof(string))]
        [SwaggerResponse(401, "Unauthorized", Type = typeof(void))]
        [SwaggerResponse(202, "Something went wrong, please try again later", Type = typeof(string))]
        [SwaggerResponse(204, "Confirms that the order was updated successfully", Type = typeof(void))]
        [HttpPut("{id}")]
        public async Task<IActionResult> PutOrder(int id, UpdateOrderDTO orderDTO)
        {
            if (await CheckRole(PowerTypes.Update, true, true))
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
        [SwaggerResponse(404, "The id that was given doesn't exist in the db", Type = typeof(string))]
        [SwaggerResponse(401, "Unauthorized", Type = typeof(void))]
        [SwaggerResponse(202, "Something went wrong, please try again later", Type = typeof(string))]
        [SwaggerResponse(204, "Confirms that the order was deleted successfully", Type = typeof(void))]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteOrder(int id)
        {
            if (await CheckRole(PowerTypes.Delete, true, true))
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

        private async Task<bool> CheckRole(PowerTypes powerType, bool isAdminAllowed, bool isRepresentativeAllowed)
        {
            var role = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Role)?.Value;


            if (role == null)
            {
                return true;
            }

            if (role == "Merchant")
            {
                return false;
            }

            if (isAdminAllowed)
            {                
                if (role == "Admin")
                {
                    return false;
                }
            }

            if (isRepresentativeAllowed)
            {
                if (role == "Representative")
                {
                    return false;
                }
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
                    if ((!rolePowers.RolePowers.FirstOrDefault(rp => rp.TableName.ToString() == controllerName)?.Create) ?? true)
                    {
                        return true;
                    }
                    break;
                case PowerTypes.Read:
                    if ((!rolePowers.RolePowers.FirstOrDefault(rp => rp.TableName.ToString() == controllerName)?.Read) ?? true)
                    {
                        return true;
                    }
                    break;
                case PowerTypes.Update:
                    if ((!rolePowers.RolePowers.FirstOrDefault(rp => rp.TableName.ToString() == controllerName)?.Update) ?? true)
                    {
                        return true;
                    }
                    break;
                case PowerTypes.Delete:
                    if ((!rolePowers.RolePowers.FirstOrDefault(rp => rp.TableName.ToString() == controllerName)?.Delete) ?? true)
                    {
                        return true;
                    }
                    break;
            }

            return false;
        }
    }
}
