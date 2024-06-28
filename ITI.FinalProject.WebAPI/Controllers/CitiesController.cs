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
        IGenericService<City, CityDisplayDTO, CityInsertDTO, CityUpdateDTO> CityServ;
        public CitiesController(IGenericService<City, CityDisplayDTO, CityInsertDTO, CityUpdateDTO> _CityServ)
        {
            CityServ = _CityServ;
        }
        [HttpGet]
        public async Task<ActionResult> getAllCityes()
        {
            List<CityDisplayDTO> Cityes = await CityServ.GetAllObjects();
            return Ok(Cityes);

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
        public  ActionResult addCity(CityInsertDTO City)
        {
            if (City == null)
                return BadRequest();
            var result = CityServ.InsertObject(City);
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

    }
}
