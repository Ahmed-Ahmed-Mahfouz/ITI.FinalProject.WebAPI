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
    public class RolePowersService : IGenericService<RolePowers, RolePowersDTO, RolePowersInsertDTO, RolePowersUpdateDTO, string>
    {
        private readonly IUnitOfWork unit;
        private readonly RoleManager<ApplicationRoles> roleManager;
        private readonly IGenericRepository<RolePowers> repository;

        public RolePowersService(IUnitOfWork<RolePowers> unit)
        {
            this.unit = unit;
            this.roleManager = roleManager;
            repository = unit.GetGenericRepository<RolePowers>();
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

            return await MapRolePower(rolePower);
        }

        public async Task<RolePowersDTO?> GetObject(Expression<Func<RolePowers, bool>> filter, params Expression<Func<RolePowers, object>>[] includes)
        {
            var rolePower = await repository.GetElement(filter, includes);

            return await MapRolePower(rolePower);
        }

        public async Task<RolePowersDTO?> GetObjectWithoutTracking(Expression<Func<RolePowers, bool>> filter)
        {
            var rolePower = await repository.GetElementWithoutTracking(filter);

            return await MapRolePower(rolePower);
        }

        public async Task<RolePowersDTO?> GetObjectWithoutTracking(Expression<Func<RolePowers, bool>> filter, params Expression<Func<RolePowers, object>>[] includes)
        {
            var rolePower = await repository.GetElementWithoutTracking(filter, includes);

            return await MapRolePower(rolePower);
        }

        public async Task<bool> InsertObject(RolePowersInsertDTO rolePowersInsertDTO)
        {
            var role = new ApplicationRoles()
            {
                Name = rolePowersInsertDTO.RoleName,
                TimeOfAddtion = DateTime.Now
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
                        return result;
                    }
                }

                return true;
            }

            return false;
        }

        public async Task<bool> UpdateObject(RolePowersUpdateDTO rolePowersUpdateDTO)
        {

            var role = await roleManager.FindByIdAsync(rolePowersUpdateDTO.RoleId);

            if (role == null)
            {
                return false;
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
                        return false;
                    }

                    rolePower.Power = power;

                    var result = repository.Edit(rolePower);

                    if (result == false)
                    {
                        return result;
                    }
                }

                return true;
            }

            return false;
        }

        public async Task<bool> DeleteObject(string rolePowersId)
        {
            var rolePowers = await repository.GetAllElements(rp => rp.RoleId == rolePowersId);

            foreach (var item in rolePowers)
            {
                var result = repository.Delete(item);

                if (result == false)
                {
                    return result;
                }
            }

            var role = await roleManager.FindByIdAsync(rolePowersId);

            if (role == null)
            {
                return false;
            }

            var identityResult = await roleManager.DeleteAsync(role);

            if (identityResult.Succeeded)
            {
                return true;
            }

            return false;
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
                    TimeOfAddtion = role.TimeOfAddtion,
                    Power = item.Power
                });
            }

            return rolePowersDTO;
        }

        private async Task<RolePowersDTO> MapRolePower(RolePowers? rolePower)
        {
            var role = await roleManager.FindByIdAsync(rolePower.RoleId);

            var RolePowerDTO = new RolePowersDTO()
            {
                RoleId = rolePower.RoleId,
                RoleName = role.Name,
                TimeOfAddtion = role.TimeOfAddtion,
                Power = rolePower.Power
            };

            return RolePowerDTO;
        }
    }
}
