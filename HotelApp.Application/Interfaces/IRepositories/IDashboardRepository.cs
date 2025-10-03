using HotelApp.Application.DTOs.Dashboard;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HotelApp.Application.Interfaces.IRepositories
{
    public interface IDashboardRepository
    {
        Task<TodayReservationsSummaryDto> GetTodayReservationsSummaryAsync();
        Task<RoomAvailabilityDto> GetAvailabilityRoomsAsync(int? roomTypeId = null);
        Task<GuestDashboardDTO> GetGuestDashboardStatsAsync();
        Task<RoomStatusDashboardDTO> GetRoomStatusDahsboardAsync();
        Task<List<RoomOccupancyDashboardDTO>> GetRoomOccupancyDashboardAsync();

    }
}
