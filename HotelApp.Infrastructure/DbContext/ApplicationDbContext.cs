using HotelApp.Domain.Entities;
using HotelApp.Infrastructure.Configurations;
using HotelApp.Infrastructure.Configurations.Locations;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using System.Reflection;
using System.Reflection.Emit;
using System.Security.Claims;
using HotelApp.Domain.Common;


namespace HotelApp.Infrastructure.DbContext
{
    public class ApplicationDbContext : IdentityDbContext<User, Role, int>
    {		
		private readonly IHttpContextAccessor? _httpContextAccessor;
		public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options, 
			IHttpContextAccessor? httpContextAccessor)
			: base(options)
		{
			_httpContextAccessor = httpContextAccessor;
		}

		#region Overrided SaveChangesAsync Method
		public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
		{
			return await SaveChangesAsync(skipAuditFields: false, cancellationToken);
		}

		public async Task<int> SaveChangesAsync(bool skipAuditFields = false, CancellationToken cancellationToken = default)
		{
			if (!skipAuditFields)
			{
				var entries = ChangeTracker
					.Entries<BaseEntity>()
					.Where(e => e.State == EntityState.Added || e.State == EntityState.Modified);

				var now = DateTime.UtcNow;

				// Get userId safely
				var userIdStr = _httpContextAccessor?.HttpContext?.User?.FindFirstValue(ClaimTypes.NameIdentifier);
				int? userId = null;
				if (int.TryParse(userIdStr, out var parsedUserId))
					userId = parsedUserId;

				foreach (var entry in entries)
				{
					if (entry.State == EntityState.Added)
					{
						entry.Entity.CreatedDate = now;
						if (userId.HasValue)
							entry.Entity.CreatedById = userId.Value;
					}
					else if (entry.State == EntityState.Modified)
					{
						entry.Entity.LastModifiedDate = now;
						if (userId.HasValue)
							entry.Entity.LastModifiedById = userId.Value;
					}
				}
			}

			return await base.SaveChangesAsync(cancellationToken);
		}
		#endregion

		#region DbSets
		public DbSet<Branch> Branches { get; set; }
		public DbSet<Room> Rooms { get; set; }
		public DbSet<RoomType> RoomTypes { get; set; }
		public DbSet<RoomStatus> RoomStatuses { get; set; }
		public DbSet<Guest> Guests { get; set; }
		public DbSet<Reservation> Reservations { get; set; }
		public DbSet<GuestReservation> guestReservations { get; set; }
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
		#endregion


		
		protected override void OnModelCreating(ModelBuilder builder)
		{
			base.OnModelCreating(builder);

			builder.Entity<RoomStatus>().HasData(
				new RoomStatus { Id = 1, Name = "Available", Description = "Room is ready to be booked", BranchId = 2, Color = "#20BF7E" },
				new RoomStatus { Id = 2, Name = "Reserved", Description = "Room is reserved by a guest", BranchId = 2, Color = "#20BF7E" },
				new RoomStatus { Id = 3, Name = "Occupied", Description = "Room is currently occupied", BranchId = 2, Color = "#20BF7E" },
				new RoomStatus { Id = 4, Name = "Maintenance", Description = "Room is under maintenance", BranchId = 2, Color = "#20BF7E" },
				new RoomStatus { Id = 5, Name = "Cleaning", Description = "Room is being cleaned", BranchId = 2, Color = "#20BF7E" }
				);

            builder.Entity<ReservationSource>().HasData(
                new ReservationSource { Id = 1, Name = "Walk In" },
                new ReservationSource { Id = 2, Name = "Hotel website" },
                new ReservationSource { Id = 3, Name = "Admin panel" },
                new ReservationSource { Id = 4, Name = "Government" },
                new ReservationSource { Id = 5, Name = "Mobile App"}
                );

            builder.Entity<User>().ToTable("Users");
			builder.Entity<Role>().ToTable("Roles");
			builder.Entity<IdentityUserRole<int>>().ToTable("UserRoles");

			//builder.Ignore<IdentityUserClaim<int>>();
			//builder.Ignore<IdentityUserLogin<int>>();
			//builder.Ignore<IdentityRoleClaim<int>>();
			//builder.Ignore<IdentityUserToken<int>>();


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
			

		}
		protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
		{
            base.OnConfiguring(optionsBuilder);
		}
	}
}
