using HotelApp.Application.DTOs.Dashboard;
using HotelApp.Application.Interfaces.IRepositories;
using HotelApp.Domain;
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
    public class DashboardRepository : IDashboardRepository
	{
		private readonly ApplicationDbContext _context;

		public DashboardRepository(ApplicationDbContext context)
        {
			_context = context;
		}
        public async Task<TodayReservationsSummaryDto> GetTodayReservationsSummaryAsync()
        {
            var today = GetEgyptDateTime().Date;
            var tomorrow = today.AddDays(1);

            var query = _context.Reservations
                .AsNoTracking()
                .BranchFilter()
                .Where(r =>
                    (r.CheckInDate >= today && r.CheckInDate < tomorrow) ||
                    (r.CheckOutDate >= today && r.CheckOutDate < tomorrow) ||
                    (r.CreatedDate >= today && r.CreatedDate < tomorrow) ||
                    (r.LastModifiedDate >= today && r.LastModifiedDate < tomorrow) ||
                    (r.Status == ReservationStatus.CheckedIn && r.CheckInDate < today && r.CheckOutDate > today)
                );

            var summary = await query
                .GroupBy(_ => 1)
                .Select(g => new TodayReservationsSummaryDto
                {
                    Arrivals = g.Count(r =>
                        r.Status == ReservationStatus.Confirmed &&
                        r.CheckInDate >= today && r.CheckInDate < tomorrow),

                    Departures = g.Count(r =>
                        r.Status != ReservationStatus.Cancelled &&
                        r.CheckOutDate >= today && r.CheckOutDate < tomorrow),

                    NewBookings = g.Count(r =>
                        r.Status != ReservationStatus.Cancelled &&
                        r.CreatedDate >= today && r.CreatedDate < tomorrow),

                    StayOvers = g.Count(r =>
                        r.Status == ReservationStatus.CheckedIn &&
                        r.CheckInDate <= today && r.CheckOutDate > today),

                    Cancellations = g.Count(r =>
                        r.Status == ReservationStatus.Cancelled &&
                        r.LastModifiedDate >= today && r.LastModifiedDate < tomorrow),

                    NoShow = g.Count(r =>
                        r.Status == ReservationStatus.NoShow &&
                        r.CheckInDate >= today && r.CheckInDate < tomorrow)
                })
                .FirstOrDefaultAsync();

            return summary ?? new TodayReservationsSummaryDto();
        }


        public async Task<RoomAvailabilityDto> GetAvailabilityRoomsAsync(int? roomTypeId)
		{
			var today = GetEgyptDateTime();
			var next7Days = Enumerable.Range(0, 7).Select(i => today.AddDays(i)).ToList();
			var windowEnd = today.AddDays(7); 

			var roomsQuery = _context.Rooms
				.BranchFilter()            
				.Where(r => r.IsActive);

			if (roomTypeId.HasValue && roomTypeId.Value > 0)
				roomsQuery = roomsQuery.Where(r => r.RoomTypeId == roomTypeId.Value);

			var roomIds = await roomsQuery.Select(r => r.Id).ToListAsync();
			var totalRooms = roomIds.Count;

			// 2) Get reservation-room links that overlap the window [today, windowEnd)
			//    and belong to the room IDs above. We only fetch necessary fields.
			var overlappingReservations = await _context.ReservationRooms
				.AsNoTracking()
				.Where(rr =>
					roomIds.Contains(rr.RoomId) &&
					rr.Reservation.Status != ReservationStatus.Cancelled &&
					rr.Reservation.CheckInDate < windowEnd &&   // overlap condition
					rr.Reservation.CheckOutDate > today)
				.Select(rr => new
				{
					rr.Reservation.CheckInDate,
					rr.Reservation.CheckOutDate
				})
				.ToListAsync();

			// 3) Compute reserved count per day in-memory (cheap: only overlapping records within 7-day window)
			var availability = new List<AvailabilityDayDto>();
			foreach (var day in next7Days)
			{
				var dayStart = day;
				var dayEnd = day.AddDays(1);

				var reservedCount = overlappingReservations
					.Count(r => r.CheckInDate < dayEnd && r.CheckOutDate > dayStart);

				var available = Math.Max(0, totalRooms - reservedCount);

				availability.Add(new AvailabilityDayDto
				{
					Date = day,
					Label = day == today ? "Today" : day.ToString("ddd dd MMM"),
					AvailableRooms = available
				});
			}

			// 4) Room type name resolution
			string roomTypeName = roomTypeId.HasValue && roomTypeId.Value > 0
				? await _context.RoomTypes
					.Where(rt => rt.Id == roomTypeId.Value)
					.Select(rt => rt.Name)
					.FirstOrDefaultAsync() ?? "Unknown"
				: "All Room Types";

			return new RoomAvailabilityDto
			{
				RoomTypeId = roomTypeId ?? 0,
				RoomTypeName = roomTypeName,
				Availability = availability
			};
		}

		public async Task<GuestDashboardDTO> GetGuestDashboardStatsAsync()
		{
			var today = GetEgyptDateTime();

			var reservationsQuery = _context.Reservations
				.AsNoTracking()
				.BranchFilter()
				.Where(r => r.Status != ReservationStatus.Cancelled);

			var reservations = await reservationsQuery
				.Select(r => new
				{
					r.Id,
					r.Status,
					r.CheckInDate,
					r.CheckOutDate,
					GuestIDs = r.guestReservations.Select(gr => gr.GuestId)
				})
				.ToListAsync();


            var checkedInGuests = reservations
				.Where(r => r.Status == ReservationStatus.CheckedIn
						 || (r.CheckInDate <= today && r.CheckOutDate >= today && r.Status != ReservationStatus.CheckedOut))
				.SelectMany(r => r.GuestIDs)
				.Distinct()
				.Count();

            var todayArrivals = reservations
                .Where(r => r.CheckInDate.Date == today && r.Status == ReservationStatus.Confirmed)
                .SelectMany(r => r.GuestIDs)
                .Distinct()
                .Count();

            var todayDepartures = reservations
                .Where(r => r.CheckOutDate.Date == today && r.Status == ReservationStatus.CheckedIn)
                .SelectMany(r => r.GuestIDs)
                .Distinct()
                .Count();

            var totalGuests = await _context.Guests
				.AsNoTracking()
				.BranchFilter()
				.CountAsync();

			var returningGuestsCount = reservations
				.SelectMany(r => r.GuestIDs)
				.GroupBy(g => g)
				.Count(g => g.Count() > 1);

			double returningPercentage = 0;
			if (totalGuests > 0)
				returningPercentage = Math.Round((double)returningGuestsCount / totalGuests * 100, 2);

			return new GuestDashboardDTO
			{
				CurrentCheckedIn = checkedInGuests,
				TodayArrivals = todayArrivals,
				TodayDepartures = todayDepartures,
				ReturningGuestsPercentage = returningPercentage,
			};
		}

		public async Task<RoomStatusDashboardDTO> GetRoomStatusDahsboardAsync()
		{
			var query = _context.Rooms
				.AsNoTracking()
				.BranchFilter();

			var grouped = await query
				.GroupBy(r => r.RoomStatus.Code)
				.Select(g => new { Status = g.Key, Count = g.Count() })
				.ToListAsync();

			var total = grouped.Sum(x => x.Count);
			var available = grouped.FirstOrDefault(x => x.Status == RoomStatusEnum.Available)?.Count ?? 0;
			var occupied = grouped.FirstOrDefault(x => x.Status == RoomStatusEnum.Occupied)?.Count ?? 0;
			var outOfService = grouped.FirstOrDefault(x => x.Status == RoomStatusEnum.Maintenance)?.Count ?? 0;

			return new RoomStatusDashboardDTO
			{
				Total = total,
				Available = available,
				Occupied = occupied,
				OutOfService = outOfService
			};
		}
        public async Task<List<RoomOccupancyDashboardDTO>> GetRoomOccupancyDashboardAsync()
        {
            var today = GetEgyptDateTime().Date;
            var next7Days = Enumerable.Range(0, 7).Select(d => today.AddDays(d)).ToList();

            var totalRooms = await _context.Rooms
                .AsNoTracking()
                .BranchFilter()
                .CountAsync();

            var activeReservations = await _context.Reservations
                .AsNoTracking()
                .BranchFilter()
                .Include(r => r.ReservationsRooms)
                .Where(r => r.Status != ReservationStatus.Cancelled)
                .SelectMany(r => r.ReservationsRooms)
                .Where(rr => rr.StartDate <= next7Days.Last() && rr.EndDate > today)
                .Select(rr => new
                {
                    rr.RoomId,
                    rr.StartDate,
                    rr.EndDate
                })
                .ToListAsync();

            var result = next7Days.Select(date => new RoomOccupancyDashboardDTO
            {
                Date = date,
                Label = date.ToString("ddd dd MMM"),
                OccupiedRooms = activeReservations
                    .Where(rr => rr.StartDate <= date && rr.EndDate > date)
                    .Select(rr => rr.RoomId)
                    .Distinct()
                    .Count(),
                TotalRooms = totalRooms
            }).ToList();

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
