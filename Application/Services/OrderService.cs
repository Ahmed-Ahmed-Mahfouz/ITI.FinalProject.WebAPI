using Application.DTOs.DisplayDTOs;
using Application.DTOs.InsertDTOs;
using Application.DTOs.UpdateDTOs;
using Application.Interfaces;
using Application.Interfaces.ApplicationServices;
using Application.Interfaces.Repositories;
using AutoMapper;
using Domain.Entities;
using Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services
{
    public class OrderService : IPaginationService<Order, DisplayOrderDTO, InsertOrderDTO, UpdateOrderDTO, int>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IPaginationRepository<Order> _repository;
        private readonly IGenericRepository<City> _cityRepository;
        private readonly IGenericRepository<Settings> _settingsRepository;
        private readonly IGenericRepository<Merchant> _merchantRepository;
        private readonly IGenericRepository<Shipping> _shippingRepository;

        public OrderService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _repository = _unitOfWork.GetPaginationRepository<Order>();
            _cityRepository = _unitOfWork.GetGenericRepository<City>();
            _settingsRepository = _unitOfWork.GetGenericRepository<Settings>();
            _merchantRepository = _unitOfWork.GetGenericRepository<Merchant>();
            _shippingRepository = _unitOfWork.GetGenericRepository<Shipping>();
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

        public async Task<ModificationResultDTO> InsertObject(InsertOrderDTO orderDTO)
        {
            var order = _mapper.Map<Order>(orderDTO);

            //order.ShippingCost = await CalculateShipmentCost(order);
            order.ShippingCost = 0;

            var result =  _repository.Add(order);

            if (result == false)
            {
                return new ModificationResultDTO()
                {
                    Succeeded = false,
                    Message = "Error inserting the order"
                };
            }

            return new ModificationResultDTO()
            {
                Succeeded = true
            };
        }

        public async Task<ModificationResultDTO> UpdateObject(UpdateOrderDTO orderDTO)
        {
            var order = _mapper.Map<Order>(orderDTO);

            //order.ShippingCost = await CalculateShipmentCost(order);
            order.ShippingCost = 0;

            var result = _repository.Edit(order);

            if (result == false)
            {
                return new ModificationResultDTO()
                {
                    Succeeded = false,
                    Message = "Error updating the order"
                };
            }

            return new ModificationResultDTO()
            {
                Succeeded = true
            };
        }

        public async Task<ModificationResultDTO> DeleteObject(int orderId)
        {
            var order = await _repository.GetElement(x => x.Id == orderId);

            if (order == null)
            {
                return new ModificationResultDTO()
                {
                    Succeeded = false,
                    Message = "Order doesn't exist in the db"
                };
            }

            var result = _repository.Delete(order);

            if (result == false)
            {
                return new ModificationResultDTO()
                {
                    Succeeded = false,
                    Message = "Error deleting the order"
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

        public async Task<PaginationDTO<DisplayOrderDTO>> GetPaginatedOrders(int pageNumber, int pageSize, Expression<Func<Order, bool>> filter)
        {
            var totalOrders = await _repository.Count(); 
            var totalPages = await _repository.Pages(pageSize);
            var orders = await _repository.GetPaginatedElements(pageNumber, pageSize, filter); 
            var mappedOrders = _mapper.Map<List<DisplayOrderDTO>>(orders);
            return new PaginationDTO<DisplayOrderDTO>()
            {
                TotalCount = totalOrders,
                TotalPages = totalPages,
                List = mappedOrders
            };
        }

        public async Task<decimal> CalculateShipmentCost(Order order)
        {
            var settings = (await _settingsRepository.GetAllElements())[0];
            var city = await _cityRepository.GetElement(c => c.id == order.CityId);
            var merchant = await _merchantRepository.GetElement(m => m.Id == order.MerchantId, c => c.SpecialPackages);
            var shipping = await _shippingRepository.GetElement(s => s.Id == order.ShippingId);
            var orderType = order.Type;
            var shippingType = shipping!.ShippingType;
            var totalWeight = order.TotalWeight;
            decimal shippingCost = 0;

            switch (orderType)
            {
                case OrderTypes.BranchDelivery:

                    var merchantSpecialPackage = merchant?.SpecialPackages.FirstOrDefault(sp => sp.cityId == order.CityId);
                    
                    if (merchantSpecialPackage != null)
                    {
                        shippingCost += merchantSpecialPackage.ShippingPrice;
                    }
                    else
                    {
                        shippingCost += city!.normalShippingCost;
                    }

                break;

                case OrderTypes.HomeDelivery:

                    if (merchant?.SpecialPickupShippingCost != null)
                    {
                        shippingCost += (decimal)merchant.SpecialPickupShippingCost;
                    }
                    else
                    {
                        shippingCost += city!.pickupShippingCost;
                    }

                break;
            }

            switch (shippingType)
            {
                case ShippingTypes.Ordinary:
                    shippingCost += settings.OrdinaryShippingCost;
                break;

                case ShippingTypes.Within24Hours:
                    shippingCost += settings.TwentyFourHoursShippingCost;
                break;

                case ShippingTypes.Within15Days:
                    shippingCost += settings.FifteenDayShippingCost;
                break;
            }

            if (totalWeight > settings.BaseWeight)
            {
                decimal additionalWeight = totalWeight - settings.BaseWeight;
                shippingCost += additionalWeight * settings.AdditionalFeePerKg;
            }

            if (order.ShippingToVillage)
            {
                shippingCost += settings.VillageDeliveryFee;
            }

            return shippingCost;
        }
    }
}
