using HotelApp.Application.DTOs.Dashboard;
using HotelApp.Application.IRepositories;
using HotelApp.Application.Services.DashboardService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HotelApp.UI.Controllers
{
	[Authorize]
	public class DashboardController : Controller
	{
		private readonly IDashboardService _dashboardService;

		public DashboardController(IDashboardService dashboardService)
        {
			_dashboardService = dashboardService;
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
    }
}
