using HotelApp.Application;
using HotelApp.Application.DTOs.Guests;
using HotelApp.Application.DTOs.Reservation;
using HotelApp.Application.Services.GuestService;
using HotelApp.Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;

namespace HotelApp.UI.Controllers
{
	public class GuestController : BaseController
	{
		private readonly IGuestService _guestService;

		public GuestController(IGuestService guestService)
        {
			_guestService = guestService;
		}

		[HttpGet]
        [Authorize(Policy = "Guest.View")]
        public async Task<IActionResult> Index()
		{
			var guests = await _guestService.GetAllGuestsAsync();
			return View(guests);
		}

		[HttpGet]
        [Authorize(Policy = "Guest.Add")]
        public IActionResult AddGuest()
		{
			return PartialView("_AddGuest");
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
        [Authorize(Policy = "Guest.Add")]
        public async Task<IActionResult> AddGuest(AddGuestDTO guestDTO)
		{
			if(!ModelState.IsValid)
			{
				return PartialView("_AddGuest", guestDTO);
			}
			var result = await _guestService.AddGuestAsync(guestDTO);
			return Json(new {success = result.Success, message = result.Message});
		}

		[HttpGet]
        [Authorize(Policy = "Guest.Edit")]
        public async Task<IActionResult> EditGuest(int Id)
		{
			var guest = await _guestService.GetGuestToEditByIdAsync(Id);

			return PartialView("_EditGuest",guest);
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
        [Authorize(Policy = "Guest.Edit")]
        public async Task<IActionResult> EditGuest(EditGuestDTO guest, int Id)
		{
			if(!ModelState.IsValid)
			{
				return PartialView("_EditGuest", guest);
			}
			var result = await _guestService.EditGuestAsync(guest, Id);
			return Json(new {success = result.Success,message = result.Message});
		}

		[HttpDelete]
        [Authorize(Policy = "Guest.Delete")]
        public async Task<IActionResult> DeleteGuest(int Id)
		{
			var result = await _guestService.DeleteGuestAsync(Id);

			return Json(new {success = result.Success,message = result.Message});
		}

        [HttpGet]
        public async Task<IActionResult> GetGuestJson(int id)
        {
            var guest = await _guestService.GetGuestByIdAsync(id);

            if (guest == null)
            {
                return Json(new Guest());
            }

            return Json(guest);
        }


        [HttpGet]
        public async Task<IActionResult> SearchGuests(string term)
        {
            var guests = await _guestService.SearchGuestsAsync(term);

            return Json(guests.Select(g => new {
                id = g.Id,
                text = g.DisplayText
            }));
        }


        [HttpGet]
        public IActionResult LoadGuestForm()
        {
            return PartialView("_ManageGuestsPartial");
        }



		[HttpPost]
		public async Task<IActionResult> AddOrEditGuest([FromBody] AddGuestDTO dto)
		{
			if (dto == null)
			{
				return BadRequest(new { success = false, message = "Invalid guest data." });
			}

			if(dto.BranchId == 0 || dto.BranchId == null)
			{
				dto.BranchId = BranchId;
			}
			var result = await _guestService.AddOrEditGuestsAsync(dto);
			if (result.Success)
			{
				return Json(new
				{
					success = result.Success,
					guestId = result.Data.GuestId,
					isPrimary = result.Data.IsPrimary
				});
			}
			return Json(new {success = result.Success, message = result.Message});
		}


		[HttpPost]
		public IActionResult AddGuestsToReservation([FromBody] List<ReservationGuestDTO> guests)
		{
			if (guests == null || !guests.Any())
			{
				return Json(new { success = false, message = "Guest list is empty." });
			}

			if (!guests.Any(g => g.IsPrimary))
			{
				return Json(new { success = false, message = "One primary guest is required." });
			}

			if (guests.Count(g => g.IsPrimary) != 1)
			{
				return Json(new { success = false, message = "Exactly one primary guest must be selected." });
			}

			try
			{
				HttpContext.Session.SetString("ReservationGuests", JsonConvert.SerializeObject(guests));
				return Json(new { success = true, message = "Guests saved successfully." });
			}
			catch (Exception)
			{
				return Json(new { success = false, message = "An error occurred while saving guests." });
			}
		}

	}
}
