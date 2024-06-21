using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrasc(this IServiceCollection services)
        {
            return services;
        }
    }
}
