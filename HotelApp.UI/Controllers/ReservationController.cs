using HotelApp.Application.DTOs;
using HotelApp.Application.DTOs.Guests;
using HotelApp.Application.DTOs.Rates;
using HotelApp.Application.DTOs.Reservation;
using HotelApp.Application.Services.GuestService;
using HotelApp.Application.Services.ReservationService;
using HotelApp.Application.Services.RoomService;
using HotelApp.Application.Services.RoomTypeService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
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
			IGuestService guestService, IRoomTypeService roomTypeService)
		{
			_reservationService = reservationService;
			_roomService = roomService;
			_guestService = guestService;
			_roomTypeService = roomTypeService;
		}

		[HttpGet]
		[Authorize(Policy = "Reservation.View")]
		public async Task<IActionResult> Index(string searchTerm)
		{
			var allReservations = await _reservationService.GetAllReservationAsync();

			return View(allReservations);
		}

		[HttpGet]
		[Authorize(Policy = "Reservation.Add")]
		public async Task<IActionResult> AddReservation()
		{
			ViewBag.RoomTypes = await _roomTypeService.GetRoomTypesForReservationAsync();
			return View();
		}

        [HttpPost]
        public async Task<IActionResult> SaveRoomToBook([FromBody] BookRoomDTO model)
        {
            if (!ModelState.IsValid)
            {
				return Json(new
				{
					success = false,
					message = "Please fill in all required fields correctly."
                });
            }

            foreach (var roomTypeToBook in model.roomTypeToBookDTOs)
            {
                var availabilityResult = await _roomService.CheckRoomAvailabilityAsync(roomTypeToBook.Id, roomTypeToBook.NumOfRooms);

                if (!availabilityResult.Success)
                {
					return Json(new
					{
						success = false,
						message = availabilityResult.Message
					});
				}
            }

            HttpContext.Session.SetString("BookRoomData", JsonConvert.SerializeObject(model));

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
            var userIdClaim = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (!int.TryParse(userIdClaim, out int userId))
            {
                return Unauthorized();
            }

            var roomDataJson = HttpContext.Session.GetString("BookRoomData");
			var guestsDataJson = HttpContext.Session.GetString("ReservationGuests");

			var roomData = JsonConvert.DeserializeObject<BookRoomDTO>(roomDataJson);
			var guestsData = JsonConvert.DeserializeObject<List<AddGuestDTO>>(guestsDataJson);

			var reservationDTO = new AddReservationDTO
			{
				addGuestDTOs = guestsData,
				bookRoomDTO = roomData,
				confirmReservationDTO = confirmReservationDTO
			};

			var result = await _reservationService.AddReservation(reservationDTO, userId);

			return Json(new {success = result.Success, message = result.Message});
		}

	
		[HttpGet]
		public IActionResult LoadConfirmForm()
		{
			return PartialView("_ConfirmReservationPartial");
		}

		[HttpGet]
		public IActionResult LoadGuestForm()
		{
			return PartialView("_ManageGuestsPartial");
		}
	}
}