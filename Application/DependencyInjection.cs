
﻿using Application.DTOs.DisplayDTOs;
using Application.DTOs.InsertDTOs;
using Application.DTOs.UpdateDTOs;
using Application.Interfaces.ApplicationServices;
using Application.Services;
using Domain.Entities;
﻿using Application.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Services;

namespace Application
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddApplication(this IServiceCollection services)
        {
            services.AddAutoMapper(typeof(Mappings.MappingProfile));

            services.AddScoped<IGenericService<Governorate, GovernorateDTO, GovernorateInsertDTO, GovernorateUpdateDTO, int>, GovernorateService>();
            services.AddScoped<IGenericService<RolePowers, RolePowersDTO, RolePowersInsertDTO, RolePowersUpdateDTO, string>, RolePowersService>();
            //services.AddScoped<IGenericService<Order, DisplayOrderDTO, InsertOrderDTO, UpdateOrderDTO, int>, OrderService>();
            services.AddScoped<IGenericService<Product, DisplayProductDTO, InsertProductDTO, UpdateProductDTO, int>, ProductService>();
            services.AddScoped<IGenericService<Payment, DisplayPaymentDTO, InsertPaymentDTO, UpdatePaymentDTO, int>, PaymentService>();
            services.AddScoped<IGenericService<Shipping, DisplayShippingDTO, InsertShippingDTO, UpdateShippingDTO, int>, ShippingService>();

            services.AddScoped<IPaginationService<Order, DisplayOrderDTO, InsertOrderDTO, UpdateOrderDTO, int>, OrderService>();

            services.AddAutoMapper(typeof(Mappings.MappingProfile));
            services.AddScoped<IGenericService<Order, DisplayOrderDTO, InsertOrderDTO, UpdateOrderDTO,int>, OrderService>();
            services.AddScoped<IGenericService<Product, DisplayProductDTO, InsertProductDTO, UpdateProductDTO,int>, ProductService>();
            services.AddScoped<IGenericService<Shipping, DisplayShippingDTO, InsertShippingDTO, UpdateShippingDTO,int>, ShippingService>();
            services.AddScoped<IGenericService<Payment, DisplayPaymentDTO, InsertPaymentDTO, UpdatePaymentDTO,int>, PaymentService>();
            services.AddScoped<IGenericService<Branch, BranchDisplayDTO, BranchInsertDTO, BranchUpdateDTO,int>, BranchService>();
            services.AddScoped<IGenericService<City, CityDisplayDTO, CityInsertDTO, CityUpdateDTO,int>, CityService>();
            services.AddScoped<IGenericService<Merchant, MerchantResponseDto, MerchantAddDto, MerchantUpdateDto, string>, MerchantService>();
            services.AddScoped<IEmployeeService, EmployeeService>();


            return services;
        }
    }
}
