using Application.DTOs.DisplayDTOs;
using Application.DTOs.InsertDTOs;
using Application.DTOs.UpdateDTOs;
using Application.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace ITI.FinalProject.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeesController : ControllerBase
    {
        private readonly IEmployeeService employeeService;

        public EmployeesController(IEmployeeService employeeService)
        {
            this.employeeService = employeeService;
        }

        [SwaggerOperation(
        Summary = "This Endpoint returns a list of employees",
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
            EmployeeReadDto? employeeReadDto = await employeeService.GetByid(id);
            if (employeeReadDto == null)
            {
                return NotFound();
            }
            return Ok(employeeReadDto);
        }

        [SwaggerOperation(
        Summary = "This Endpoint inserts an employee element in the db",
        Description = ""
        )]
        [SwaggerResponse(204, "Confirms that the employee was inserted successfully", Type = typeof(void))]
        [SwaggerResponse(400, "Something went wrong, please try again later", Type = typeof(void))]
        [HttpPost]
        public async Task<ActionResult> Add(EmployeeAddDto employeeAddDto)
        {
            try
            {
                await employeeService.AddUserAndEmployee(employeeAddDto);
                return NoContent();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

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
        public async Task<ActionResult> Update(string id, EmployeeupdateDto employeeupdateDto)
        {
            try
            {
                await employeeService.Update(id, employeeupdateDto);
                return Ok("Employee update is successful");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
