using HotelApp.Application.DTOs.Dashboard;
using HotelApp.Application.IRepositories;
using HotelApp.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HotelApp.Application.Services.DashboardService
{
	public class DashboardService : IDashboardService
	{
		private readonly IUnitOfWork _unitOfWork;
		private readonly IDashboardRepository _dashboardRepository;

		public DashboardService(IUnitOfWork unitOfWork, IDashboardRepository dashboardRepository)
        {
			_unitOfWork = unitOfWork;
			_dashboardRepository = dashboardRepository;
		}

        public async Task<TodayReservationsSummaryDto> GetTodayReservationsSummaryAsync()
        {
			var summary = await _dashboardRepository.GetTodayReservationsSummaryAsync();
			return summary;
        }
        public async Task<RoomAvailabilityDto> GetAvailabilityRoomsAsync(int? roomTypeId)
		{
			var summary = await _dashboardRepository.GetAvailabilityRoomsAsync(roomTypeId);
			return summary;
		}

    }
}
