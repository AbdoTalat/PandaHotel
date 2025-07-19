using HotelApp.Application.Authorization;
using HotelApp.Application.Services.BranchService;
using HotelApp.Application.Services.FloorService;
using HotelApp.Application.Services.GuestService;
using HotelApp.Application.Services.LocationsService;
using HotelApp.Application.Services.OptionService;
using HotelApp.Application.Services.RateService;
using HotelApp.Application.Services.ReportService;
using HotelApp.Application.Services.ReservationService;
using HotelApp.Application.Services.RoleService;
using HotelApp.Application.Services.RoomService;
using HotelApp.Application.Services.RoomStatusService;
using HotelApp.Application.Services.RoomTypeService;
using HotelApp.Application.Services.SystemSettingService;
using HotelApp.Application.Services.UserService;
using HotelApp.Domain;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HotelApp.Application
{
    public static class DependencyInjectionRegister
    {
        public static IServiceCollection AddApplicationDI(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddScoped<IRoomService, RoomService>();
            services.AddScoped<IBranchService, BranchService>();
            services.AddScoped<IRoomTypeService, RoomTypeService>();
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IRoleService, RoleService>();
            services.AddScoped<IGuestService, GuestService>();
            services.AddScoped<IReservationService, ReservationService>();
            services.AddScoped<IRoomStatusService, RoomStatusService>();
            services.AddScoped<IFloorService, FloorService>();
            services.AddScoped<IOptionService, OptionService>();
            services.AddScoped<IRateService, RateService>();
			services.AddScoped<ILocationsService, LocationsService>();
            services.AddScoped<ISystemSettingService, SystemSettingService>();
            services.AddScoped<IReportService, ReportService>();
            
            services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

			return services;
        }
    }
}
