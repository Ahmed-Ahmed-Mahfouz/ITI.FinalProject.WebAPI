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
    public class OrderService : IGenericService<Order, DisplayOrderDTO, InsertOrderDTO, UpdateOrderDTO, int>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IGenericRepository<Order> _repository;

        public OrderService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _repository = _unitOfWork.GetGenericRepository<Order>();
        }

        public async Task<List<DisplayOrderDTO>> GetAllObjects()
        {
            var orders = await _repository.GetAllElements();
            return _mapper.Map<List<DisplayOrderDTO>>(orders);
        }

        public async Task<List<DisplayOrderDTO>> GetAllObjects(params System.Linq.Expressions.Expression<Func<Order, object>>[] includes)
        {
            var orders = await _repository.GetAllElements(includes);
            return _mapper.Map<List<DisplayOrderDTO>>(orders);
        }

        public async Task<DisplayOrderDTO?> GetObject(System.Linq.Expressions.Expression<Func<Order, bool>> filter)
        {
            var order = await _repository.GetElement(filter);
            return _mapper.Map<DisplayOrderDTO>(order);
        }

        public async Task<DisplayOrderDTO?> GetObject(System.Linq.Expressions.Expression<Func<Order, bool>> filter, params System.Linq.Expressions.Expression<Func<Order, object>>[] includes)
        {
            var order = await _repository.GetElement(filter, includes);
            return _mapper.Map<DisplayOrderDTO>(order);
        }

        public async Task<DisplayOrderDTO?> GetObjectWithoutTracking(System.Linq.Expressions.Expression<Func<Order, bool>> filter)
        {
            var order = await _repository.GetElementWithoutTracking(filter);
            return _mapper.Map<DisplayOrderDTO>(order);
        }

        public async Task<DisplayOrderDTO?> GetObjectWithoutTracking(System.Linq.Expressions.Expression<Func<Order, bool>> filter, params System.Linq.Expressions.Expression<Func<Order, object>>[] includes)
        {
            var order = await _repository.GetElementWithoutTracking(filter, includes);
            return _mapper.Map<DisplayOrderDTO>(order);
        }

        public Task<bool> InsertObject(InsertOrderDTO orderDTO)
        {
            var order = _mapper.Map<Order>(orderDTO);
            var result =  _repository.Add(order); 
            return Task.FromResult(result);
        }

        public Task<bool> UpdateObject(UpdateOrderDTO orderDTO)
        {
            var order = _mapper.Map<Order>(orderDTO);
            var result = _repository.Edit(order); 
            return Task.FromResult(result);
        }

        public async Task<bool> DeleteObject(int orderId)
        {
            var order = await _repository.GetElement(x => x.Id == orderId);
            var result = _repository.Delete(order);
            return result;
        }

        public async Task<bool> SaveChangesForObject()
        {
            return await _unitOfWork.SaveChanges();
        }
    }
}
