using HotelApp.Application.DTOs.Dashboard;
using HotelApp.Application.IRepositories;
using HotelApp.Application.Services.DashboardService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HotelApp.UI.Controllers
{
	[Authorize]
	public class DashboardController : BaseController
	{
		private readonly IDashboardService _dashboardService;
		private readonly IDashboardRepository _dashboardRepository;

		public DashboardController(IDashboardService dashboardService, IDashboardRepository dashboardRepository)
        {
			_dashboardService = dashboardService;
			_dashboardRepository = dashboardRepository;
		}
        public IActionResult Index()
		{
			return View();
		}

		[HttpGet]
		public async Task<IActionResult> GetTodayReservations()
		{
			var data = await _dashboardService.GetTodayReservationsSummaryAsync();
			return Ok(data);
		}
        [HttpGet]
        public async Task<IActionResult> GetAvailabilityRooms(int? roomTypeId)
        {
            var data = await _dashboardService.GetAvailabilityRoomsAsync(roomTypeId);
			var result = new
			{
				roomTypeId = data.RoomTypeId,
				roomTypeName = data.RoomTypeName,
				availability = data.Availability.Select(a => new
				{
					date = a.Date.ToString("o"), // ISO string
					label = a.Label,
					availableRooms = a.AvailableRooms
				}).ToList()
			};

			return Ok(result);
		}

		[HttpGet]
		public async Task<IActionResult> GetGuestDashboardStats()
		{
			var data = await _dashboardRepository.GetGuestDashboardStatsAsync();
			return Ok(data);
		}

		[HttpGet]
		public async Task<IActionResult> GetRoomStatusDahsboard()
		{
			var data = await _dashboardRepository.GetRoomStatusDahsboardAsync();

			return Json(data);
		}

		[HttpGet]
		public async Task<IActionResult> GetRoomOccupancyDashboard()
		{
			var data = await _dashboardRepository.GetRoomOccupancyDashboardAsync();
			var result = data.Select(d => new
			{
				date = d.Date.ToString("o"),
				label = d.Label,
				occupiedRooms = d.OccupiedRooms,
				totalRooms = d.TotalRooms
			});

			return Json(result);
		}
	}
}
