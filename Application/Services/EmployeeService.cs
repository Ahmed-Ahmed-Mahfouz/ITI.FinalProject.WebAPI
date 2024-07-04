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
using Application.DTOs;


namespace Domain.Services
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
                PhoneNumber = userDto.PhoneNo,
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

        public Task<PaginationDTO<EmployeeReadDto>> GetPaginatedOrders(int pageNumber, int pageSize, Expression<Func<Employee, bool>> filter)
        {
            throw new NotImplementedException();
        }

    }
}


