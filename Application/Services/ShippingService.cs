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
    public class ShippingService : IGenericService<Shipping, DisplayShippingDTO, InsertShippingDTO, UpdateShippingDTO>
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

        public bool InsertObject(InsertShippingDTO shippingDTO)
        {
            var shipping = _mapper.Map<Shipping>(shippingDTO);
            _repository.Add(shipping);
            return true;
        }

        public bool UpdateObject(UpdateShippingDTO shippingDTO)
        {
            var shipping = _mapper.Map<Shipping>(shippingDTO);
            _repository.Edit(shipping);
            return true;
        }

        public async Task<bool> DeleteObject(int shippingId)
        {
            var shipping = await _repository.GetElement(x => x.Id == shippingId);
            _repository.Delete(shipping);
            return true;
        }

        public async Task<bool> SaveChangesForObject()
        {
            return await _unitOfWork.SaveChanges();
        }
    }
}
