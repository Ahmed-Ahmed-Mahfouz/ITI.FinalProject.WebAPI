using Application.DTOs;
using Application.DTOs.DisplayDTOs;
using Application.DTOs.InsertDTOs;
using Application.DTOs.UpdateDTOs;
using Application.Interfaces.ApplicationServices;
using Domain.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NuGet.Protocol.Core.Types;
using System.Transactions;

namespace ITI.FinalProject.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RepresentativeController : ControllerBase
    {
        private readonly IGenericService<Representative, RepresentativeDisplayDTO, RepresentativeInsertDTO, ReoresentativeUpdateDTO, string> service;

        public RepresentativeController(IGenericService<Representative,RepresentativeDisplayDTO,RepresentativeInsertDTO,ReoresentativeUpdateDTO,string> service)
        {
            this.service = service;
        }

        [HttpPost("AddRepresentative")]
        public async Task<ActionResult> AddRepresentative([FromBody] RepresentativeInsertDTO RepresentativeInsertDTO)
        {
            using (var transaction = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
            {

                var result= await service.InsertObject(RepresentativeInsertDTO);
                //Transaction inside true condition or before it.
                transaction.Complete();
                if (result == true) return NoContent();
                else return BadRequest();
                
                       
            }

        }
        
        [HttpGet]
        public async Task<ActionResult> GetAllRepresentative()
        {
            var Representatives=await service.GetAllObjects();
            return Ok(Representatives);
        }
        [HttpGet("id")]
        public async Task<ActionResult<Representative>> GetRepresentativeById(string id)
        {
            var representative = await service.GetObject(r => r.userId == id);

            if (representative == null)
            {
                return NotFound();
            }

            return Ok(representative);
        }

    }
}
