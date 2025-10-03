using Microsoft.AspNetCore.Mvc;

namespace HotelApp.UI.Controllers
{
	public class ErrorController : Controller
	{
		[Route("AccessDenied")]
		public IActionResult AccessDenied()
		{
			TempData["AlertType"] = "error";
			TempData["AlertTitle"] = "Access Denied";
			TempData["AlertText"] = "You do not have permission to access this page.";

			// Return empty result so the modal will show on the current page
			return new EmptyResult();
		}

		[Route("NotFoundPage")]
		public IActionResult NotFoundPage()
		{
			TempData["AlertType"] = "warning";
			TempData["AlertTitle"] = "Page Not Found";
			TempData["AlertText"] = "The page you are trying to access does not exist.";

			// Stay on the current page
			return new EmptyResult();
		}
	}

}
