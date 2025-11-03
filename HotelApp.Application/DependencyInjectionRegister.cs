using FluentValidation;
using HotelApp.Application.Authorization;
using HotelApp.Application.Services.BranchService;
using HotelApp.Application.Services.CompanyService;
using HotelApp.Application.Services.CurrentUserService;
using HotelApp.Application.Services.FloorService;
using HotelApp.Application.Services.GuestService;
using HotelApp.Application.Services.LocationsService;
using HotelApp.Application.Services.OptionService;
using HotelApp.Application.Services.ProofTypeService;
using HotelApp.Application.Services.RateService;
using HotelApp.Application.Services.ReportService;
using HotelApp.Application.Services.ReservationService;
using HotelApp.Application.Services.ReservationSourceService;
using HotelApp.Application.Services.RoleService;
using HotelApp.Application.Services.RoomService;
using HotelApp.Application.Services.RoomStatusService;
using HotelApp.Application.Services.RoomTypeService;
using HotelApp.Application.Services.SystemSettingService;
using HotelApp.Application.Services.UserService;
using HotelApp.Application.Validators;
using HotelApp.Domain;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using FluentValidation.AspNetCore;
using HotelApp.Application.Services.CalculationTypeService;
using HotelApp.Application.Services.DashboardService;
using HotelApp.Application.Services.RateCalculationService;
using HotelApp.Application.DTOs.Reservation;
using HotelApp.Application.Services.DropdownService;
using HotelApp.Application.Validators.Reservations;

namespace HotelApp.Application
{
    public static class DependencyInjectionRegister
    {
        public static IServiceCollection AddApplicationDI(this IServiceCollection services, IConfiguration configuration)
        {
            #region Services
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
            services.AddScoped<IReservationSourceService, ReservationSourceService>();
            services.AddScoped<ICompanyService, CompanyService>();
            services.AddScoped<IProofTypeService, ProofTypeService>();
            services.AddScoped<ICurrentUserService, CurrentUserService>();
            services.AddScoped<ICalculationTypeService, CalculationTypeService>();
            services.AddScoped<IDashboardService, DashboardService>();
            services.AddScoped<IRateCalculationService, RateCalculationService>();
            services.AddScoped<IDropdownService, DropdownService>();
            #endregion

            services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());


            services.AddControllersWithViews()
	        .AddFluentValidation(fv =>
	        {
		        fv.RegisterValidatorsFromAssemblyContaining<GuestValidator>();
                fv.RegisterValidatorsFromAssemblyContaining<ReservationDTOValidator>();
            });

			services.AddFluentValidationAutoValidation();
			services.AddFluentValidationClientsideAdapters();

			return services;
        }
    }
}
