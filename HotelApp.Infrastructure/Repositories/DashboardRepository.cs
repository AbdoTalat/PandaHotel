using HotelApp.Application.DTOs.Dashboard;
using HotelApp.Application.IRepositories;
using HotelApp.Domain.Entities;
using HotelApp.Helper;
using HotelApp.Infrastructure.DbContext;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
			var today = DateTime.UtcNow.Date;

			var Query = _context.Reservations
				.AsNoTracking()
				.BranchFilter()
				.AsQueryable();

			var summary = new TodayReservationsSummaryDto
			{
				// Guests expected to check-in today 
				Arrivals = await Query
					.Where(r => r.CheckInDate.Date == today && r.IsConfirmed && !r.IsCancelled)
					.CountAsync(),

				// Guests expected to check-out today
				Departures = await Query.Where(r => r.CheckOutDate.Date == today && !r.IsCancelled)
					.CountAsync(),

				NewBookings = await Query.Where(r => r.CreatedDate.Value.Date == today && !r.IsCancelled)
					.CountAsync(),

				// Stay overs: already checked-in before today & not checking out today
				StayOvers = await Query.Where(r => 
					r.CheckInDate.Date < today && 
					r.CheckOutDate.Date > today &&
					r.IsCheckedIn && !r.IsCancelled)
					.CountAsync(),

				// Cancelled reservations today
				Cancellations = await Query.Where(r => 
					r.IsCancelled &&
				    r.LastModifiedDate != null &&
					r.LastModifiedDate.Value.Date == today)
					.CountAsync()
			};

			return summary;
		}

		//public async Task<TodayReservationsSummaryDto> GetTodayReservationsSummaryAsync()
		//{
		//    // NOTE: decide which time basis to use (UTC vs branch local). 
		//    // Here I keep your original UTC choice but you should prefer branch-local date.
		//    var today = DateTime.UtcNow.Date;
		//    var tomorrow = today.AddDays(1);

		//    // Base query (apply BranchFilter extension which should return IQueryable)
		//    var q = _context.Reservations
		//        .BranchFilter()        // make sure this just adds a WHERE/Filter and returns IQueryable
		//        .AsNoTracking();       // read-only, avoid change tracking overhead

		//    // Single grouped aggregation -> translates to one SQL query with SUM(CASE WHEN ...)
		//    var aggregated = await q
		//        .GroupBy(r => 1)
		//        .Select(g => new TodayReservationsSummaryDto
		//        {
		//            Arrivals = g.Sum(r =>
		//                (r.IsConfirmed && !r.IsCancelled
		//                 && r.CheckInDate >= today && r.CheckInDate < tomorrow) ? 1 : 0),

		//            Departures = g.Sum(r =>
		//                (!r.IsCancelled
		//                 && r.CheckOutDate >= today && r.CheckOutDate < tomorrow) ? 1 : 0),

		//            NewBookings = g.Sum(r =>
		//                (!r.IsCancelled
		//                 && r.CreatedDate >= today && r.CreatedDate < tomorrow) ? 1 : 0),

		//            StayOvers = g.Sum(r =>
		//                (r.IsCheckedIn && !r.IsCancelled
		//                 && r.CheckInDate < today && r.CheckOutDate > today) ? 1 : 0),

		//            Cancellations = g.Sum(r =>
		//                (r.IsCancelled
		//                 && r.LastModifiedDate != null
		//                 && r.LastModifiedDate >= today
		//                 && r.LastModifiedDate < tomorrow) ? 1 : 0)
		//        })
		//        .FirstOrDefaultAsync();

		//    return aggregated ?? new TodayReservationsSummaryDto();
		//}

		public async Task<RoomAvailabilityDto> GetAvailabilityRoomsAsync(int? roomTypeId)
		{
			var today = DateTime.UtcNow.Date;
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
					!rr.Reservation.IsCancelled &&
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
	}
}
