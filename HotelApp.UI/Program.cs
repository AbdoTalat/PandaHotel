using HotelApp.Infrastructure;
using HotelApp.Infrastructure.DbContext;
using HotelApp.UI;
using HotelApp.Helper;
using System;
using static HotelApp.Infrastructure.DbContext.ApplicationDbContext;
//using HotelApp.UI.Helper;
using HotelApp.Infrastructure.Seed;
using HotelApp.UI.DependencyInjectionExtentions;
using HotelApp.Infrastructure.Repositories;
using HotelApp.Application;

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
			builder.Services.AddAuthorizationPoliciesDIAsync(new PermissionLoader(builder.Environment));



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
				pattern: "{controller=Home}/{action=Index}/{id?}");

			app.Run();

		}
	}
}
