using Domain.Entities;
using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using Application.DTOs.DisplayDTOs;
using Application.DTOs.InsertDTOs;
using Application.DTOs.UpdateDTOs;
using Application.Interfaces.Repositories;
using AutoMapper.QueryableExtensions;
using AutoMapper;
using Application.Interfaces;
using Application.Interfaces.ApplicationServices;
using System.Linq.Expressions;


namespace Domain.Services
{
    public class EmployeeService :IEmployeeService
    {
        private readonly IGenericRepository<Employee> employeeRepository;
        private readonly IMapper mapper;
        IUnitOfWork unit;
        private readonly UserManager<ApplicationUser> _userManager;
        

        public EmployeeService(IUnitOfWork employeeRepository, IMapper mapper, UserManager<ApplicationUser> userManager)
        {
            this.employeeRepository = employeeRepository.GetGenericRepository<Employee>();
            this.mapper = mapper;
            _userManager = userManager;
            unit = employeeRepository;
           
        }
        public async Task<List<ValidationResult>?> AddUserAndEmployee(EmployeeAddDto employee)
        {
            var validationResults = new List<ValidationResult>();
            var context = new ValidationContext(employee, null, null);

            // Validate the employee object using DataAnnotations
            Validator.TryValidateObject(employee, context, validationResults, true);

            if (validationResults.Count == 0)
            {
                ApplicationUser? checkUserEmail = await _userManager.FindByEmailAsync(employee.Email);
                if (checkUserEmail is null)
                {
                    ApplicationUser? checkUserName = await _userManager.FindByNameAsync(employee.UserName);
                    if (checkUserName is null)
                    {
                        ApplicationUser user = mapper.Map<ApplicationUser>(employee);
                        IdentityResult result = await _userManager.CreateAsync(user, employee.PasswordHash);
                        if (!result.Succeeded)
                        {
                            foreach (var error in result.Errors)
                            {
                                ValidationResult err = new ValidationResult(error.Description);
                                validationResults.Add(err);
                            }
                            return validationResults;
                        }
                        await _userManager.AddToRoleAsync(user, employee.role);
                        await _userManager.UpdateAsync(user);
                        ApplicationUser? addedUser = await _userManager.FindByEmailAsync(employee.Email);
                        employee.User = addedUser;
                    }
                    else
                    {
                        validationResults.Add(new ValidationResult("User name is already exist"));
                        return validationResults;
                    }
                }
                else
                {
                    validationResults.Add(new ValidationResult("Email is already exist"));
                    return validationResults;
                }

                 employeeRepository.Add(mapper.Map<EmployeeAddDto, Employee>(employee));
                return validationResults;
            }
            else
            {
                return validationResults;
            }
        }


        public async Task<bool> DeleteObject(string id)
        {
            Employee? employeeFromDb = await employeeRepository.GetElement(e=>e.userId == id);
            if (employeeFromDb != null)
            {
                //employeeFromDb.IsActive = false;

                await unit.SaveChanges();
                return true;
            }
            else
            {
                return false;
                //throw new Exception("this employee not found");
            }
        }

        public async Task<IEnumerable<EmployeeReadDto>> GetAllObjects()
        {
            var employeesfromDb = await employeeRepository.GetAllElements();
            return mapper.Map<IEnumerable<EmployeeReadDto>>(employeesfromDb);
        }

        public async Task<EmployeeReadDto> GetByid(string id)
        {
            var employeefromDb = await employeeRepository.GetElement(m=>m.userId == id);

            if (employeefromDb == null)
            {
                return null;
            }
            return mapper.Map<EmployeeReadDto>(employeefromDb);
        }

        
        public async Task<bool> Update(string id, EmployeeupdateDto employeeDto)
        {
            Employee? empFromDb = await employeeRepository.GetElement(e=>e.userId==id);
            
            if (empFromDb != null)
            {
                ApplicationUser? applicationUser = await _userManager.FindByIdAsync(id);
                var roles = await _userManager.GetRolesAsync(applicationUser);
                mapper.Map(employeeDto, empFromDb);
                await _userManager.RemoveFromRolesAsync(applicationUser, roles);
                await _userManager.AddToRoleAsync(applicationUser, employeeDto.role);
                //await _userManager.UpdateAsync(user);

                await unit.SaveChanges();
                return true;

            }
            else
            {
                return false;
            }
        }

      
        

       

        //public IQueryable<EmployeeReadDto> GetEmployeesPaginated()
        //{
        //    IQueryable employees = employeeRepository.GetEmployeePaginated();
        //    return employees.ProjectTo<EmployeeReadDto>(mapper.ConfigurationProvider);
        //}



        //public async Task AssignOrderToSales(string salesId, int orderId)
        //{
        //    await employeeRepository.AssignOrderToSales(salesId, orderId);
        //    await employeeRepository.SaveChangesForObject();
        //}
    }
}


