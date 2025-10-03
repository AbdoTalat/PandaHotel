using AutoMapper;
using AutoMapper.QueryableExtensions;
using HotelApp.Application.DTOs.Reservation;
using HotelApp.Application.Interfaces.IRepositories;
using HotelApp.Domain.Entities;
using HotelApp.Domain.Enums;
using HotelApp.Helper;
using HotelApp.Infrastructure.DbContext;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace HotelApp.Infrastructure.Repositories
{
    public class ReservationRepository : GenericRepository<Reservation>, IReservationRepository
    {
        private readonly ApplicationDbContext _context;
        private readonly IConfigurationProvider _mapperConfig;

        public ReservationRepository(ApplicationDbContext context, IConfigurationProvider mapperConfig)
            : base(context, mapperConfig)
        {
            _context = context;
            _mapperConfig = mapperConfig;
        }

		public async Task<Reservation> GetReservationDetailsByIds(int Id)
		{
			var reservation = await _context.Reservations
				.BranchFilter()
				.Where(r => r.Id == Id)
				.Include(r => r.ReservationSource)
				.Include(r => r.Company)
				.Include(r => r.ReservationsRooms)
					.ThenInclude(r => r.Room)
				.Include(r => r.guestReservations)
					.ThenInclude(gr => gr.Guest)
				.Include(r => r.ReservationRoomTypes)
					.ThenInclude(rt => rt.RoomType)
				.FirstOrDefaultAsync();

			return reservation;
		}
		public async Task<IEnumerable<GetAllReservationsDTO>> GetFilteredReservationsAsync(ReservationFilterDTO dto)
		{
			var today = GetEgyptDateTime().Date;
			var tomorrow = today.AddDays(1);

			var query = _context.Reservations
				.BranchFilter()
				.AsQueryable();

			if (dto.ReservationCategory.HasValue && dto.ReservationCategory.Value != ReservationCategory.All)
			{
				switch (dto.ReservationCategory.Value)
				{
					case ReservationCategory.NewBookings:
						query = query.Where(r => r.CreatedDate.HasValue &&
												 r.CreatedDate.Value.Date == today &&
												 !r.IsCancelled);
						break;

					case ReservationCategory.Arrivals:
						query = query.Where(r => r.CheckInDate.Date == today &&
												 r.IsConfirmed &&
												 !r.IsCancelled &&
												 !r.IsCheckedOut &&
												 !r.IsCheckedIn);
						break;

					case ReservationCategory.Departures:
						query = query.Where(r => r.CheckOutDate.Date == today &&
												 !r.IsCancelled);
						break;

					case ReservationCategory.StayOvers:
						query = query.Where(r =>
							r.CheckInDate.Date <= today &&
							r.CheckOutDate.Date > today &&
							r.IsCheckedIn &&
							!r.IsCancelled &&
							!r.IsCheckedOut);
						break;

					case ReservationCategory.Cancellations:
						query = query.Where(r =>
							r.IsCancelled &&
							r.LastModifiedDate != null &&
							r.LastModifiedDate.Value.Date == today);
						break;

					case ReservationCategory.NoShow:
						query = query.Where(r =>
							!r.IsCancelled &&
							!r.IsCheckedIn &&
							r.IsConfirmed &&
							r.CheckInDate.Date >= today && r.CheckInDate.Date < tomorrow);
						break;
				}
			}

			if (dto.CheckInDate.HasValue)
				query = query.Where(r => r.CheckInDate >= dto.CheckInDate.Value);

			if (dto.CheckOutDate.HasValue)
				query = query.Where(r => r.CheckOutDate <= dto.CheckOutDate.Value);

			if (!string.IsNullOrEmpty(dto.PrimaryGuestName))
			{
				query = query.Where(r => r.guestReservations.Any(gr =>
					gr.IsPrimaryGuest &&
					gr.Guest.FullName.Contains(dto.PrimaryGuestName)));
			}

			var result = await query
				.ProjectTo<GetAllReservationsDTO>(_mapperConfig)
				.ToListAsync();

			return result;
		}
		private DateTime GetEgyptDateTime()
		{
			var egyptTimeZone = TimeZoneInfo.FindSystemTimeZoneById("Egypt Standard Time");
			var today = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, egyptTimeZone).Date;

			return today;
		}

    }


}
