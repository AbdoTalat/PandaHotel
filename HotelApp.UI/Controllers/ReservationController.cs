using HotelApp.Application;
using HotelApp.Application.DTOs;
using HotelApp.Application.DTOs.Guests;
using HotelApp.Application.DTOs.Rates;
using HotelApp.Application.DTOs.Reservation;
using HotelApp.Application.Services.GuestService;
using HotelApp.Application.Services.ReservationService;
using HotelApp.Application.Services.ReservationSourceService;
using HotelApp.Application.Services.RoomService;
using HotelApp.Application.Services.RoomTypeService;
using HotelApp.Domain.Entities;
using HotelApp.Infrastructure.DbContext;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Routing.Template;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;
using System.Reflection;
using System.Security.Claims;

namespace HotelApp.UI.Controllers
{
	public class ReservationController : BaseController
	{
		private readonly IReservationService _reservationService;
		private readonly IRoomService _roomService;
		private readonly IGuestService _guestService;
		private readonly IRoomTypeService _roomTypeService;
        private readonly IReservationSourceService _reservationSourceService;

        public ReservationController(IReservationService reservationService, IRoomService roomService,
			IGuestService guestService, IRoomTypeService roomTypeService, IReservationSourceService reservationSourceService
			)
		{
			_reservationService = reservationService;
			_roomService = roomService;
			_guestService = guestService;
			_roomTypeService = roomTypeService;
            _reservationSourceService = reservationSourceService;
        }

		[HttpGet]
		[Authorize(Policy = "Reservation.View")]
		public async Task<IActionResult> Index([FromQuery] ReservationFilterDTO? model)
		{
			IEnumerable<GetAllReservationsDTO> reservations;

			if (model != null && model.ReservationCategory.HasValue)
				reservations = await _reservationService.GetFilteredReservationsAsync(model);
			else
				reservations = await _reservationService.GetAllReservationAsync();

			ViewBag.CurrentReservationCategory = ((int?)model?.ReservationCategory) ?? 0;

            return View(reservations);
		}

		[HttpGet]
		[Authorize(Policy = "Reservation.View")]
		public async Task<IActionResult> GetReservationJson([FromQuery] ReservationFilterDTO model)
		{
			var filteredReservations = await _reservationService.GetFilteredReservationsAsync(model);

			var result = filteredReservations.Select(r => new
			{
				id = r.Id,
				primaryGuestName = r.PrimaryGuestName,
				checkInDate = r.CheckInDate,
				checkOutDate = r.CheckOutDate,
				createdBy = r.CreatedBy,
				status = r.Status,
				reservationSource = r.ReservationSource,
				numberOfNights = r.NumberOfNights,
				numberOfPeople = r.NumberOfPeople,
				totalPrice = r.TotalPrice,
			}).ToList();

			return Json(new { success = true, data = result });
		}
		
		[HttpGet]
		[Authorize(Policy = "Reservation.View")]
		public async Task<IActionResult> ReservationDetails(int Id)
		{
			var model = await _reservationService.GetReservationDetailsByIdAsync(Id);
			return View(model);
		}


		[HttpGet]
		[Authorize(Policy = "Reservation.Add")]
		public async Task<IActionResult> AddReservation()
		{
			ViewBag.RoomTypes = await _roomTypeService.GetRoomTypesForReservationAsync();
            var model = new ReservationDTO()
			{
				ReservationInfoDto = new ReservationInfoDTO
				{
					ReservationSources = await _reservationSourceService.GetReservationSourcesDropDownAsync()
                }
			};
            return View(model);
		}

        [HttpPost]
        public async Task<IActionResult> SavReservationInfo([FromBody] ReservationInfoDTO model)
        {
            if (!ModelState.IsValid)
            {
				return Json(new { success = false, message = "Please fill in all required fields correctly." });
				//return PartialView("Partial/_ReservationInfoPartial", model);
			}

			var ValidateSelectedRooms = await _roomService.ValidateRoomSelectionsAsync(model.RoomTypeToBookDTOs, model.RoomsIDs);
			if (!ValidateSelectedRooms.Success)
			{
				return Json(new { success = false, message = ValidateSelectedRooms.Message });
            }

            HttpContext.Session.SetString("ReservationDetailsData", JsonConvert.SerializeObject(model));

            return Json(new
            {
                success = true,
                message = "Room details saved successfully.",
                amount = 1000 
            });
        }

		[HttpGet]
		public IActionResult LoadConfirmForm()
		{
			return PartialView("_ConfirmReservationPartial");
		}

		[HttpPost]
		public async Task<IActionResult> SubmitReservation([FromBody] ConfirmReservationDTO confirmReservationDTO)
		{
			var roomDataJson = HttpContext.Session.GetString("ReservationDetailsData");
			var guestsDataJson = HttpContext.Session.GetString("ReservationGuests");

			if (string.IsNullOrEmpty(roomDataJson) || string.IsNullOrEmpty(guestsDataJson))
			{
				return Json(new { success = false, message = "Session data is missing. Please restart the reservation process." });
			}

			var roomData = JsonConvert.DeserializeObject<ReservationInfoDTO>(roomDataJson)!;
			var guestsData = JsonConvert.DeserializeObject<List<ReservationGuestDTO>>(guestsDataJson)!;

			var reservationDTO = new ReservationDTO
			{
				GuestDtos = guestsData,
				ReservationInfoDto = roomData,
				ConfirmDto = confirmReservationDTO
			};

			var result = await _reservationService.SaveReservation(reservationDTO, UserId);

			if (result.Success)
			{
				HttpContext.Session.Remove("ReservationDetailsData");
				HttpContext.Session.Remove("ReservationGuests");

				TempData["Success"] = result.Message;
			}

			return Json(new { success = result.Success, message = result.Message });
		}

        [HttpPost]
        [Authorize(Policy = "Reservation.Edit")]
        public async Task<IActionResult> ChangeReservationDates([FromBody] ChangeReservationDatesDTO model)
        {
            if (!ModelState.IsValid)
                return Json(new { success = false, message = "Invalid input data." });

            var result = await _reservationService.ChangeReservationDatesAsync(model);

            return Json(new { success = result.Success, message = result.Message });
        }

		[HttpGet]
        [Authorize]
        public async Task<IActionResult> EditReservation(int Id, string? mode)
		{
            if (!(User.HasClaim("Permission", "Reservation.Edit") || User.HasClaim("Permission", "Reservation.CheckIn")))
            {
                return Forbid();
            }
			if(!User.HasClaim("Permission", "Reservation.CheckIn") && mode == "checkIn")
			{
                return Forbid();
            }
            var model = await _reservationService.GetReservationToEditByIdAsync(Id);
            ViewBag.RoomTypes = await _roomTypeService.GetRoomTypesForReservationAsync();
			ViewBag.PageTitle = "Edit Reservation";

            if (string.Equals(mode, "checkIn", StringComparison.OrdinalIgnoreCase))
            {
                ViewBag.PageTitle = "Check In Reservation";
                model.ConfirmDto.IsPending = true;
                model.ConfirmDto.IsConfirmed = true;
                model.ConfirmDto.IsCheckedIn = true;
            }
			model.ReservationInfoDto.ReservationSources = 
				await _reservationSourceService.GetReservationSourcesDropDownAsync();


            return View(model);
		}

		[HttpPost]
		[Authorize(Policy = "Reservation.CheckIn")]
		public async Task<IActionResult> QuickCheckIn(int Id)
		{
			var result = await _reservationService.QuickCheckInByIdAsync(Id, UserId);

			return Json(new { success = result.Success, message = result.Message });
		}

		[HttpGet]
		[Authorize(Policy = "Reservation.CheckOut")]
        public async Task<IActionResult> CheckOut(int Id)
		{
			var model = await _reservationService.GetCheckOutReservationByIdAsync(Id);
			return View(model);
		}

        [HttpPost]
		[ValidateAntiForgeryToken]
        [Authorize(Policy = "Reservation.CheckOut")]
        public async Task<IActionResult> CheckOut(CheckOutDTO model)
        {
			if (!ModelState.IsValid)
			{
				return View(model);
			}

            var result = await _reservationService.CheckOutReservationAsync(model, UserId);
			if (result.Success)
			{
				TempData["Success"] = result.Message;
				return RedirectToAction("ReservationDetails", new { Id = model.Id });
			}
            return View(model);
        }
    }
}