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

            services.AddScoped<IEmployeeRepository, EmployeeRepository>();
            services.AddScoped<IEmpRepository, EmpRepository>();
            services.AddScoped<IBankNameRepository, BankNameRepository>();
            services.AddScoped<IBankBranchRepository, BankBranchRepository>();
            services.AddScoped<IBankAccountInfoRepository, BankAccountInfoRepository>();
            services.AddScoped<IBankDepositRepository, BankDepositRepository>();
            services.AddScoped<ILoanInformationRepository, LoanInformationRepository>();
            services.AddScoped<IEmployeeCpfledgerRepository, EmployeeCpfledgerRepository>();
			services.AddScoped<IEmployeeCPFOpeningRepository, EmployeeCPFOpeningRepository>();
			return services;
        }
    }
}
