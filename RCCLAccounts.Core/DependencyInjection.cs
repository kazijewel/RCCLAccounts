using RCCLAccounts.Data;
using RCCLAccounts.Core.Interfaces;
using RCCLAccounts.Core.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Mapster;
using MapsterMapper;
using System.Reflection;
using RCCLAccounts.Data.Repositories;

namespace RCCLAccounts.Core
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddCoreLayer(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDataLayer(configuration);
            services.AddMappings();
            services.AddScoped<IPrimaryGroupService, PrimaryGroupService>();
            return services;
        }
        public static IServiceCollection AddMappings(this IServiceCollection services)
        {
            var config = TypeAdapterConfig.GlobalSettings;
            config.Scan(Assembly.GetExecutingAssembly());

            services.AddSingleton(config);
            services.AddScoped<IMapper, ServiceMapper>();
            return services;
        }
    }
}