using AspNetCore.Reporting;
using HotelApp.Application.Services.ReportService;
using HotelApp.Application.Services.RoomService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HotelApp.UI.Controllers
{
	public class ReportController : Controller
	{
		private readonly IHostEnvironment _env;
		private readonly IReportService _reportService;
		private readonly IRoomService _roomService;

		public ReportController(IHostEnvironment env, IReportService reportService, IRoomService roomService)
        {
			_env = env;
			_reportService = reportService;
			_roomService = roomService;
		}

		[Authorize(Policy = "Report.View")]
		public IActionResult Index()
		{
			return View();
		}

		[HttpPost]
		public async Task<IActionResult> RoomsReport(DateTime StartDate, DateTime EndDate)
		{
			var data = await _roomService.GetRoomsReportBetweenDatesAsync(StartDate, EndDate);

			var parameters = new Dictionary<string, string>
			{
				{ "StartDate", StartDate.ToString("yyyy-MM-dd") },
				{ "EndDate", EndDate.ToString("yyyy-MM-dd") },
				{ "NumOfAvailable", data.NumOfAvailable.ToString() },
				{ "NumOfMaintain", data.NumOfMaintainable.ToString() },
				{ "NumOfOccupied", data.NumOfOccupied.ToString() }
			};

			string reportPath = Path.Combine(_env.ContentRootPath, "Reports", "RoomsReport.rdlc");

			var reportBytes = _reportService.GeneratePdfReport(reportPath, parameters, data.roomsDetails, "RoomDataSet");

			return File(reportBytes, "application/pdf", "RoomsReport.pdf");
		}

	}
}
