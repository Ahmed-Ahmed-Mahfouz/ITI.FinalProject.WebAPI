﻿using Application.DTOs.DisplayDTOs;
using Application.DTOs.InsertDTOs;
using Application.DTOs.UpdateDTOs;
using Application.Interfaces.ApplicationServices;
using Domain.Entities;
using Domain.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Swashbuckle.AspNetCore.Annotations;
using System.Security.Claims;

namespace ITI.FinalProject.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CitiesController : ControllerBase
    {
        IPaginationService<City, CityDisplayDTO, CityInsertDTO, CityUpdateDTO,int> CityServ;
        private readonly RoleManager<ApplicationRoles> roleManager;

        public CitiesController(IPaginationService<City, CityDisplayDTO, CityInsertDTO, CityUpdateDTO,int> _CityServ, RoleManager<ApplicationRoles> roleManager)
        {
            CityServ = _CityServ;
            this.roleManager = roleManager;
        }

        [SwaggerOperation(
        Summary = "This Endpoint returns a list of cities",
            Description = ""
        )]
        [SwaggerResponse(404, "There weren't any cities in the database", Type = typeof(void))]
        [SwaggerResponse(401, "Unauthorized", Type = typeof(void))]
        [SwaggerResponse(200, "Returns A list of cities", Type = typeof(List<CityDisplayDTO>))]
        [HttpGet]
        public async Task<ActionResult> GetAllCities()
        {
            if (await CheckRole(PowerTypes.Read))
            {
                return Unauthorized();
            }

            List<CityDisplayDTO> Cities = await CityServ.GetAllObjects();
            if (Cities.Count == 0)
            {
                return NotFound();
            }
            return Ok(Cities);
        }

        [SwaggerOperation(
        Summary = "This Endpoint returns a list of cities with the specified page size",
            Description = ""
        )]
        [SwaggerResponse(401, "Unauthorized", Type = typeof(void))]
        [SwaggerResponse(200, "Returns A list of cities", Type = typeof(PaginationDTO<CityDisplayDTO>))]
        [HttpGet("/api/CityPage")]
        public async Task<ActionResult<PaginationDTO<CityDisplayDTO>>> GetPage([FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10, [FromQuery] string name = "")
        {
            if (await CheckRole(PowerTypes.Read))
            {
                return Unauthorized();
            }

            var paginationDTO = await CityServ.GetPaginatedOrders(pageNumber, pageSize, c => c.name.Trim().ToLower().Contains(name.Trim().ToLower()));

            return Ok(paginationDTO);
        }

        [SwaggerOperation(
        Summary = "This Endpoint returns the specified city",
            Description = ""
        )]
        [SwaggerResponse(404, "The id that was given doesn't exist in the db", Type = typeof(void))]
        [SwaggerResponse(401, "Unauthorized", Type = typeof(void))]
        [SwaggerResponse(200, "Returns the specified city", Type = typeof(CityDisplayDTO))]
        [HttpGet("{id}")]
        public async Task<ActionResult> GetById(int id)
        {
            if (await CheckRole(PowerTypes.Read))
            {
                return Unauthorized();
            }

            CityDisplayDTO? City = await CityServ.GetObject(p=>p.id==id);
            if (City == null)
                return NotFound();
            return Ok(City);

        }

        [SwaggerOperation(
        Summary = "This Endpoint inserts a city element in the db",
           Description = ""
        )]
        [SwaggerResponse(400, "Something went wrong, please check your request", Type = typeof(void))]
        [SwaggerResponse(401, "Unauthorized", Type = typeof(void))]
        [SwaggerResponse(204, "Confirms that the city was inserted successfully", Type = typeof(void))]
        [SwaggerResponse(202, "Something went wrong, please try again later", Type = typeof(string))]
        [HttpPost]
        public async Task <ActionResult> AddCity(CityInsertDTO City)
        {
            if (await CheckRole(PowerTypes.Create))
            {
                return Unauthorized();
            }

            if (City == null)
                return BadRequest();
            var result =await CityServ.InsertObject(City);
            if (result.Succeeded)
            {
                return Created();
            }
            return Accepted(result.Message);


        }

        [SwaggerOperation(
        Summary = "This Endpoint inserts a city element in the db",
          Description = ""
        )]
        [SwaggerResponse(404, "The id that was given doesn't exist in the db", Type = typeof(void))]
        [SwaggerResponse(401, "Unauthorized", Type = typeof(void))]
        [SwaggerResponse(200, "Confirms that the city was deleted successfully", Type = typeof(ModificationResultDTO))]
        [SwaggerResponse(202, "Something went wrong, please try again later", Type = typeof(string))]

        [HttpDelete]
        public async Task<ActionResult> DeleteCity(int id)
        {
            if (await CheckRole(PowerTypes.Delete))
            {
                return Unauthorized();
            }

            CityDisplayDTO? city = await CityServ.GetObjectWithoutTracking(c => c.id == id);
            if (city == null)
            {
                return NotFound();
            }

            var result = await CityServ.DeleteObject(id);
            if (result.Succeeded)
            {
                return Ok(result);
            }

            return Accepted(result.Message);
        }

        [SwaggerOperation(
        Summary = "This Endpoint updates the specified city",
           Description = ""
        )]
        [SwaggerResponse(404, "The id that was given doesn't exist in the db", Type = typeof(void))]
        [SwaggerResponse(400, "The id that was given doesn't equal the id in the given city object", Type = typeof(string))]
        [SwaggerResponse(401, "Unauthorized", Type = typeof(void))]
        [SwaggerResponse(202, "Something went wrong, please try again later", Type = typeof(string))]
        [SwaggerResponse(200, "Confirms that the city was updated successfully", Type = typeof(CityUpdateDTO))]

        [HttpPut("{id}")]
        public async Task<ActionResult> UpdateCity(int id, CityUpdateDTO city)
        {
            if (await CheckRole(PowerTypes.Update))
            {
                return Unauthorized();
            }

            if (city == null || id != city.id)
            {
                return BadRequest("Id doesn't match the id in the object");
            }
            CityDisplayDTO? cityDisplay = await CityServ.GetObjectWithoutTracking(c => c.id == id);
            if (cityDisplay == null)
            {
                return NotFound();
            }

            var result = await CityServ.UpdateObject(city);
            if (result.Succeeded)
            {
                return Ok(city);
            }

            return Accepted(result.Message);
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
