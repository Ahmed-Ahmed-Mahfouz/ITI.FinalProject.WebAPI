<<<<<<< Updated upstream
﻿using Application.DTOs.DisplayDTOs;
using Application.DTOs.InsertDTOs;
using Application.DTOs.UpdateDTOs;
using Application.Interfaces.ApplicationServices;
using Application.Services;
using Domain.Entities;
=======
﻿using Application.Interfaces;
using Application.Services;
>>>>>>> Stashed changes
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
<<<<<<< Updated upstream
            services.AddScoped<IGenericService<Governorate, GovernorateDTO, GovernorateInsertDTO, GovernorateUpdateDTO>, GovernorateService>();

=======
            services.AddAutoMapper(typeof(Mappings.MappingProfile));
            services.AddScoped<IOrderService, OrderService>();
>>>>>>> Stashed changes
            return services;
        }
    }
}
