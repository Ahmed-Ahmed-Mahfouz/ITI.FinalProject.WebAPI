using Domain.Entities;
using Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Application.Interfaces;

namespace Infrastructure
{
    public static class DependencyInjection
    {
       
        public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<ShippingContext>(options =>
                options.UseSqlServer(configuration.GetConnectionString("con"),
                    b => b.MigrationsAssembly("Infrastructure"))); 

            services.AddIdentityCore<ApplicationUser>().AddRoles<ApplicationRoles>().AddEntityFrameworkStores<ShippingContext>();

            services.AddScoped<IUnitOfWork, UnitOfWork>();
            //services.AddScoped<IUnitOfWork<Merchant>, UnitOfWork<Merchant>>();
            //services.AddScoped<IUnitOfWork<Representative>, UnitOfWork<Representative>>();
            //services.AddScoped<IUnitOfWork<City>, UnitOfWork<City>>();
            //services.AddScoped<IUnitOfWork<Branch>, UnitOfWork<Branch>>();
            //services.AddScoped<IUnitOfWork<Governorate>, UnitOfWork<Governorate>>();
            //services.AddScoped<IUnitOfWork<GovernorateRepresentatives>, UnitOfWork<GovernorateRepresentatives>>();
            //services.AddScoped<IUnitOfWork<Order>, UnitOfWork<Order>>();
            //services.AddScoped<IUnitOfWork<Product>, UnitOfWork<Product>>();
            //services.AddScoped<IUnitOfWork<Payment>, UnitOfWork<Payment>>();
            //services.AddScoped<IUnitOfWork<RolePowers>, UnitOfWork<RolePowers>>();
            //services.AddScoped<IUnitOfWork<Shipping>, UnitOfWork<Shipping>>();

            return services;
        }
    }
}
