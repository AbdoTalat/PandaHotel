using HotelApp.Application.DTOs.Dashboard;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HotelApp.Application.IRepositories
{
	public interface IDashboardRepository
	{
		Task<TodayReservationsSummaryDto> GetTodayReservationsSummaryAsync();
        Task<RoomAvailabilityDto> GetAvailabilityRoomsAsync(int? roomTypeId = null);

    }
}
