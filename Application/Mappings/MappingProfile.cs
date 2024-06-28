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
                .ForMember(dest => dest.StoreName, opt => opt.MapFrom(src => src.merchant.StoreName))
                .ForMember(dest => dest.GovernorateName, opt => opt.MapFrom(src => src.governorate.name))
                .ForMember(dest => dest.CityName, opt => opt.MapFrom(src => src.city.name))
                .ForMember(dest => dest.PaymentMethod, opt => opt.MapFrom(src => src.payment.paymentType))
                .ForMember(dest => dest.ShippingMethod, opt => opt.MapFrom(src => src.shipping.ShippingType))
                .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.Status.ToString()));
            CreateMap<InsertOrderDTO, Order>();
            CreateMap<UpdateOrderDTO, Order>();

            // Product Mappings
            CreateMap<Product, DisplayProductDTO>()
                .ForMember(dest => dest.ClientName, opt => opt.MapFrom(src => src.order.Client_Name))
                .ForMember(dest => dest.ProductStatus, opt => opt.MapFrom(src => src.ProductStatus.ToString()));
            CreateMap<InsertProductDTO, Product>();
            CreateMap<UpdateProductDTO, Product>();

            // Payment Mappings
            CreateMap<Payment, DisplayPaymentDTO>()
                .ForMember(dest => dest.PaymentMethod, opt => opt.MapFrom(src => src.paymentType.ToString()));
            CreateMap<InsertPaymentDTO, Payment>();
            CreateMap<UpdatePaymentDTO, Payment>();

            // Shipping Mappings
            CreateMap<Shipping, DisplayShippingDTO>()
                .ForMember(dest => dest.ShippingMethod, opt => opt.MapFrom(src => src.ShippingType.ToString()));
            CreateMap<InsertShippingDTO, Shipping>();
            CreateMap<UpdateShippingDTO, Shipping>();

        }
    }
}
