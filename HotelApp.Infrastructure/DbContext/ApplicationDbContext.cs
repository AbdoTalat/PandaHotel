using HotelApp.Domain.Entities;
using HotelApp.Infrastructure.Configurations;
using HotelApp.Infrastructure.Configurations.Locations;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using HotelApp.Infrastructure.Seed;

namespace HotelApp.Infrastructure.DbContext
{
    public class ApplicationDbContext : IdentityDbContext<User, Role, int>
    {
		public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
			: base(options)
		{
		}

		#region DbSets
		public DbSet<Branch> Branches { get; set; }
		public DbSet<Room> Rooms { get; set; }
		public DbSet<RoomType> RoomTypes { get; set; }
		public DbSet<RoomStatus> RoomStatuses { get; set; }
		public DbSet<Guest> Guests { get; set; }
		public DbSet<Reservation> Reservations { get; set; }
		public DbSet<GuestReservation> GuestReservations { get; set; }
		public DbSet<UserBranch> UserBranches { get; set; }
		public DbSet<Floor> Floors { get; set; }
		public DbSet<Option> Options { get; set; }
		public DbSet<RoomOption> RoomOptions { get; set; }
		public DbSet<Rate> Rates { get; set; }
		public DbSet<RoomTypeRate> RoomTypeRates { get; set; }
		public DbSet<ReservationSource> ReservationSources { get; set; }
		public DbSet<Country> Countries { get; set; }
		public DbSet<State> States { get; set; }
		public DbSet<ReservationRoomType> ReservationRoomTypes { get; set; }
		public DbSet<SystemSetting> SystemSettings { get; set; }
		public DbSet<Company> Companies { get; set; }
		public DbSet<ProofType> ProofTypes { get; set; }
		public DbSet<ReservationRoom> ReservationRooms { get; set; }
		public DbSet<CalculationType> CalculationTypes { get; set; }
		public DbSet<ReservationHistory> ReservationHistory { get; set; }
        #endregion


        protected override void OnModelCreating(ModelBuilder builder)
		{
			base.OnModelCreating(builder);

			builder.Entity<User>().ToTable("Users");
			builder.Entity<Role>().ToTable("Roles");
			builder.Entity<IdentityUserRole<int>>().ToTable("UserRoles");

			#region Configurations
			builder.ApplyConfiguration(new UserConfiguration());
			builder.ApplyConfiguration(new RoleConfiguration());

			builder.ApplyConfiguration(new BranchConfiguration());
			builder.ApplyConfiguration(new RoomConfiguration());
			builder.ApplyConfiguration(new RoomTypeConfiguration());
			builder.ApplyConfiguration(new GuestConfiguration());
			builder.ApplyConfiguration(new ReservationConfiguration());
			builder.ApplyConfiguration(new GuestReservationConfiguration());
			builder.ApplyConfiguration(new RoomStatusConfiguration());
			builder.ApplyConfiguration(new UserBranchConfiguration());
			builder.ApplyConfiguration(new FloorConfiguration());
			builder.ApplyConfiguration(new OptionConfiguration());
			builder.ApplyConfiguration(new RoomOptionConfiguration());
			builder.ApplyConfiguration(new RateConfiguration());
			builder.ApplyConfiguration(new RoomTypeRateConfiguration());
			builder.ApplyConfiguration(new ReservationSourceConfiguration());

			builder.ApplyConfiguration(new CountryConfiguration());
			builder.ApplyConfiguration(new StateConfiguration());
			builder.ApplyConfiguration(new ReservationRoomTypeConfiguration());
			builder.ApplyConfiguration(new SystemSettingConfiguration());
			builder.ApplyConfiguration(new CompanyConfiguration());
			builder.ApplyConfiguration(new ProofTypeConfiguration());
			builder.ApplyConfiguration(new ReservationRoomConfiguration());

			builder.ApplyConfiguration(new CalculationTypeConfiguration());
			builder.ApplyConfiguration(new ReservationHistoryConfiguration());
            #endregion

            #region Seed Data
            builder.SeedRoomStatuses();
            builder.SeedReservationSources();
            #endregion
        }
	}
}
