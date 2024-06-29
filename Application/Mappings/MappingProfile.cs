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
                .ForMember(dest => dest.MerchantName, opt => opt.MapFrom(src => src.Merchant.StoreName))
                .ForMember(dest => dest.GovernorateName, opt => opt.MapFrom(src => src.Governorate.name))
                .ForMember(dest => dest.CityName, opt => opt.MapFrom(src => src.City.name))
                .ForMember(dest => dest.PaymentMethod, opt => opt.MapFrom(src => src.Payment.PaymentType))
                .ForMember(dest => dest.ShippingMethod, opt => opt.MapFrom(src => src.Shipping.ShippingType))
                .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.Status));
            CreateMap<InsertOrderDTO, Order>();
            CreateMap<UpdateOrderDTO, Order>();

            // Product Mappings
            CreateMap<Product, DisplayProductDTO>()
                .ForMember(dest => dest.ClientName, opt => opt.MapFrom(src => src.Order.ClientName))
                .ForMember(dest => dest.ProductStatus, opt => opt.MapFrom(src => src.ProductStatus));
            CreateMap<InsertProductDTO, Product>();
            CreateMap<UpdateProductDTO, Product>();

            // Payment Mappings
            CreateMap<Payment, DisplayPaymentDTO>()
                .ForMember(dest => dest.PaymentMethod, opt => opt.MapFrom(src => src.PaymentType));
            CreateMap<InsertPaymentDTO, Payment>();
            CreateMap<UpdatePaymentDTO, Payment>();

            // Shipping Mappings
            CreateMap<Shipping, DisplayShippingDTO>()
                .ForMember(dest => dest.ShippingMethod, opt => opt.MapFrom(src => src.ShippingType));
            CreateMap<InsertShippingDTO, Shipping>();
            CreateMap<UpdateShippingDTO, Shipping>();

        }
    }
}
