using Application.DTOs.DisplayDTOs;
using Application.DTOs.InsertDTOs;
using Application.DTOs.UpdateDTOs;
using Application.Interfaces;
using Application.Interfaces.ApplicationServices;
using Application.Interfaces.Repositories;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services
{
    public class GovernorateService: IGenericService<Governorate, GovernorateDTO, GovernorateInsertDTO, GovernorateUpdateDTO>
    {
        private readonly IUnitOfWork unit;
        private readonly IGenericRepository<Governorate> repository;

        public GovernorateService(IUnitOfWork unit)
        {
            this.unit = unit;
            this.repository = unit.GetGenericRepository<Governorate>();

        }

        public async Task<List<GovernorateDTO>> GetAllObjects()
        {
            var governorates = await repository.GetAllElements();

            return MapGovernorates(governorates);
        }

        public async Task<List<GovernorateDTO>> GetAllObjects(params Expression<Func<Governorate, object>>[] includes)
        {
            var governorates = await repository.GetAllElements(includes);

            return MapGovernorates(governorates);
        }

        public async Task<GovernorateDTO?> GetObject(Expression<Func<Governorate, bool>> filter)
        {
            var governorate = await repository.GetElement(filter);

            return MapGovernorate(governorate);
        }

        public async Task<GovernorateDTO?> GetObject(Expression<Func<Governorate, bool>> filter, params Expression<Func<Governorate, object>>[] includes)
        {
            var governorate = await repository.GetElement(filter, includes);

            return MapGovernorate(governorate);
        }

        public async Task<GovernorateDTO?> GetObjectWithoutTracking(Expression<Func<Governorate, bool>> filter)
        {
            var governorate = await repository.GetElementWithoutTracking(filter);

            return MapGovernorate(governorate);
        }

        public async Task<GovernorateDTO?> GetObjectWithoutTracking(Expression<Func<Governorate, bool>> filter, params Expression<Func<Governorate, object>>[] includes)
        {
            var governorate = await repository.GetElementWithoutTracking(filter, includes);

            return MapGovernorate(governorate);
        }

        public bool InsertObject(GovernorateInsertDTO governorateDTO)
        {
            var governorate = new Governorate()
            {
                name = governorateDTO.name,
                status = governorateDTO.status
            };

            var result = repository.Add(governorate);

            return result;
        }

        public bool UpdateObject(GovernorateUpdateDTO governorateDTO)
        {
            var governorate = new Governorate()
            {
                id = governorateDTO.id,
                name = governorateDTO.name,
                status = governorateDTO.status
            };

            var result = repository.Edit(governorate);

            return result;
        }

        public async Task<bool> DeleteObject(int governorateId)
        {
            var governorate = await repository.GetElement(g => g.id == governorateId);
            
            if (governorate != null)
            {
                var result = repository.Delete(governorate);

                return result;                
            }

            return false;
        }

        public async Task<bool> SaveChangesForObject()
        {
            var result = await unit.SaveChanges();

            return result;
        }

        private List<GovernorateDTO> MapGovernorates(List<Governorate> governorates)
        {
            var governoratesDTO = governorates.Select(g => new GovernorateDTO() { id = g.id, name = g.name, status = g.status }).ToList();

            return governoratesDTO;
        }

        private GovernorateDTO MapGovernorate(Governorate? governorate)
        {
            var governorateDTO = new GovernorateDTO()
            {
                id = governorate?.id ?? 0,
                name = governorate?.name ?? "",
                status = governorate?.status ?? Domain.Enums.Status.Inactive,
            };

            return governorateDTO;
        }
    }
}
