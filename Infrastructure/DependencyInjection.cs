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

namespace Infrastructure
{
    public static class DependencyInjection
    {
       
        public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<ShippingContext>(options => options.UseSqlServer(configuration.GetConnectionString("con")));
            services.AddIdentityCore<ApplicationUser>().AddRoles<ApplicationRoles>().AddEntityFrameworkStores<ShippingContext>();

            return services;
        }
    }
}
