using Application.DTOs.DisplayDTOs;
using Application.DTOs.InsertDTOs;
using Application.DTOs.UpdateDTOs;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services
{
    public interface IEmployeeService
    {
        Task<IEnumerable<EmployeeReadDto>> Getall();
        Task<EmployeeReadDto> GetByid(int id);
        Task<List<ValidationResult>?> AddUserAndEmployee(EmployeeAddDto employee);
        Task Update(int id, EmployeeupdateDto employee);
        Task Delete(int id);
        Task Savechanges();
       // IQueryable<EmployeeReadDto> GetEmployeesPaginated();
        //Task AssignOrderToSales(int salesId, int orderId);
    }
}
