using HotelApp.Domain;
using HotelApp.Infrastructure.DbContext;
using HotelApp.Infrastructure.Repositories;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.EntityFrameworkCore;
using HotelApp.Domain.Entities;
using HotelApp.Helper;
using HotelApp.Application.Interfaces.IRepositories;
using HotelApp.Application.Interfaces;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using System.Text.Json;
using HotelApp.Application.Authorization;
using Microsoft.AspNetCore.Authorization;
using HotelApp.Infrastructure.UnitOfWorks;

namespace HotelApp.Infrastructure
{
    public static class DependencyInjectionRegister
    {
        public static IServiceCollection AddInfrastructureDI(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<ApplicationDbContext>(options =>
            {
                options.UseSqlServer(configuration.GetConnectionString("DefaultConnString"));
            });

            services.AddIdentity<User, Role>()
                .AddEntityFrameworkStores<ApplicationDbContext>();



			services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
            services.AddScoped<IUnitOfWork, UnitOfWork>();

			services.AddScoped<IRoomRepository, RoomRepository>();
            services.AddScoped<IRoleRepository, RoleRepository>();
            services.AddScoped<IReservationRepository, ReservationRepository>();
            services.AddScoped<IGuestRepository, GuestRepository>();
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IRoomTypeRepository, RoomTypeRepository>();
            services.AddScoped<IRateRepository, RateRepository>();
            services.AddScoped<ISystemSettingRepositroy, SystemSettingRepositroy>();
            services.AddScoped<IDashboardRepository, DashboardRepository>();

            services.AddHttpContextAccessor();

            return services;
        }
        public static IServiceCollection AddAuthenticationAndSessionDI(this IServiceCollection services)
        {
            services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
                .AddCookie();

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

                options.ExpireTimeSpan = TimeSpan.FromDays(30);
                options.SlidingExpiration = true;

                // Security
                options.Cookie.HttpOnly = true;
                options.Cookie.SecurePolicy = CookieSecurePolicy.Always; // Only over HTTPS
            });

            services.AddScoped<IUserClaimsPrincipalFactory<User>, CustomClaimsPrincipalFactory>();

            return services;
        }
        public static IServiceCollection AddAuthorizationPoliciesDI(this IServiceCollection services)
        {
            services.AddScoped<IAuthorizationHandler, PermissionHandler>();
            services.AddScoped<IPermissionLoader, PermissionLoader>();

            services.AddAuthorization(async options =>
            {
                using var serviceProvider = services.BuildServiceProvider();
                var permissionLoader = serviceProvider.GetRequiredService<IPermissionLoader>();

                var allPermissions = await permissionLoader.LoadAllPermissions();

                foreach (var permission in allPermissions)
                {
                    options.AddPolicy(permission!, policy =>
                        policy.Requirements.Add(new PermissionRequirement(permission!)));
                }
            });

            return services;
        }


    }
}
