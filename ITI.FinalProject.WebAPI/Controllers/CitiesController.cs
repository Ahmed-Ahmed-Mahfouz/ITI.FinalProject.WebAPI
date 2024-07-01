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
    public class CitiesController : ControllerBase
    {
        IGenericService<City, CityDisplayDTO, CityInsertDTO, CityUpdateDTO,int> CityServ;
        public CitiesController(IGenericService<City, CityDisplayDTO, CityInsertDTO, CityUpdateDTO,int> _CityServ)
        {
            CityServ = _CityServ;
        }
        [HttpGet]
        public async Task<ActionResult> getAllCities()
        {
            List<CityDisplayDTO> Cities = await CityServ.GetAllObjects();
            return Ok(Cities);

        }
        [HttpGet("id")]
        public async Task<ActionResult> getById(int id)
        {
            CityDisplayDTO? City = await CityServ.GetObject(p=>p.id==id);
            if (City == null)
                return NotFound();
            return Ok(City);

        }
        [HttpPost]
        public async Task <ActionResult> addCity(CityInsertDTO City)
        {
            if (City == null)
                return BadRequest();
            var result =await CityServ.InsertObject(City);
            if (result)
            {
                return Ok(result);
            }
            return BadRequest();


        }
        [HttpDelete]
        public async Task<ActionResult> deleteCity(int id)
        {

            bool result =await CityServ.DeleteObject(id);
            if (result)
            {
                return Ok(result);
            }
            return BadRequest();


        }
        [HttpPut("id")]
        public async Task<ActionResult> updateCity(int id, CityUpdateDTO city)
        {
            if (city == null || id != city.id)
            {
                return BadRequest();
            }

            var result = await CityServ.UpdateObject(city);
            if (result)
            {
                return Ok(city);
            }
            return BadRequest();
        }

    }
}
