
using Application.DTOs.DisplayDTOs;
using Application.DTOs.InsertDTOs;
using Application.DTOs.UpdateDTOs;
using Application.Interfaces.ApplicationServices;
using Application.Interfaces.Repositories;
using Application.Services;
using Domain.Entities;
using Domain.Interfaces;
using Domain.Services;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Application
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddApplication(this IServiceCollection services)
        {
            services.AddAutoMapper(typeof(Mappings.MappingProfile));

            services.AddScoped<IPaginationService<Governorate, GovernorateDTO, GovernorateInsertDTO, GovernorateUpdateDTO, int>, GovernorateService>();
            services.AddScoped<IPaginationService<Representative, RepresentativeDisplayDTO, RepresentativeInsertDTO, RepresentativeUpdateDTO, string>, RepresentativeService>();
            services.AddScoped<IPaginationService<RolePowers, RolePowersDTO, RolePowersInsertDTO, RolePowersUpdateDTO, string>, RolePowersService>();
            //services.AddScoped<IGenericService<Order, DisplayOrderDTO, InsertOrderDTO, UpdateOrderDTO, int>, OrderService>();
            services.AddScoped<IGenericService<Product, DisplayProductDTO, InsertProductDTO, UpdateProductDTO, int>, ProductService>();
            services.AddScoped<IGenericService<Shipping, DisplayShippingDTO, InsertShippingDTO, UpdateShippingDTO, int>, ShippingService>();
            services.AddScoped<IPaginationService<Employee, EmployeeReadDto , EmployeeAddDto , EmployeeupdateDto , string>, EmployeeService>();

            services.AddScoped<IPaginationService<Order, DisplayOrderDTO, InsertOrderDTO, UpdateOrderDTO, int>, Application.Services.OrderService>();
            services.AddScoped<IPaginationService<City, CityDisplayDTO, CityInsertDTO, CityUpdateDTO, int>, CityService>();
            services.AddScoped<IPaginationService<Branch, BranchDisplayDTO, BranchInsertDTO, BranchUpdateDTO, int>, BranchService>();

            services.AddScoped<IOrderService, Domain.Services.OrderService>();


            services.AddAutoMapper(typeof(Mappings.MappingProfile));
           // services.AddScoped<IGenericService<Order, DisplayOrderDTO, InsertOrderDTO, UpdateOrderDTO,int>, OrderService>();
            services.AddScoped<IGenericService<Product, DisplayProductDTO, InsertProductDTO, UpdateProductDTO,int>, ProductService>();
            services.AddScoped<IGenericService<Shipping, DisplayShippingDTO, InsertShippingDTO, UpdateShippingDTO,int>, ShippingService>();
           // services.AddScoped<IGenericService<Payment, DisplayPaymentDTO, InsertPaymentDTO, UpdatePaymentDTO,int>, PaymentService>();
            services.AddScoped<IGenericService<Branch, BranchDisplayDTO, BranchInsertDTO, BranchUpdateDTO,int>, BranchService>();
            services.AddScoped<IGenericService<City, CityDisplayDTO, CityInsertDTO, CityUpdateDTO,int>, CityService>();
           services.AddScoped<IPaginationService<Merchant, MerchantResponseDto, MerchantAddDto, MerchantUpdateDto, string>, MerchantService>();
            // services.AddScoped<IEmployeeService, EmployeeService>();


            return services;
        }
    }
}
