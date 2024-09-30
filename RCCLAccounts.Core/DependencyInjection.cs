using RCCLAccounts.Data;
using RCCLAccounts.Core.Interfaces;
using RCCLAccounts.Core.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Mapster;
using MapsterMapper;
using System.Reflection;

namespace RCCLAccounts.Core
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddCoreLayer(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDataLayer(configuration);
            services.AddMappings();
            services.AddScoped<IEmployeeService, EmployeeService>();

            services.AddScoped<IEmpService, EmpService>();
            services.AddScoped<IBankNameService, BankNameService>();
            services.AddScoped<IBankBranchService, BankBranchService>();
            services.AddScoped<IBankAccountInfoService, BankAccountInfoService>();
            services.AddScoped<IBankDepositService, BankDepositService>();
            services.AddScoped<ILoanInformationService, LoanInformationService>();
            services.AddScoped<IEmployeeCpfledgerService, EmployeeCpfledgerService>();
			services.AddScoped<IEmployeeCPFOpeningService, EmployeeCPFOpeningService>();
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