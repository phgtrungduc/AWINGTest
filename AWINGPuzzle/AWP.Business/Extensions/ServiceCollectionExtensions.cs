using AWP.Business.BusinessLayer;
using AWP.Business.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AWP.Business.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddBusinessServices(this IServiceCollection services)
        {
            services.AddScoped<IBusinessPuzzleMap, BusinessPuzzleMap>();
            // Add other business services here as needed

            return services;
        }
    }
}
