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
using HotelApp.Application.IRepositories;
using HotelApp.Domain.Entities;
using HotelApp.Helper;

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
            services.AddScoped<IUnitOfWork, unitOfWork>();

			services.AddScoped<IRoomRepository, RoomRepository>();
            services.AddScoped<IRoleRepository, RoleRepository>();
            services.AddScoped<IReservationRepository, ReservationRepository>();
            services.AddScoped<IGuestRepository, GuestRepository>();
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IRoomTypeRepository, RoomTypeRepository>();
            services.AddScoped<IRateRepository, RateRepository>();

			services.AddScoped<IPermissionLoader, PermissionLoader>();

            services.AddHttpContextAccessor();

            return services;
        }
    }
}
