using AspNetCore.Reporting;
using HotelApp.Application.Services.ReportService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HotelApp.UI.Controllers
{
	public class ReportController : Controller
	{
		private readonly IHostEnvironment _env;
		private readonly IReportService _reportService;

		public ReportController(IHostEnvironment env, IReportService reportService)
        {
			_env = env;
			_reportService = reportService;
		}

		[Authorize(Policy = "Report.View")]
		public IActionResult Index()
		{
			return View();
		}
		public IActionResult RoomsReport()
		{
			var parameters = new Dictionary<string, string>
			{
				{ "pr1", "Welcome to the Rooms Report." }
			};

			string reportPath  = Path.Combine(_env.ContentRootPath, "Reports", "RoomsReport.rdlc");


			var reportBytes = _reportService.GeneratePdfReport(reportPath, parameters);
			return File(reportBytes, "application/pdf", "RoomsReport.pdf");
		}

	}
}
