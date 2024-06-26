using Application.DTOs.DisplayDTOs;
using Application.DTOs.InsertDTOs;
using Application.DTOs.UpdateDTOs;
using Application.Interfaces;
using AutoMapper;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services
{
    public class OrderService : IOrderService
    {
        private readonly IUnitOfWork<Order> _orderUnitOfWork;
        private readonly IMapper _mapper;

        public OrderService(IUnitOfWork<Order> orderUnitOfWork, IMapper mapper)
        {
            _orderUnitOfWork = orderUnitOfWork;
            _mapper = mapper;
        }
        public async Task<DisplayOrderDTO> CreateOrderAsync(InsertOrderDTO insertOrderDto)
        {
            var order = _mapper.Map<Order>(insertOrderDto);
            _orderUnitOfWork.Repository.Add(order);
            await _orderUnitOfWork.SaveChangesAsync();
            return _mapper.Map<DisplayOrderDTO>(order);
        }

        public async Task DeleteOrderByIdAsync(int id)
        {
            var order = await _orderUnitOfWork.Repository.GetElement(o => o.Id == id);
            _orderUnitOfWork.Repository.Delete(order);
            await _orderUnitOfWork.SaveChangesAsync();
        }

        public async Task<IEnumerable<DisplayOrderDTO>> GetAllOrdersAsync()
        {
            var orders = await _orderUnitOfWork.Repository.GetAllElements();
            return _mapper.Map<IEnumerable<DisplayOrderDTO>>(orders);
        }

        public Task<DisplayOrderDTO> GetOrderByIdAsync(int id)
        {
            var order = _orderUnitOfWork.Repository.GetElement(o => o.Id == id);
            return Task.FromResult(_mapper.Map<DisplayOrderDTO>(order));
        }

        public Task<DisplayOrderDTO> UpdateOrderAsync(UpdateOrderDTO updateOrderDto)
        {
            var order = _mapper.Map<Order>(updateOrderDto);
            _orderUnitOfWork.Repository.Edit(order);
            _orderUnitOfWork.SaveChangesAsync();
            return Task.FromResult(_mapper.Map<DisplayOrderDTO>(order));
        }
    }
}
