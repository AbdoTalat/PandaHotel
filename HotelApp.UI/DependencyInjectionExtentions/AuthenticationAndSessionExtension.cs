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

            services.AddDistributedMemoryCache(); // Required for session

            services.Configure<SecurityStampValidatorOptions>(options =>
            {
                options.ValidationInterval = TimeSpan.FromHours(1); // prevent reloading user/claims on every request
            });

            services.AddDistributedMemoryCache();
            
            services.AddSession(options =>
            {
                options.IdleTimeout = TimeSpan.FromHours(1); 
                options.Cookie.HttpOnly = true;
                options.Cookie.IsEssential = true;
            });

            services.ConfigureApplicationCookie(options =>
            {
                options.ExpireTimeSpan = TimeSpan.FromHours(1);
                options.SlidingExpiration = true; 
                options.LoginPath = "/Account/Login";
            });

			services.AddScoped<IUserClaimsPrincipalFactory<User>, CustomClaimsPrincipalFactory>();


			return services;
        }
    }
}
