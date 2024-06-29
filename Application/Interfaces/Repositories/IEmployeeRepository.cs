using Application.DTOs.DisplayDTOs;
using Application.DTOs.InsertDTOs;
using Application.DTOs.UpdateDTOs;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interfaces.Repositories
{
    public interface IEmployeeRepository
    {
        Task<IEnumerable<Employee>> Getall();
        Task<Employee?> GetByid(int id);
        Task Add(Employee employee);
        Task Update(int id, Employee? employee);
        Task Delete(int id);
        Task Savechanges();
        IQueryable<Employee> GetEmployeePaginated();
        Task AssignOrderToSales(int salesId, int orderId);
       
    }
}
