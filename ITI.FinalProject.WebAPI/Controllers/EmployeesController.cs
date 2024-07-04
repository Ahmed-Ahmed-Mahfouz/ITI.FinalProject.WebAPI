using Application.DTOs.DisplayDTOs;
using Application.DTOs.InsertDTOs;
using Application.DTOs.UpdateDTOs;
using Application.Interfaces;
using Application.Interfaces.ApplicationServices;
using Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System.Transactions;

namespace ITI.FinalProject.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "Admin")]
    public class EmployeesController : ControllerBase
    {
        private readonly IPaginationService<Employee, EmployeeReadDto, EmployeeAddDto, EmployeeupdateDto, string> employeeService;
        public EmployeesController(IPaginationService<Employee, EmployeeReadDto, EmployeeAddDto, EmployeeupdateDto, string> employeeService)
        {
            this.employeeService = employeeService;
        }
        //GET
        [SwaggerOperation(
        Summary = "This Endpoint returns a list of Employees",
        Description = ""
        )]
        [SwaggerResponse(200, "Returns a list of employees", Type = typeof(IEnumerable<EmployeeReadDto>))]
        [SwaggerResponse(404, "There weren't any employees in the database", Type = typeof(void))]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<EmployeeReadDto>>> GetAll()
        {
            var employees = await employeeService.GetAllObjects();

            if (employees == null || !employees.Any())
            {
                return NotFound();
            }

            return Ok(employees);
        }

        [SwaggerOperation(
        Summary = "This Endpoint returns the specified employee",
        Description = ""
        )]
        [SwaggerResponse(200, "Returns the specified employee", Type = typeof(EmployeeReadDto))]
        [SwaggerResponse(404, "The id that was given doesn't exist in the db", Type = typeof(void))]
        [HttpGet]
        [Route("{id}")]
        public async Task<ActionResult<EmployeeReadDto>> GetById(string id)
        {
            EmployeeReadDto? employeeReadDto = await employeeService.GetObject(e => e.userId == id);
            if (employeeReadDto == null)
            {
                return NotFound();
            }
            return Ok(employeeReadDto);
        }
        //POST
        [SwaggerOperation(
        Summary = "This Endpoint inserts an employee element in the db",
        Description = ""
        )]
        [SwaggerResponse(204, "Confirms that the employee was inserted successfully", Type = typeof(void))]
        [SwaggerResponse(400, "Something went wrong, please try again later", Type = typeof(void))]
        [HttpPost]
        public async Task<ActionResult> Add([FromBody] EmployeeAddDto employeeAddDto)
        {
            using (var transaction = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
            {

                var result = await employeeService.InsertObject(employeeAddDto);
                transaction.Complete();

                if (result.Succeeded)
                {
                    return NoContent();
                }
                return Accepted(result.Message);
            }

        }
        //DELETE
        [SwaggerOperation(
        Summary = "This Endpoint deletes the specified employee from the db",
        Description = ""
        )]
        [SwaggerResponse(204, "Confirms that the employee was deleted successfully", Type = typeof(void))]
        [SwaggerResponse(400, "Something went wrong, please try again later", Type = typeof(void))]
        [HttpDelete]
        [Route("{id}")]
        public async Task<ActionResult> Delete(string id)
        {
            try
            {
                await employeeService.DeleteObject(id);
                return Ok("Deleted successfully");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [SwaggerOperation(
        Summary = "This Endpoint updates the specified employee",
        Description = ""
        )]
        [SwaggerResponse(204, "Confirms that the employee was updated successfully", Type = typeof(void))]
        [SwaggerResponse(400, "Something went wrong, please try again later", Type = typeof(void))]
        [HttpPut]
        [Route("{id}")]
        public async Task<IActionResult> Update(string id, [FromBody] EmployeeupdateDto employeeupdateDto)
        {
            if (id != employeeupdateDto.Id)
            {
                return BadRequest("Id doesn't match the id in the object");
            }

            var employee = await employeeService.GetObjectWithoutTracking(e => e.userId == id);

            if (employee == null)
            {
                return NotFound("Representative doesn't exist in the db");
            }

            var result = await employeeService.UpdateObject(employeeupdateDto);

            if (result.Succeeded)
            {
                return NoContent();
            }

            return Accepted(result.Message);
        }
    }
}
