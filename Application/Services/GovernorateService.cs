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
    public class GovernorateService: IPaginationService<Governorate, GovernorateDTO, GovernorateInsertDTO, GovernorateUpdateDTO, int>
    {
        private readonly IUnitOfWork unit;
        private readonly IPaginationRepository<Governorate> repository;

        public GovernorateService(IUnitOfWork unit)
        {
            this.unit = unit;
            this.repository = unit.GetPaginationRepository<Governorate>();

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

            if (governorate == null)
            {
                return null;
            }

            return MapGovernorate(governorate);
        }

        public async Task<GovernorateDTO?> GetObject(Expression<Func<Governorate, bool>> filter, params Expression<Func<Governorate, object>>[] includes)
        {
            var governorate = await repository.GetElement(filter, includes);

            if (governorate == null)
            {
                return null;
            }

            return MapGovernorate(governorate);
        }

        public async Task<GovernorateDTO?> GetObjectWithoutTracking(Expression<Func<Governorate, bool>> filter)
        {
            var governorate = await repository.GetElementWithoutTracking(filter);

            if (governorate == null)
            {
                return null;
            }

            return MapGovernorate(governorate);
        }

        public async Task<GovernorateDTO?> GetObjectWithoutTracking(Expression<Func<Governorate, bool>> filter, params Expression<Func<Governorate, object>>[] includes)
        {
            var governorate = await repository.GetElementWithoutTracking(filter, includes);

            if (governorate == null)
            {
                return null;
            }

            return MapGovernorate(governorate);
        }

        //public Task<bool> InsertObject(GovernorateInsertDTO governorateInsertDTO, out GovernorateDTO governorateDTO)
        public Task<ModificationResultDTO> InsertObject(GovernorateInsertDTO governorateInsertDTO)
        {
            var governorate = new Governorate()
            {
                name = governorateInsertDTO.name,
                status = governorateInsertDTO.status
            };

            var result = repository.Add(governorate);

            if (result == false)
            {
                return Task.FromResult(new ModificationResultDTO()
                {
                    Succeeded = false,
                    Message = "Error inserting the governorate"
                });
            }

            return Task.FromResult(new ModificationResultDTO()
            {
                Succeeded = true
            });
        }

        public Task<ModificationResultDTO> UpdateObject(GovernorateUpdateDTO governorateDTO)
        {
            var governorate = new Governorate()
            {
                id = governorateDTO.id,
                name = governorateDTO.name,
                status = governorateDTO.status
            };

            var result = repository.Edit(governorate);

            if (result == false)
            {
                return Task.FromResult(new ModificationResultDTO()
                {
                    Succeeded = false,
                    Message = "Error updating the governorate"
                });
            }

            return Task.FromResult(new ModificationResultDTO()
            {
                Succeeded = true
            });
        }

        public async Task<ModificationResultDTO> DeleteObject(int governorateId)
        {
            var governorate = await repository.GetElement(g => g.id == governorateId);
            
            if (governorate != null)
            {
                var result = repository.Delete(governorate);

                if (result == false)
                {
                    return new ModificationResultDTO()
                    {
                        Succeeded = false,
                        Message = "Error deleting the governorate"
                    };
                }

                return new ModificationResultDTO()
                {
                    Succeeded = true
                };
            }

            return new ModificationResultDTO()
            {
                Succeeded = false,
                Message = "Governorate doesn't exist in the db"
            };
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

        public Task<(List<GovernorateDTO>, int)> GetPaginatedOrders(int pageNumber, int pageSize)
        {
            throw new NotImplementedException();
        }
    }
}
