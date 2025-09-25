using HotelApp.Application.DTOs.ReservationSource;
using HotelApp.Application.Services.ReservationSourceService;
using HotelApp.Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HotelApp.UI.Controllers
{
	public class ReservationSourceController : Controller
	{
		private readonly IReservationSourceService _reservationSourceService;

		public ReservationSourceController(IReservationSourceService reservationSourceService)
        {
			_reservationSourceService = reservationSourceService;
		}
        [HttpGet]
		[Authorize(Policy = "ReservationSource.View")]
		public async Task<IActionResult> Index()
		{
			var model = await _reservationSourceService.GetAllReservationSourcesAsync();
			return View(model);
		}

		[HttpGet]
		[Authorize(Policy = "ReservationSource.Add")]
		public IActionResult AddReservationSource()
		{
			return PartialView("_AddReservationSource");
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		[Authorize(Policy = "ReservationSource.Add")]
		public async Task<IActionResult> AddReservationSource(ReservationSourceDTO model)
		{
			if (!ModelState.IsValid)
			{
				return PartialView("_AddReservationSource");
			}

			var result = await _reservationSourceService.AddReservationSourceAsync(model);

			return Json(new { success = result.Success, message = result.Message });
		}

		[HttpGet]
		[Authorize(Policy = "ReservationSource.Edit")]
		public async Task<IActionResult> EditReservationSource(int Id)
		{
			var model = await _reservationSourceService.GetReservationSourceToEditByIdAsync(Id);

			return PartialView("_EditReservationSource", model);
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		[Authorize(Policy = "ReservationSource.Edit")]
		public async Task<IActionResult> EditReservationSource(ReservationSourceDTO model)
		{
			if (!ModelState.IsValid)
			{
				return PartialView("_EditReservationSource", model);
			}
			var result = await _reservationSourceService.EditReservationSourceAsync(model);

			return Json(new { success = result.Success, message = result.Message });
		}

		[HttpDelete]
		[Authorize(Policy = "ReservationSource.Delete")]
		public async Task<IActionResult> DeleteReservationSource(int Id)
		{
			var result = await _reservationSourceService.DeleteReservationSourceAsync(Id);

			return Json(new { success = result.Success, message = result.Message });
		}


		[HttpGet]
		public async Task<IActionResult> GetReservationSourcesDropDown()
		{
			var result = await _reservationSourceService.GetReservationSourcesDropDownAsync();

			return Json(result);
		}
	}
}
