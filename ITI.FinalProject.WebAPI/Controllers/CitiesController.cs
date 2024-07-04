using Application.DTOs.DisplayDTOs;
using Application.DTOs.InsertDTOs;
using Application.DTOs.UpdateDTOs;
using Application.Interfaces.ApplicationServices;
using Domain.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace ITI.FinalProject.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CitiesController : ControllerBase
    {
        IGenericService<City, CityDisplayDTO, CityInsertDTO, CityUpdateDTO,int> CityServ;
        public CitiesController(IGenericService<City, CityDisplayDTO, CityInsertDTO, CityUpdateDTO,int> _CityServ)
        {
            CityServ = _CityServ;
        }

        [SwaggerOperation(
        Summary = "This Endpoint returns a list of cities",
            Description = ""
        )]
        [SwaggerResponse(404, "There weren't any cities in the database", Type = typeof(void))]
        [SwaggerResponse(200, "Returns A list of cities", Type = typeof(List<CityDisplayDTO>))]
        [HttpGet]
        public async Task<ActionResult> getAllCities()
        {
            List<CityDisplayDTO> Cities = await CityServ.GetAllObjects();
            if(Cities.Count==0)
            {
                return NotFound();
            }
            return Ok(Cities);

        }


        [SwaggerOperation(
       Summary = "This Endpoint returns the specified city",
           Description = ""
       )]
        [SwaggerResponse(404, "The id that was given doesn't exist in the db", Type = typeof(void))]
        [SwaggerResponse(200, "Returns the specified city", Type = typeof(CityDisplayDTO))]
        [HttpGet("id")]
        public async Task<ActionResult> getById(int id)
        {
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
        [SwaggerResponse(201, "Confirms that the city was inserted successfully", Type = typeof(void))]
        [SwaggerResponse(500, "Something went wrong, please try again later", Type = typeof(void))]

        [HttpPost]
        public async Task <ActionResult> addCity(CityInsertDTO City)
        {
            if (City == null)
                return BadRequest();
            var result =await CityServ.InsertObject(City);
            if (result.Succeeded)
            {
                return Created();
            }
            return StatusCode(StatusCodes.Status500InternalServerError,result.Message);


        }

        [SwaggerOperation(
      Summary = "This Endpoint inserts a city element in the db",
          Description = ""
      )]
        [SwaggerResponse(404, "The id that was given doesn't exist in the db", Type = typeof(void))]
        [SwaggerResponse(200, "Confirms that the city was deleted successfully", Type = typeof(void))]
        [SwaggerResponse(500, "Something went wrong, please try again later", Type = typeof(void))]

        [HttpDelete]
        public async Task<ActionResult> deleteCity(int id)
        {
            CityDisplayDTO? city =await CityServ.GetObjectWithoutTracking(c => c.id == id);
            if(city == null)
            {
                return NotFound();
            }

            var result =await CityServ.DeleteObject(id);
            if (result.Succeeded)
            {
                return Ok(result);
            }
            return StatusCode(StatusCodes.Status500InternalServerError);


        }
        [SwaggerOperation(
       Summary = "This Endpoint updates the specified city",
           Description = ""
       )]
        [SwaggerResponse(404, "The id that was given doesn't exist in the db", Type = typeof(void))]
        [SwaggerResponse(400, "The id that was given doesn't equal the id in the given city object", Type = typeof(void))]
        [SwaggerResponse(500, "Something went wrong, please try again later", Type = typeof(void))]
        [SwaggerResponse(200, "Confirms that the city was updated successfully", Type = typeof(void))]

        [HttpPut("id")]
        public async Task<ActionResult> updateCity(int id, CityUpdateDTO city)
        {
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
            return StatusCode(StatusCodes.Status500InternalServerError);
        }

    }
}
