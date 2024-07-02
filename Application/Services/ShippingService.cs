using Application.DTOs.DisplayDTOs;
using Application.DTOs.InsertDTOs;
using Application.DTOs.UpdateDTOs;
using Application.Interfaces;
using Application.Interfaces.ApplicationServices;
using Application.Interfaces.Repositories;
using AutoMapper;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Application.Services
{
    public class ShippingService : IGenericService<Shipping, DisplayShippingDTO, InsertShippingDTO, UpdateShippingDTO, int>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IGenericRepository<Shipping> _repository;

        public ShippingService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _repository = _unitOfWork.GetGenericRepository<Shipping>();
        }

        public async Task<List<DisplayShippingDTO>> GetAllObjects()
        {
            var shippings = await _repository.GetAllElements();
            return _mapper.Map<List<DisplayShippingDTO>>(shippings);
        }

        public async Task<List<DisplayShippingDTO>> GetAllObjects(params System.Linq.Expressions.Expression<Func<Shipping, object>>[] includes)
        {
            var shippings = await _repository.GetAllElements(includes);
            return _mapper.Map<List<DisplayShippingDTO>>(shippings);
        }

        public async Task<DisplayShippingDTO?> GetObject(System.Linq.Expressions.Expression<Func<Shipping, bool>> filter)
        {
            var shipping = await _repository.GetElement(filter);
            return _mapper.Map<DisplayShippingDTO>(shipping);
        }

        public async Task<DisplayShippingDTO?> GetObject(System.Linq.Expressions.Expression<Func<Shipping, bool>> filter, params System.Linq.Expressions.Expression<Func<Shipping, object>>[] includes)
        {
            var shipping = await _repository.GetElement(filter, includes);
            return _mapper.Map<DisplayShippingDTO>(shipping);
        }

        public async Task<DisplayShippingDTO?> GetObjectWithoutTracking(System.Linq.Expressions.Expression<Func<Shipping, bool>> filter)
        {
            var shipping = await _repository.GetElementWithoutTracking(filter);
            return _mapper.Map<DisplayShippingDTO>(shipping);
        }

        public async Task<DisplayShippingDTO?> GetObjectWithoutTracking(System.Linq.Expressions.Expression<Func<Shipping, bool>> filter, params System.Linq.Expressions.Expression<Func<Shipping, object>>[] includes)
        {
            var shipping = await _repository.GetElementWithoutTracking(filter, includes);
            return _mapper.Map<DisplayShippingDTO>(shipping);
        }

        public Task<ModificationResultDTO> InsertObject(InsertShippingDTO shippingDTO)
        {
            var shipping = _mapper.Map<Shipping>(shippingDTO);
            var result = _repository.Add(shipping);
            if (result == false)
            {
                return Task.FromResult(new ModificationResultDTO()
                {
                    Succeeded = false,
                    Message = "Error inserting the shipping object"
                });
            }

            return Task.FromResult(new ModificationResultDTO()
            {
                Succeeded = true
            });
        }

        public Task<ModificationResultDTO> UpdateObject(UpdateShippingDTO shippingDTO)
        {
            var shipping = _mapper.Map<Shipping>(shippingDTO);
            var result = _repository.Edit(shipping);

            if (result == false)
            {
                return Task.FromResult(new ModificationResultDTO()
                {
                    Succeeded = false,
                    Message = "Error updating the shipping object"
                });
            }

            return Task.FromResult(new ModificationResultDTO()
            {
                Succeeded = true
            });
        }

        public async Task<ModificationResultDTO> DeleteObject(int shippingId)
        {
            var shipping = await _repository.GetElement(x => x.Id == shippingId);

            if (shipping == null)
            {
                return new ModificationResultDTO()
                {
                    Succeeded = false,
                    Message = "shipping object doesn't exist in the db"
                };
            }

            var result = _repository.Delete(shipping);

            if (result == false)
            {
                return new ModificationResultDTO()
                {
                    Succeeded = false,
                    Message = "Error deleting the shipping object"
                };
            }

            return new ModificationResultDTO()
            {
                Succeeded = true
            };
        }

        public async Task<bool> SaveChangesForObject()
        {
            return await _unitOfWork.SaveChanges();
        }
    }
}
