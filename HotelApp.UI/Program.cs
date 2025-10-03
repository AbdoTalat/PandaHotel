using HotelApp.Infrastructure;
using HotelApp.Helper;
using System;
using HotelApp.Infrastructure.Seed;
using HotelApp.Infrastructure.Repositories;
using HotelApp.Application;
using HotelApp.Application.Validators;
using HotelApp.UI.Controllers;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc;

namespace HotelApp.PL
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddControllersWithViews();


			/* DI */
			builder.Services.AddInfrastructureDI(builder.Configuration);
			builder.Services.AddApplicationDI(builder.Configuration);
			builder.Services.AddAuthenticationAndSessionDI();
			builder.Services.AddAuthorizationPoliciesDI();

			var app = builder.Build();


			using (var scope = app.Services.CreateScope())
			{
				var serviceProvider = scope.ServiceProvider;
				await AdminSeeder.SeedAsync(scope.ServiceProvider);
				await DefaultPermissionsSeeder.SeedAsync(serviceProvider);
			}

			// Global error handling for production
			if (!app.Environment.IsDevelopment())
			{
				app.UseExceptionHandler("/Home/Error");
			}
			

			var httpContextAccessor = app.Services.GetRequiredService<IHttpContextAccessor>();
			BranchContext.Configure(httpContextAccessor);

			app.UseStaticFiles();

			app.UseRouting();
			app.UseSession(); 

			app.UseAuthentication();
			app.UseAuthorization();


			app.UseStatusCodePagesWithRedirects("/Home/ErrorPage?statusCode={0}");

			app.MapControllerRoute(
				name: "default",
				pattern: "{controller=Dashboard}/{action=Index}/{id?}");

			app.Run();

		}
	}
}
