﻿using Application.DTOs.DisplayDTOs;
using Application.DTOs.InsertDTOs;
using Application.DTOs.UpdateDTOs;
using Application.Interfaces.ApplicationServices;
using Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ITI.FinalProject.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "Admin")]
    public class BranchesController : ControllerBase
    {
        IPaginationService<Branch, BranchDisplayDTO, BranchInsertDTO, BranchUpdateDTO,int> branchServ;
        public BranchesController(IPaginationService<Branch, BranchDisplayDTO, BranchInsertDTO, BranchUpdateDTO,int> _branchServ)
        {
            branchServ = _branchServ;
        }

        [HttpGet]
        public async Task<ActionResult> GetAllBranches()
        {
            List<BranchDisplayDTO> branches = await branchServ.GetAllObjects();
            return Ok(branches);

        }

        [HttpGet("{name:alpha}")]
        public async Task<ActionResult<PaginationDTO<BranchDisplayDTO>>> GetBranches([FromQuery] string name = "", [FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10)
        {

            var paginationDTO = await branchServ.GetPaginatedOrders(pageNumber, pageSize, b => b.name.Trim().ToLower().Contains(name.Trim().ToLower()));

            return Ok(paginationDTO);
        }

        //[HttpGet("{name:alpha}")]
        //public async Task<ActionResult> GetBranchesByName([FromQuery] string name)
        //{
        //    var (branches, totalCount) = await branchServ.GetPaginatedOrders(pageNumber, pageSize);
        //    return Ok(branches);
        //}

        [HttpGet("id")]
        public async Task<ActionResult> GetById(int id)
        {
            BranchDisplayDTO? branch = await branchServ.GetObject(p=>p.id==id);
            if (branch == null)
                return NotFound();
            return Ok(branch);

        }

        [HttpPost]
        public async Task<ActionResult> AddBranch(BranchInsertDTO branch)
        {
            if (branch == null)
                return BadRequest();
            var result =await branchServ.InsertObject(branch);
            if (result.Succeeded)
            {
                return Ok(result);
            }
            return BadRequest(result.Message);


        }

        [HttpDelete]
        public async Task<ActionResult> DeleteBranch(int id)
        {

            var result =await branchServ.DeleteObject(id);
            if (result.Succeeded)
            {
                return Ok(result);
            }
            return BadRequest(result.Message);


        }

        [HttpPut("id")]
        public async Task<ActionResult> UpdateBranch(int id,BranchUpdateDTO branch)
        {
            if(branch == null || id != branch.id)
            {
                return BadRequest();
            }

            var result=await branchServ.UpdateObject(branch);
            if(result.Succeeded)
            {
                return Ok(branch);
            }
            return BadRequest(result.Message);
        }

    }
}
