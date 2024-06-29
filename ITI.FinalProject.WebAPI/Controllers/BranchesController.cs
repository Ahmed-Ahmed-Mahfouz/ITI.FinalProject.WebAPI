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
    public class BranchesController : ControllerBase
    {
        IGenericService<Branch, BranchDisplayDTO, BranchInsertDTO, BranchUpdateDTO,int> branchServ;
        public BranchesController(IGenericService<Branch, BranchDisplayDTO, BranchInsertDTO, BranchUpdateDTO,int> _branchServ)
        {
            branchServ = _branchServ;
        }
        [HttpGet]
        public async Task<ActionResult> getAllBranches()
        {
            List<BranchDisplayDTO> branches = await branchServ.GetAllObjects();
            return Ok(branches);

        }
        [HttpGet("id")]
        public async Task<ActionResult> getById(int id)
        {
            BranchDisplayDTO? branch = await branchServ.GetObject(p=>p.id==id);
            if (branch == null)
                return NotFound();
            return Ok(branch);

        }
        [HttpPost]
        public async Task<ActionResult> addBranch(BranchInsertDTO branch)
        {
            if (branch == null)
                return BadRequest();
            var result =await branchServ.InsertObject(branch);
            if (result == true)
            {
                return Ok(result);
            }
            return BadRequest();


        }
        [HttpDelete]
        public async Task<ActionResult> deleteBranch(int id)
        {

            bool result =await branchServ.DeleteObject(id);
            if (result)
            {
                return Ok(result);
            }
            return BadRequest();


        }
        [HttpPut("id")]
        public async Task<ActionResult> updateBranch(int id,BranchUpdateDTO branch)
        {
            if(branch == null || id != branch.id)
            {
                return BadRequest();
            }

            var result=await branchServ.UpdateObject(branch);
            if(result)
            {
                return Ok(branch);
            }
            return BadRequest();
        }

    }
}
