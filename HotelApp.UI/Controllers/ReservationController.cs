using HotelApp.Application.DTOs;
using HotelApp.Application.DTOs.Guests;
using HotelApp.Application.DTOs.Rates;
using HotelApp.Application.DTOs.Reservation;
using HotelApp.Application.Services.GuestService;
using HotelApp.Application.Services.ReservationService;
using HotelApp.Application.Services.ReservationSourceService;
using HotelApp.Application.Services.RoomService;
using HotelApp.Application.Services.RoomTypeService;
using HotelApp.Infrastructure.DbContext;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
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

		public ReservationController(IReservationService reservationService, IRoomService roomService,
			IGuestService guestService, IRoomTypeService roomTypeService
			)
		{
			_reservationService = reservationService;
			_roomService = roomService;
			_guestService = guestService;
			_roomTypeService = roomTypeService;
		}

		[HttpGet]
		[Authorize(Policy = "Reservation.View")]
		public async Task<IActionResult> Index()
		{
			var allReservations = await _reservationService.GetAllReservationAsync();

			return View(allReservations);
		}

		[HttpGet]
		[Authorize(Policy = "Reservation.View")]
		public async Task<IActionResult> GetReservationJson([FromQuery] ReservationFilterDTO model)
		{
			var filteredReservations = await _reservationService.GetFilteredReservationsAsync(model);

			var result = filteredReservations.Select(r => new
			{
				id = r.Id,
				PrimaryGuestName = r.PrimaryGuestName,
				CheckInDate = r.CheckInDate,
				CheckOutDate = r.CheckOutDate,
				CreatedBy = r.CreatedBy,
				Status = r.Status,
                ReservationSource = r.ReservationSource,
                NumberOfNights = r.NumberOfNights,
                NumberOfPeople = r.NumberOfPeople,
                TotalPrice = r.TotalPrice,
				IsConfirmed = r.IsConfirmed,
				IsPending = r.IsPending,
				IsStarted = r.IsStarted,
				IsCheckedIn = r.IsCheckedIn,
				IsCheckedOut = r.IsCheckedOut,
				IsClosed = r.IsClosed,
				IsCancelled = r.IsCancelled,
			}).ToList();

			return Json(new { success = true, data = result });
		}

		[HttpGet]
		[Authorize(Policy = "Reservation.Add")]
		public async Task<IActionResult> AddReservation()
		{
			ViewBag.RoomTypes = await _roomTypeService.GetRoomTypesForReservationAsync();
			return View();
		}

        [HttpPost]
        public async Task<IActionResult> SaveRoomToBook([FromBody] ReservationDetailsDTO model)
        {
            if (!ModelState.IsValid)
            {
				return Json(new { success = false, message = "Please fill in all required fields correctly." });
            }

			var ValidateSelectedRooms = await _roomService.ValidateRoomSelectionsAsync(model.roomTypeToBookDTOs, model.RoomsIDs);
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


		[HttpPost]
		public async Task<IActionResult> SubmitReservation([FromBody] ConfirmReservationDTO confirmReservationDTO)
		{
            var roomDataJson = HttpContext.Session.GetString("ReservationDetailsData");
			var guestsDataJson = HttpContext.Session.GetString("ReservationGuests");

			var roomData = JsonConvert.DeserializeObject<ReservationDetailsDTO>(roomDataJson);
			var guestsData = JsonConvert.DeserializeObject<List<ReservationGuestDTO?>>(guestsDataJson);

			var reservationDTO = new ReservationDTO
			{
				GuestDTOs = guestsData,
				bookRoomDTO = roomData,
				confirmDTO = confirmReservationDTO
			};

			var result = await _reservationService.AddReservation(reservationDTO);
            if (result.Success)
            {
                HttpContext.Session.Remove("ReservationDetailsData");
                HttpContext.Session.Remove("ReservationGuests");

				TempData["Success"] = result.Message;
                return Json(new { success = result.Success, message = result.Message });
            }

            return Json(new {success = result.Success, message = result.Message});
		}

	
		[HttpGet]
		public IActionResult LoadConfirmForm()
		{
			var commentMax = typeof(ConfirmReservationDTO)
	.GetProperty(nameof(ConfirmReservationDTO.Comment))?
	.GetCustomAttribute<MaxLengthAttribute>()?.Length ?? 200;

			ViewBag.CommentMaxLength = commentMax;
			//ViewBag.ReservationComment = yourCommentText; // Optional
			return PartialView("_ConfirmReservationPartial");
		}

	}
}