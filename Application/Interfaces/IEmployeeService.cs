using Application.DTOs.DisplayDTOs;
using Application.DTOs.InsertDTOs;
using Application.DTOs.UpdateDTOs;
using Application.Interfaces.ApplicationServices;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interfaces
{
    public interface IEmployeeService
    {
        //Task<IEnumerable<EmployeeReadDto>> Getall();
        //Task<EmployeeReadDto> GetByid(string id);
        //Task<List<ValidationResult>?> AddUserAndEmployee(EmployeeAddDto employee);
        //Task Update(string id, EmployeeupdateDto employee);
        //Task Delete(string id);
        //Task Savechanges();
        // IQueryable<EmployeeReadDto> GetEmployeesPaginated();
        //Task AssignOrderToSales(int salesId, int orderId);
        public  Task<List<ValidationResult>?> AddUserAndEmployee(EmployeeAddDto employee);
        public Task<bool> DeleteObject(string id);
        public Task<IEnumerable<EmployeeReadDto>> GetAllObjects();
        public  Task<EmployeeReadDto> GetByid(string id);
        public  Task<bool> Update(string id, EmployeeupdateDto employeeDto);
    }
}
