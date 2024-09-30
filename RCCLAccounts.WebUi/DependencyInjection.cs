using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.Extensions.DependencyInjection;
using RCCLAccounts.Core;

namespace RCCLAccounts.WebUi
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddPresentationLayer(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddCoreLayer(configuration);
            services.AddControllersWithViews();
            return services;
        }
    }
}
