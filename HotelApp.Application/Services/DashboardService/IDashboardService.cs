using HotelApp.Application.DTOs.Dashboard;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HotelApp.Application.Services.DashboardService
{
	public interface IDashboardService
	{
		Task<TodayReservationsSummaryDto> GetTodayReservationsSummaryAsync();
		Task<RoomAvailabilityDto> GetAvailabilityRoomsAsync(int? roomTypeId);

    }
}
