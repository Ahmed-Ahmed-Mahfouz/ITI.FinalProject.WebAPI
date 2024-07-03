//using Domain.Entities;
//using Microsoft.AspNetCore.Identity;
//using System.ComponentModel.DataAnnotations;
//using Application.DTOs.DisplayDTOs;
//using Application.DTOs.InsertDTOs;
//using Application.DTOs.UpdateDTOs;
//using Application.Interfaces.Repositories;
//using AutoMapper.QueryableExtensions;
//using AutoMapper;
//using Application.Interfaces;
//using Application.Interfaces.ApplicationServices;
//using System.Linq.Expressions;


//namespace Domain.Services
//{
//    public class EmployeeService :IEmployeeService
//    {
//        private readonly IGenericRepository<Employee> employeeRepository;
//        private readonly IMapper mapper;
//        IUnitOfWork unit;
//        private readonly UserManager<ApplicationUser> _userManager;


//        public EmployeeService(IUnitOfWork employeeRepository, IMapper mapper, UserManager<ApplicationUser> userManager)
//        {
//            this.employeeRepository = employeeRepository.GetGenericRepository<Employee>();
//            this.mapper = mapper;
//            _userManager = userManager;
//            unit = employeeRepository;

//        }
//        public async Task<List<ValidationResult>?> AddUserAndEmployee(EmployeeAddDto employee)
//        {
//            var validationResults = new List<ValidationResult>();
//            var context = new ValidationContext(employee, null, null);

//            // Validate the employee object using DataAnnotations
//            Validator.TryValidateObject(employee, context, validationResults, true);

//            if (validationResults.Count == 0)
//            {
//                ApplicationUser? checkUserEmail = await _userManager.FindByEmailAsync(employee.Email);
//                if (checkUserEmail is null)
//                {
//                    ApplicationUser? checkUserName = await _userManager.FindByNameAsync(employee.UserName);
//                    if (checkUserName is null)
//                    {
//                        ApplicationUser user = mapper.Map<ApplicationUser>(employee);
//                        IdentityResult result = await _userManager.CreateAsync(user, employee.PasswordHash);
//                        if (!result.Succeeded)
//                        {
//                            foreach (var error in result.Errors)
//                            {
//                                ValidationResult err = new ValidationResult(error.Description);
//                                validationResults.Add(err);
//                            }
//                            return validationResults;
//                        }
//                        await _userManager.AddToRoleAsync(user, employee.role);
//                        await _userManager.UpdateAsync(user);
//                        ApplicationUser? addedUser = await _userManager.FindByEmailAsync(employee.Email);
//                        employee.User = addedUser;
//                    }
//                    else
//                    {
//                        validationResults.Add(new ValidationResult("User name is already exist"));
//                        return validationResults;
//                    }
//                }
//                else
//                {
//                    validationResults.Add(new ValidationResult("Email is already exist"));
//                    return validationResults;
//                }

//                 employeeRepository.Add(mapper.Map<EmployeeAddDto, Employee>(employee));
//                return validationResults;
//            }
//            else
//            {
//                return validationResults;
//            }
//        }


//        public async Task<bool> DeleteObject(string id)
//        {
//            Employee? employeeFromDb = await employeeRepository.GetElement(e=>e.userId == id);
//            if (employeeFromDb != null)
//            {
//                //employeeFromDb.IsActive = false;

//                await unit.SaveChanges();
//                return true;
//            }
//            else
//            {
//                return false;
//                //throw new Exception("this employee not found");
//            }
//        }

//        public async Task<IEnumerable<EmployeeReadDto>> GetAllObjects()
//        {
//            var employeesfromDb = await employeeRepository.GetAllElements();
//            return mapper.Map<IEnumerable<EmployeeReadDto>>(employeesfromDb);
//        }

//        public async Task<EmployeeReadDto> GetByid(string id)
//        {
//            var employeefromDb = await employeeRepository.GetElement(m=>m.userId == id);

//            if (employeefromDb == null)
//            {
//                return null;
//            }
//            return mapper.Map<EmployeeReadDto>(employeefromDb);
//        }


//        public async Task<bool> Update(string id, EmployeeupdateDto employeeDto)
//        {
//            Employee? empFromDb = await employeeRepository.GetElement(e=>e.userId==id);

//            if (empFromDb != null)
//            {
//                ApplicationUser? applicationUser = await _userManager.FindByIdAsync(id);
//                var roles = await _userManager.GetRolesAsync(applicationUser);
//                mapper.Map(employeeDto, empFromDb);
//                await _userManager.RemoveFromRolesAsync(applicationUser, roles);
//                await _userManager.AddToRoleAsync(applicationUser, employeeDto.role);
//                //await _userManager.UpdateAsync(user);

//                await unit.SaveChanges();
//                return true;

//            }
//            else
//            {
//                return false;
//            }
//        }






//        //public IQueryable<EmployeeReadDto> GetEmployeesPaginated()
//        //{
//        //    IQueryable employees = employeeRepository.GetEmployeePaginated();
//        //    return employees.ProjectTo<EmployeeReadDto>(mapper.ConfigurationProvider);
//        //}



//        //public async Task AssignOrderToSales(string salesId, int orderId)
//        //{
//        //    await employeeRepository.AssignOrderToSales(salesId, orderId);
//        //    await employeeRepository.SaveChangesForObject();
//        //}
//    }
//}

using Application.DTOs;
using Application.DTOs.DisplayDTOs;
using Application.DTOs.InsertDTOs;
using Application.DTOs.UpdateDTOs;
using Application.Interfaces;
using Application.Interfaces.ApplicationServices;
using Application.Interfaces.Repositories;
using AutoMapper;
using Domain.Entities;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Application.Services
{
    public class EmployeeService : IPaginationService<Employee, EmployeeReadDto, EmployeeAddDto, EmployeeupdateDto, string>
    {
        private readonly IUnitOfWork unit;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IPaginationRepository<Employee> repository;
        private readonly IMapper mapper;

        public EmployeeService(IUnitOfWork unit, UserManager<ApplicationUser> userManager, IMapper mapper)
        {
            this.unit = unit;
            this._userManager = userManager;
            this.repository = unit.GetPaginationRepository<Employee>();
            this.mapper = mapper;
        }

        public async Task<List<EmployeeReadDto>> GetAllObjects()
        {
            var employees = await repository.GetAllElements();
            return MapEmployees(employees);
        }

        public async Task<List<EmployeeReadDto>> GetAllObjects(params Expression<Func<Employee, object>>[] includes)
        {
            var employees = await repository.GetAllElements(includes);
            return MapEmployees(employees);
        }

        public async Task<EmployeeReadDto?> GetObject(Expression<Func<Employee, bool>> filter)
        {
            var employee = await repository.GetElement(filter);
            if (employee == null)
            {
                return null;
            }
            return MapEmployee(employee);
        }

        public async Task<EmployeeReadDto?> GetObject(Expression<Func<Employee, bool>> filter, params Expression<Func<Employee, object>>[] includes)
        {
            var employee = await repository.GetElement(filter, includes);
            if (employee == null)
            {
                return null;
            }
            return MapEmployee(employee);
        }

        public async Task<EmployeeReadDto?> GetObjectWithoutTracking(Expression<Func<Employee, bool>> filter)
        {
            var employee = await repository.GetElementWithoutTracking(filter);
            if (employee == null)
            {
                return null;
            }
            return MapEmployee(employee);
        }

        public async Task<EmployeeReadDto?> GetObjectWithoutTracking(Expression<Func<Employee, bool>> filter, params Expression<Func<Employee, object>>[] includes)
        {
            var employee = await repository.GetElementWithoutTracking(filter, includes);
            if (employee == null)
            {
                return null;
            }
            return MapEmployee(employee);
        }

        public async Task<ModificationResultDTO> InsertObject(EmployeeAddDto ObjectDTO)
        {
            var userAdded = new UserDto()
            {
                Email = ObjectDTO.Email,
                Password = ObjectDTO.PasswordHash,
                Address = ObjectDTO.Address,
                FullName = ObjectDTO.FullName,
                PhoneNo = ObjectDTO.PhoneNumber,
                Status = ObjectDTO.Status,
                UserType = Domain.Enums.UserType.Employee,
            };

            var resultUser = await AddUser(userAdded);

            if (resultUser.Message != null)
            {
                return new ModificationResultDTO()
                {
                    Succeeded = false,
                    Message = resultUser.Message
                };
            }

            var employee = mapper.Map<Employee>(ObjectDTO);
            employee.userId = resultUser.UserId;

            var employeeResult = repository.Add(employee);

            if (employeeResult == false)
            {
                return new ModificationResultDTO()
                {
                    Succeeded = false,
                    Message = "Error inserting the employee"
                };
            }

            var saveResult = await unit.SaveChanges();

            if (saveResult == false)
            {
                return new ModificationResultDTO()
                {
                    Succeeded = false,
                    Message = "Error saving the changes"
                };
            }

            return new ModificationResultDTO()
            {
                Succeeded = true
            };
        }

        public async Task<ModificationResultDTO> UpdateObject(EmployeeupdateDto ObjectDTO)
        {
            var user = await _userManager.FindByIdAsync(ObjectDTO.UserName);

            if (user == null)
            {
                return new ModificationResultDTO()
                {
                    Succeeded = false,
                    Message = "User doesn't exist in the db"
                };
            }

            user.FullName = ObjectDTO.FullName;
            var identityResult = await _userManager.ChangePasswordAsync(user, user.PasswordHash, ObjectDTO.PasswordHash);

            if (!identityResult.Succeeded)
            {
                return new ModificationResultDTO()
                {
                    Succeeded = false,
                    Message = "Error changing user password"
                };
            }

            var token = await _userManager.GenerateChangeEmailTokenAsync(user, ObjectDTO.Email);

            identityResult = await _userManager.ChangeEmailAsync(user, ObjectDTO.Email, token);

            if (!identityResult.Succeeded)
            {
                return new ModificationResultDTO()
                {
                    Succeeded = false,
                    Message = "Error changing user email"
                };
            }

            token = await _userManager.GenerateChangePhoneNumberTokenAsync(user, ObjectDTO.PhoneNumber);

            identityResult = await _userManager.ChangePhoneNumberAsync(user, ObjectDTO.PhoneNumber, token);

            if (!identityResult.Succeeded)
            {
                return new ModificationResultDTO()
                {
                    Succeeded = false,
                    Message = "Error changing user phone number"
                };
            }

            user.Status = ObjectDTO.Status;
            identityResult = await _userManager.UpdateAsync(user);

            if (!identityResult.Succeeded)
            {
                return new ModificationResultDTO()
                {
                    Succeeded = false,
                    Message = "Error updating the user"
                };
            }

            var employee = await repository.GetElement(e => e.userId == user.Id);

            if (employee == null)
            {
                return new ModificationResultDTO()
                {
                    Succeeded = false,
                    Message = "Employee doesn't exist in the db"
                };
            }

            mapper.Map(ObjectDTO, employee);
            var result = repository.Edit(employee);

            if (result == false)
            {
                return new ModificationResultDTO()
                {
                    Succeeded = false,
                    Message = "Error updating the employee"
                };
            }

            result = await SaveChangesForObject();

            if (result == false)
            {
                return new ModificationResultDTO()
                {
                    Succeeded = false,
                    Message = "Error saving changes"
                };
            }

            return new ModificationResultDTO()
            {
                Succeeded = true
            };
        }

        public async Task<ModificationResultDTO> DeleteObject(string ObjectId)
        {
            var employee = await repository.GetElement(e => e.userId == ObjectId);

            if (employee == null)
            {
                return new ModificationResultDTO()
                {
                    Succeeded = false,
                    Message = "Employee doesn't exist in the db"
                };
            }

            var result = repository.Delete(employee);

            if (result == false)
            {
                return new ModificationResultDTO()
                {
                    Succeeded = false,
                    Message = "Error deleting the employee"
                };
            }

            result = await SaveChangesForObject();

            if (result == false)
            {
                return new ModificationResultDTO()
                {
                    Succeeded = false,
                    Message = "Error saving changes"
                };
            }

            var user = await _userManager.FindByIdAsync(ObjectId);

            if (user == null)
            {
                return new ModificationResultDTO()
                {
                    Succeeded = false,
                    Message = "User doesn't exist in the db"
                };
            }

            var identityResult = await _userManager.DeleteAsync(user);

            if (identityResult.Succeeded)
            {
                return new ModificationResultDTO()
                {
                    Succeeded = true
                };
            }

            return new ModificationResultDTO()
            {
                Succeeded = false,
                Message = "Error deleting user"
            };
        }

        public async Task<bool> SaveChangesForObject()
        {
            var result = await unit.SaveChanges();
            return result;
        }

        private EmployeeReadDto MapEmployee(Employee employee)
        {
            var employeeDTO = new EmployeeReadDto()
            {
                FullName = employee.user.FullName,
                Address = employee.user.Address,
                PhoneNumber = employee.user.PhoneNumber,
                UserName = employee.user.UserName,
                Email = employee.user.Email,
                Status = employee.user.Status,
                role = employee.user.UserType.ToString() 
            };

            return employeeDTO;
        }

        private List<EmployeeReadDto> MapEmployees(List<Employee> employees)
        {
            var employeeDTOs = employees.Select(e => MapEmployee(e)).ToList();
            return employeeDTOs;
        }

        public async Task<ResultUser> AddUser(UserDto userDto)
        {
            if (await _userManager.FindByEmailAsync(userDto.Email) != null)
                return new ResultUser { Message = "Email is Already registered!" };

            if (await _userManager.FindByNameAsync(userDto.FullName.Trim().Replace(' ', '_')) != null)
                return new ResultUser { Message = "UserName is Already registered!" };

            var user = new ApplicationUser
            {
                UserName = userDto.FullName.Trim().Replace(' ', '_'),
                Email = userDto.Email,
                FullName = userDto.FullName,
                UserType = userDto.UserType,
                Status = userDto.Status,
                PhoneNo = userDto.PhoneNo,
                Address = userDto.Address
            };

            var result = await _userManager.CreateAsync(user, userDto.Password);
            if (!result.Succeeded)
            {
                string errors = string.Empty;

                foreach (var error in result.Errors)
                {
                    errors += $"{error.Description}{Environment.NewLine}";
                }

                return new ResultUser { Message = errors };
            }

            return new ResultUser { Message = null, UserId = user.Id };
        }

        public Task<(List<EmployeeReadDto>, int)> GetPaginatedOrders(int pageNumber, int pageSize)
        {
            throw new NotImplementedException();
        }

        //Task<bool> IGenericService<Employee, EmployeeReadDto, EmployeeAddDto, EmployeeupdateDto, string>.InsertObject(EmployeeAddDto ObjectDTO)
        //{
        //    throw new NotImplementedException();
        //}

        //Task<bool> IGenericService<Employee, EmployeeReadDto, EmployeeAddDto, EmployeeupdateDto, string>.UpdateObject(EmployeeupdateDto ObjectDTO)
        //{
        //    throw new NotImplementedException();
        //}

        //Task<bool> IGenericService<Employee, EmployeeReadDto, EmployeeAddDto, EmployeeupdateDto, string>.DeleteObject(string ObjectId)
        //{
        //    throw new NotImplementedException();
        //}
    }
}

