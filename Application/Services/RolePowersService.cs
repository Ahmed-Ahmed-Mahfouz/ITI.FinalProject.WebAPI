using Application.DTOs.DisplayDTOs;
using Application.DTOs.InsertDTOs;
using Application.DTOs.UpdateDTOs;
using Application.Interfaces;
using Application.Interfaces.ApplicationServices;
using Application.Interfaces.Repositories;
using Domain.Entities;
using Microsoft.AspNetCore.Identity;
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
            var rolePowers = await repository.GetAllElements();

            return await MapRolePowers(rolePowers);
        }

        public async Task<List<RolePowersDTO>> GetAllObjects(params Expression<Func<RolePowers, object>>[] includes)
        {
            var rolePowers = await repository.GetAllElements(includes);

            return await MapRolePowers(rolePowers);
        }

        public async Task<RolePowersDTO?> GetObject(Expression<Func<RolePowers, bool>> filter)
        {
            var rolePower = await repository.GetElement(filter);

            if (rolePower == null)
            {
                return null;
            }

            return await MapRolePower(rolePower);
        }

        public async Task<RolePowersDTO?> GetObject(Expression<Func<RolePowers, bool>> filter, params Expression<Func<RolePowers, object>>[] includes)
        {
            var rolePower = await repository.GetElement(filter, includes);

            if (rolePower == null)
            {
                return null;
            }

            return await MapRolePower(rolePower);
        }

        public async Task<RolePowersDTO?> GetObjectWithoutTracking(Expression<Func<RolePowers, bool>> filter)
        {
            var rolePower = await repository.GetElementWithoutTracking(filter);

            if (rolePower == null)
            {
                return null;
            }

            return await MapRolePower(rolePower);
        }

        public async Task<RolePowersDTO?> GetObjectWithoutTracking(Expression<Func<RolePowers, bool>> filter, params Expression<Func<RolePowers, object>>[] includes)
        {
            var rolePower = await repository.GetElementWithoutTracking(filter, includes);

            if (rolePower == null)
            {
                return null;
            }

            return await MapRolePower(rolePower);
        }

        public async Task<ModificationResultDTO> InsertObject(RolePowersInsertDTO rolePowersInsertDTO)
        {
            var role = new ApplicationRoles()
            {
                Name = rolePowersInsertDTO.RoleName,
                TimeOfAddition = DateTime.Now
            };

            var identityResult = await roleManager.CreateAsync(role);

            if (identityResult.Succeeded)
            {
                foreach (var power in rolePowersInsertDTO.Powers)
                {
                    var result = repository.Add(new RolePowers()
                    {
                        RoleId = role.Id,
                        Power = power
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
                    var rolePower = await repository.GetElement(rp => rp.RoleId == rolePowersUpdateDTO.RoleId);

                    if (rolePower == null)
                    {
                        return new ModificationResultDTO()
                        {
                            Succeeded = false,
                            Message = "Role power doesn't exist in the db"
                        };
                    }

                    rolePower.Power = power;

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

        private async Task<List<RolePowersDTO>> MapRolePowers(List<RolePowers> rolePowers)
        {

            //var rolePowersDTO = rolePowers.Select(g => new RolePowersDTO() { RoleId = g.RoleId, }).ToList();

            var rolePowersDTO = new List<RolePowersDTO>();

            foreach (var item in rolePowers)
            {
                var role = await roleManager.FindByIdAsync(item.RoleId);

                rolePowersDTO.Add(new RolePowersDTO()
                {
                    RoleId = item.RoleId,
                    RoleName = role.Name,
                    TimeOfAddtion = role.TimeOfAddition,
                    Power = item.Power
                });
            }

            return rolePowersDTO;
        }

        private async Task<RolePowersDTO> MapRolePower(RolePowers rolePower)
        {
            var role = await roleManager.FindByIdAsync(rolePower.RoleId);

            var RolePowerDTO = new RolePowersDTO()
            {
                RoleId = rolePower.RoleId,
                RoleName = role.Name,
                TimeOfAddtion = role.TimeOfAddition,
                Power = rolePower.Power
            };

            return RolePowerDTO;
        }

        public Task<(List<RolePowersDTO>, int)> GetPaginatedOrders(int pageNumber, int pageSize)
        {
            throw new NotImplementedException();
        }
    }
}
