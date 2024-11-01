using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using RCCLAccounts.Data.Interfaces;
using RCCLAccounts.Data.Repositories;


namespace RCCLAccounts.Data
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddDataLayer(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<AppDbContext>(options =>
            {
                options.UseSqlServer(configuration.GetConnectionString("DefaultConnection"));
            });

            services.AddScoped<IPrimaryGroupRepository, PrimaryGroupRepository>();
            return services;
        }
    }
}
