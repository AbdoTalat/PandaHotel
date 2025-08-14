using HotelApp.Domain.Entities;
using HotelApp.Infrastructure;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Identity;
using System.Text.Json;
using static HotelApp.UI.Controllers.AccountController;

namespace HotelApp.UI.DependencyInjectionExtentions
{
	public static class AuthenticationAndSessionExtension
	{
		public static IServiceCollection AddAuthenticationAndSessionDI(this IServiceCollection services)
		{
			services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
				.AddCookie(options =>
				{
					options.AccessDeniedPath = "/Home/NoPermission";
				});

			services.AddControllers().AddJsonOptions(options =>
			{
				options.JsonSerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
			});

			services.AddDistributedMemoryCache();

			// Claims refresh every 1 hour
			services.Configure<SecurityStampValidatorOptions>(options =>
			{
				options.ValidationInterval = TimeSpan.FromHours(1);
			});

			services.AddSession(options =>
			{
				options.IdleTimeout = TimeSpan.FromHours(1);
				options.Cookie.HttpOnly = true;
				options.Cookie.IsEssential = true;
			});

			services.ConfigureApplicationCookie(options =>
			{
				options.LoginPath = "/Account/Login";
				options.AccessDeniedPath = "/Account/AccessDenied";

				// Absolute expiration for RememberMe
				options.ExpireTimeSpan = TimeSpan.FromDays(30);
				options.SlidingExpiration = true;

				// Security
				options.Cookie.HttpOnly = true;
				options.Cookie.SecurePolicy = CookieSecurePolicy.Always; // Only over HTTPS
			});

			services.AddScoped<IUserClaimsPrincipalFactory<User>, CustomClaimsPrincipalFactory>();

			return services;
		}
	}

}
