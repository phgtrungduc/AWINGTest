using AWP.Repository.Implements;
using AWP.Repository.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AWP.Repository.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddRepositories(this IServiceCollection services)
        {
            services.AddScoped<IPuzzleMapRepository, PuzzleMapRepository>();
            // Add other repositories here as needed

            return services;
        }
    }
}
