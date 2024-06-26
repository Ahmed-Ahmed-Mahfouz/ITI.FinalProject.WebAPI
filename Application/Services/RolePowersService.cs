using Application.DTOs.DisplayDTOs;
using Application.DTOs.InsertDTOs;
using Application.DTOs.UpdateDTOs;
using Application.Interfaces;
using Application.Interfaces.ApplicationServices;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services
{
    internal class RolePowersService : IGenericService<RolePowers, RolePowersDTO, RolePowersInsertDTO, RolePowersUpdateDTO>
    {
        private readonly IUnitOfWork<RolePowers> unit;

        public RolePowersService(IUnitOfWork<RolePowers> unit)
        {
            this.unit = unit;
        }

        public Task<List<RolePowersDTO>> GetAllObjects()
        {
            throw new NotImplementedException();
        }

        public Task<List<RolePowersDTO>> GetAllObjects(params Expression<Func<RolePowers, object>>[] includes)
        {
            throw new NotImplementedException();
        }

        public Task<RolePowersDTO?> GetObject(Expression<Func<RolePowers, bool>> filter)
        {
            throw new NotImplementedException();
        }

        public Task<RolePowersDTO?> GetObject(Expression<Func<RolePowers, bool>> filter, params Expression<Func<RolePowers, object>>[] includes)
        {
            throw new NotImplementedException();
        }

        public Task<RolePowersDTO?> GetObjectWithoutTracking(Expression<Func<RolePowers, bool>> filter)
        {
            throw new NotImplementedException();
        }

        public Task<RolePowersDTO?> GetObjectWithoutTracking(Expression<Func<RolePowers, bool>> filter, params Expression<Func<RolePowers, object>>[] includes)
        {
            throw new NotImplementedException();
        }

        public bool InsertObject(RolePowersInsertDTO rolePowersInsertDTO)
        {
            throw new NotImplementedException();
        }

        public bool UpdateObject(RolePowersUpdateDTO rolePowersUpdateDTO)
        {
            throw new NotImplementedException();
        }

        public Task<bool> DeleteObject(int rolePowersId)
        {
            throw new NotImplementedException();
        }

        public Task<bool> SaveChangesForObject()
        {
            throw new NotImplementedException();
        }

        private List<RolePowersDTO> MapRolePowers(List<RolePowers> rolePowers)
        {
            var RolePowersDTO = rolePowers.Select(g => new RolePowersDTO() {  }).ToList();

            return RolePowersDTO;
        }

        private RolePowersDTO MapRolePower(RolePowers? rolePower)
        {
            var RolePowerDTO = new RolePowersDTO()
            {
                
            };

            return RolePowerDTO;
        }
    }
}
