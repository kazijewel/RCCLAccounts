using RCCLAccounts.WebUi;
using Microsoft.AspNetCore.Identity;
using RCCLAccounts.Data;
using RCCLAccounts.Data.Entities;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Serilog;

var builder = WebApplication.CreateBuilder(args);


var logger = new LoggerConfiguration()
    .ReadFrom.Configuration(new ConfigurationBuilder()
    .AddJsonFile("seri-log.config.json")
    .Build())
    .Enrich.FromLogContext()
    .CreateLogger();
builder.Logging.ClearProviders();
builder.Logging.AddSerilog(logger);

builder.Services.AddSession();

// Add services to the container.
{
    builder.Services.PostConfigure<CookieAuthenticationOptions>(IdentityConstants.ApplicationScheme, options =>
		{
			//options.Cookie.ExpireTimeSpan = TimeSpan.FromMinutes(20);
			options.LoginPath = "/Authentication/Login";
			options.LogoutPath = "/Authentication/Logout";
			options.AccessDeniedPath = "/Authentication/AccessDenied";
			options.SlidingExpiration = true;
			options.ExpireTimeSpan = TimeSpan.FromMinutes(20);
		});

	builder.Services
	.AddPresentationLayer(builder.Configuration)
	.AddIdentity<ApplicationUser, ApplicationRole>()
	.AddEntityFrameworkStores<AppDbContext>()
	.AddUserStore<UserStore<ApplicationUser, ApplicationRole, AppDbContext, Guid>>()
	.AddRoleStore<RoleStore<ApplicationRole, AppDbContext, Guid>>()
	.AddDefaultUI()
	.AddDefaultTokenProviders();
	           

}
var app = builder.Build();


// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
	app.UseExceptionHandler("/Home/Error");
	// The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
	app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();


app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();
app.UseSession();
app.MapControllers();


app.UseFastReport();


app.MapControllerRoute(
	name: "default",
	pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
