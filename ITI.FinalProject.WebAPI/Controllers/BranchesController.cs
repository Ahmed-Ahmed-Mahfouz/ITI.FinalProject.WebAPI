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
    public class BranchesController : ControllerBase
    {
        IGenericService<Branch, BranchDisplayDTO, BranchInsertDTO, BranchUpdateDTO,int> branchServ;
        public BranchesController(IGenericService<Branch, BranchDisplayDTO, BranchInsertDTO, BranchUpdateDTO,int> _branchServ)
        {
            branchServ = _branchServ;
        }

        [SwaggerOperation(
        Summary = "This Endpoint returns a list of branches",
            Description = ""
        )]
        [SwaggerResponse(404, "There weren't any branches in the database", Type = typeof(void))]
        [SwaggerResponse(200, "Returns A list of branches", Type = typeof(List<BranchDisplayDTO>))]

        [HttpGet]
        public async Task<ActionResult> getAllBranches()
        {
            List<BranchDisplayDTO> branches = await branchServ.GetAllObjects();
            if(branches.Count == 0)
            {
                return NotFound();
            }
            return Ok(branches);

        }
         [SwaggerOperation(
       Summary = "This Endpoint returns the specified branch",
           Description = ""
       )]
        [SwaggerResponse(404, "The id that was given doesn't exist in the db", Type = typeof(void))]
        [SwaggerResponse(200, "Returns the specified branch", Type = typeof(BranchDisplayDTO))]

        [HttpGet("id")]
        public async Task<ActionResult> getById(int id)
        {
            BranchDisplayDTO? branch = await branchServ.GetObject(p=>p.id==id);
            if (branch == null)
                return NotFound();
            return Ok(branch);

        }
        [SwaggerOperation(
      Summary = "This Endpoint inserts a city element in the db",
          Description = ""
      )]
        [SwaggerResponse(400, "Something went wrong, please check your request", Type = typeof(void))]
        [SwaggerResponse(201, "Confirms that the city was inserted successfully", Type = typeof(void))]
        [SwaggerResponse(500, "Something went wrong, please try again later", Type = typeof(void))]

        [HttpPost]
        public async Task<ActionResult> addBranch(BranchInsertDTO branch)
        {
            if (branch == null)
                return BadRequest();
            var result =await branchServ.InsertObject(branch);
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
        public async Task<ActionResult> deleteBranch(int id)
        {
            BranchDisplayDTO? branch = await branchServ.GetObjectWithoutTracking(c => c.id == id);
            if (branch == null)
            {
                return NotFound();
            }
            var result =await branchServ.DeleteObject(id);
            if (result.Succeeded)
            {
                return Ok(result);
            }
            return StatusCode(StatusCodes.Status500InternalServerError,result.Message);


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
        public async Task<ActionResult> updateBranch(int id,BranchUpdateDTO branch)
        {
            if(branch == null || id != branch.id)
            {
                return BadRequest();
            }

            BranchDisplayDTO? branchDisplay = await branchServ.GetObjectWithoutTracking(c => c.id == id);
            if (branch == null)
            {
                return NotFound();
            }

            var result=await branchServ.UpdateObject(branch);
            if(result.Succeeded)
            {
                return Ok(branch);
            }
            return StatusCode(StatusCodes.Status500InternalServerError, result.Message);
        }

    }
}
