using AutoMapper;
using Domain.Entities;
using Application.DTOs.DisplayDTOs;
using Application.DTOs.InsertDTOs;
using Application.DTOs.UpdateDTOs;

namespace Application.Mappings
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            // Order Mappings
            CreateMap<Order, DisplayOrderDTO>()
                .ForMember(dest => dest.MerchantName, opt => opt.MapFrom(src => src.merchant.user.FullName)) //Possible Error
                .ForMember(dest => dest.GovernorateName, opt => opt.MapFrom(src => src.governorate.name))
                .ForMember(dest => dest.CityName, opt => opt.MapFrom(src => src.city.name))
                .ForMember(dest => dest.ShippingType, opt => opt.MapFrom(src => src.shipping.ShippingType))
                .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.Status));
            CreateMap<InsertOrderDTO, Order>();
            CreateMap<UpdateOrderDTO, Order>();

            // Product Mappings
            CreateMap<Product, DisplayProductDTO>()
                .ForMember(dest => dest.ClientName, opt => opt.MapFrom(src => src.order.ClientName))
                .ForMember(dest => dest.ProductStatus, opt => opt.MapFrom(src => src.ProductStatus));
            CreateMap<InsertProductDTO, Product>();
            CreateMap<UpdateProductDTO, Product>();

            // Shipping Mappings
            CreateMap<Shipping, DisplayShippingDTO>()
                .ForMember(dest => dest.ShippingType, opt => opt.MapFrom(src => src.ShippingType));
            CreateMap<InsertShippingDTO, Shipping>();
            CreateMap<UpdateShippingDTO, Shipping>();

        }
    }
}
