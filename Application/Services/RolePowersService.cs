using Application.DTOs.DisplayDTOs;
using Application.DTOs.InsertDTOs;
using Application.DTOs.UpdateDTOs;
using Application.Interfaces;
using Application.Interfaces.ApplicationServices;
using Application.Interfaces.Repositories;
using Domain.Entities;
using Domain.Enums;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services
{
    public class RolePowersService : IPaginationService<RolePowers, RolePowersDTO, RolePowersInsertDTO, RolePowersUpdateDTO, string>
    {
        private readonly IUnitOfWork unit;
        private readonly RoleManager<ApplicationRoles> roleManager;
        private readonly IPaginationRepository<RolePowers> repository;

        public RolePowersService(IUnitOfWork unit, RoleManager<ApplicationRoles> roleManager)
        {
            this.unit = unit;
            this.roleManager = roleManager;
            repository = unit.GetPaginationRepository<RolePowers>();
        }

        public async Task<List<RolePowersDTO>> GetAllObjects()
        {
            //var rolePowers = await repository.GetAllElements();

            //return await MapRolePowers(rolePowers);

            var roles = await roleManager.Roles.ToListAsync();

            return MapRoles(roles);
        }

        public async Task<List<RolePowersDTO>> GetAllObjects(params Expression<Func<RolePowers, object>>[] includes)
        {
            //var rolePowers = await repository.GetAllElements(includes);

            //return await MapRolePowers(rolePowers);

            var roles = await roleManager.Roles.ToListAsync();

            return MapRoles(roles);
        }

        public async Task<RolePowersDTO?> GetObject(Expression<Func<RolePowers, bool>> filter)
        {
            var rolePowers = await repository.GetAllElements(filter);

            if (rolePowers == null)
            {
                return null;
            }

            return await MapRolePowers(rolePowers);
        }

        public async Task<RolePowersDTO?> GetObject(Expression<Func<RolePowers, bool>> filter, params Expression<Func<RolePowers, object>>[] includes)
        {
            var rolePowers = await repository.GetAllElements(filter, includes);

            if (rolePowers == null)
            {
                return null;
            }

            return await MapRolePowers(rolePowers);
        }

        public async Task<RolePowersDTO?> GetObjectWithoutTracking(Expression<Func<RolePowers, bool>> filter)
        {
            var rolePower = await repository.GetElementsWithoutTracking(filter);

            if (rolePower == null)
            {
                return null;
            }

            return await MapRolePowers(rolePower);
        }

        public async Task<RolePowersDTO?> GetObjectWithoutTracking(Expression<Func<RolePowers, bool>> filter, params Expression<Func<RolePowers, object>>[] includes)
        {
            var rolePower = await repository.GetElementsWithoutTracking(filter, includes);

            if (rolePower == null)
            {
                return null;
            }

            return await MapRolePowers(rolePower);
        }

        public async Task<ModificationResultDTO> InsertObject(RolePowersInsertDTO rolePowersInsertDTO)
        {
            var role = new ApplicationRoles()
            {
                Id = Guid.NewGuid().ToString(),
                Name = rolePowersInsertDTO.RoleName,
                TimeOfAddition = DateTime.Now
            };

            var identityResult = await roleManager.CreateAsync(role);

            if (identityResult.Succeeded)
            {
                foreach (Tables power in Enum.GetValues(typeof(Tables)))
                {
                    var result = repository.Add(new RolePowers()
                    {
                        RoleId = role.Id,
                        TableName = power,
                        Create = false,
                        Delete = false,
                        Read = false,
                        Update = false
                    });

                    if (result == false)
                    {
                        return new ModificationResultDTO()
                        {
                            Succeeded = false,
                            Message = "Error inserting the rolepower"
                        };
                    }
                }

                return new ModificationResultDTO()
                {
                    Succeeded = true
                };
            }

            return new ModificationResultDTO()
            {
                Succeeded = false,
                Message = "Error inserting the role"
            };
        }

        public async Task<ModificationResultDTO> UpdateObject(RolePowersUpdateDTO rolePowersUpdateDTO)
        {

            var role = await roleManager.FindByIdAsync(rolePowersUpdateDTO.RoleId);

            if (role == null)
            {
                return new ModificationResultDTO()
                {
                    Succeeded = false,
                    Message = "Role doesn't exist in the db"
                };
            }

            role.Name = rolePowersUpdateDTO.RoleName;

            var identityResult = await roleManager.UpdateAsync(role);

            if (identityResult.Succeeded)
            {
                foreach (var power in rolePowersUpdateDTO.Powers)
                {
                    var rolePower = await repository.GetElement(rp => rp.RoleId == rolePowersUpdateDTO.RoleId && rp.TableName == power.TableName);

                    if (rolePower == null)
                    {
                        return new ModificationResultDTO()
                        {
                            Succeeded = false,
                            Message = "Role power doesn't exist in the db"
                        };
                    }

                    rolePower.Create = power.Create;
                    rolePower.Delete = power.Delete;
                    rolePower.Update = power.Update;
                    rolePower.Read = power.Read;
                    
                    var result = repository.Edit(rolePower);

                    if (result == false)
                    {
                        return new ModificationResultDTO()
                        {
                            Succeeded = false,
                            Message = "Error updating the role power"
                        };
                    }
                }

                return new ModificationResultDTO()
                {
                    Succeeded = true
                };
            }

            return new ModificationResultDTO()
            {
                Succeeded = false,
                Message = "Error updating the role"
            };
        }

        public async Task<ModificationResultDTO> DeleteObject(string rolePowersId)
        {
            var rolePowers = await repository.GetAllElements(rp => rp.RoleId == rolePowersId);

            foreach (var item in rolePowers)
            {
                var result = repository.Delete(item);

                if (result == false)
                {
                    return new ModificationResultDTO()
                    {
                        Succeeded = false,
                        Message = "Error deleting the role power"
                    };
                }
            }

            var role = await roleManager.FindByIdAsync(rolePowersId);

            if (role == null)
            {
                return new ModificationResultDTO()
                {
                    Succeeded = false,
                    Message = "Role doesn't exist in the db"
                };
            }

            var identityResult = await roleManager.DeleteAsync(role);

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
                Message = "Error deleting the role"
            };
        }

        public async Task<bool> SaveChangesForObject()
        {
            var result = await unit.SaveChanges();

            return result;
        }

        private async Task<RolePowersDTO> MapRolePowers(List<RolePowers> rolePowers)
        {

            //foreach (var rolePower in rolePowers)
            //{            

            //if (role == null || role.Name == "Admin" || role.Name == "Merchant" || role.Name == "Representative")
            //{
            //    re
            //}

            var role = await roleManager.FindByIdAsync(rolePowers[0].RoleId);
                
            if (role == null)
            {
                return new RolePowersDTO();
            }

            var powers = rolePowers.Select(rp => new PowersDTO() { TableName = rp.TableName, Create = rp.Create, Update = rp.Update, Delete = rp.Delete, Read = rp.Read}).ToList();

            return new RolePowersDTO()
            {
                RoleId = role.Id,
                RoleName = role.Name ?? "RoleName",
                TimeOfAddtion = role.TimeOfAddition,
                Powers = powers
            };
        }

        //private async Task<RolePowersDTO> MapRolePower(RolePowers rolePower)
        //{
        //    var role = await roleManager.FindByIdAsync(rolePower.RoleId);

        //    if (role == null || role.Name == "Admin" || role.Name == "Merchant" || role.Name == "Representative")
        //    {
        //        return new RolePowersDTO();
        //    }

        //    var powers = new List<PowersDTO>()
        //    {
        //        new PowersDTO()
        //        {
        //            TableName = rolePower.TableName,
        //            Create = rolePower.Create,
        //            Delete = rolePower.Delete,
        //            Read = rolePower.Read,
        //            Update = rolePower.Update
        //        }
        //    };

        //    var RolePowerDTO = new RolePowersDTO()
        //    {
        //        RoleId = rolePower.RoleId,
        //        RoleName = role.Name ?? "RoleName",
        //        TimeOfAddtion = role.TimeOfAddition,
        //        Powers = 
        //    };

        //    return RolePowerDTO;
        //}

        private List<RolePowersDTO> MapRoles(List<ApplicationRoles> roles)
        {
            var rolePowersDTO = new List<RolePowersDTO>();

            foreach (var role in roles)
            {
                if (role == null || role.Name == "Admin" || role.Name == "Merchant" || role.Name == "Representative")
                {
                    continue;
                }

                rolePowersDTO.Add(new RolePowersDTO()
                {
                    RoleId = role.Id,
                    RoleName = role.Name ?? "RoleName",
                    TimeOfAddtion = role.TimeOfAddition
                });
            }

            return rolePowersDTO;
        }


        public Task<PaginationDTO<RolePowersDTO>> GetPaginatedOrders(int pageNumber, int pageSize, Expression<Func<RolePowers, bool>> filter)
        {
            //var totalCount = await repository.Count();
            //var totalPages = await repository.Pages(pageSize);
            //var objectList = await repository.GetPaginatedElements(pageNumber, pageSize, filter);
            //var rolePowers = await MapRolePowers(objectList.ToList());

            var totalCount = roleManager.Roles.Count();
            var totalPages = (int)Math.Ceiling((double)(totalCount) / pageSize);
            var objectList = roleManager.Roles.Skip((pageNumber - 1) * pageSize).Take(pageSize);
            var roles = MapRoles(objectList.ToList());

            return Task.FromResult(new PaginationDTO<RolePowersDTO>()
            {
                TotalCount = totalCount,
                TotalPages = totalPages,
                List = roles
            });
        }
    }
}
