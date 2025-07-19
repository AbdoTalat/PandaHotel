using HotelApp.Application.Services.BranchService;
using HotelApp.Application.Services.RoomService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using HotelApp.Application.DTOs.Rooms;
using HotelApp.Application.Services.OptionService;
using Microsoft.EntityFrameworkCore;
//using HotelApp.UI.Helper;

namespace HotelApp.UI.Controllers
{
    public class RoomController : BaseController
	{
		private readonly IRoomService _roomService;
        private readonly IBranchService _branchService;
		private readonly IAuthorizationService _authorizationService;
		private readonly IOptionService _optionService;

		public RoomController(IRoomService roomService, IBranchService branchService,
			IAuthorizationService authorizationService, IOptionService optionService)
		{
			_roomService = roomService;
            _branchService = branchService;
			_authorizationService = authorizationService;
			_optionService = optionService;
		}
        [HttpGet]
		[Authorize(Policy = "Room.View")]
		public async Task<IActionResult> Index()
		{
			var rooms = await _roomService.GetAllRoomsAsync();

			var roomsReview = _roomService.GetRoomsReview();

            ViewBag.Available = roomsReview.available;
            ViewBag.Occupied = roomsReview.Occupied;
            ViewBag.Maintain = roomsReview.maintain;

			return View(rooms);
		}

		public async Task<IActionResult> GetRoomsJson(string roomNumber, int? maxAdults, int? maxChildren)
		{
			var rooms = await _roomService.GetAllRoomsAsync();

			if (!string.IsNullOrEmpty(roomNumber))
			{
				rooms = rooms.Where(r => r.RoomNumber.Contains(roomNumber)).ToList();
			}
			if (maxAdults.HasValue)
			{
				rooms = rooms.Where(r => r.MaxNumOfAdults == maxAdults.Value).ToList();
			}
			if (maxChildren.HasValue)
			{
				rooms = rooms.Where(r => r.MaxNumOfChildren == maxChildren.Value).ToList();
			}

			var result = rooms.Select(item => new
			{
				Id = item.Id,
				RoomNumber = item.RoomNumber,
				Floor = item.Floor,
				TypeName = item.TypeName,
				MaxNumOfAdults = item.MaxNumOfAdults,
				MaxNumOfChildren = item.MaxNumOfChildren,
				PricePerNight = item.PricePerNight,
                roomStatus = item.RoomStatusName,     
                roomStatusColor = item.RoomStatusColor,
                CanViewRoom = (User != null) && (_authorizationService.AuthorizeAsync(User, "Room.View").Result.Succeeded),
				CanEditRoom = (User != null) && (_authorizationService.AuthorizeAsync(User, "Room.Edit").Result.Succeeded),
				CanDeleteRoom = (User != null) && (_authorizationService.AuthorizeAsync(User, "Room.Delete").Result.Succeeded && !item.IsActive)
			});

			return Json(new { success = true, data = result });
		}

		public async Task<IActionResult> GetRoomsForRefresh()
		{
			var rooms = await _roomService.GetAllRoomsAsync();

			var result = rooms.Select(item => new
			{
				Id = item.Id,
				RoomNumber = item.RoomNumber,
				Floor = item.Floor,
				TypeName = item.TypeName,
				MaxNumOfAdults = item.MaxNumOfAdults,
				MaxNumOfChildren = item.MaxNumOfChildren,
				PricePerNight = item.PricePerNight,
                roomStatus = item.RoomStatusName,            
                roomStatusColor = item.RoomStatusColor,
                CanViewRoom = (User != null) && (_authorizationService.AuthorizeAsync(User, "Room.View").Result.Succeeded),
				CanEditRoom = (User != null) && (_authorizationService.AuthorizeAsync(User, "Room.Edit").Result.Succeeded),
				CanDeleteRoom = (User != null) && (_authorizationService.AuthorizeAsync(User, "Room.Delete").Result.Succeeded && !item.IsActive)
			});

			return Json(new { success = true, data = result });
		}


		[HttpGet]
        [Authorize(Policy = ("Room.Add"))]
        public async Task<IActionResult> AddRoom()
        {
			ViewBag.Options = await _optionService.GetAllOptionsAsync();
			return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Policy = ("Room.Add"))]
        public async Task<IActionResult> AddRoom(AddRoomDTO model)
        {
			if (!ModelState.IsValid)
			{
				ViewBag.Options = await _optionService.GetAllOptionsAsync();
				return View(model);
			}
			var result = await _roomService.AddRoomAsync(model);
			if (result.Success)
			{
				return RedirectToAction("Index");
			}

			TempData["Error"] = result.Message;
			ViewBag.Options = await _optionService.GetAllOptionsAsync();
			return View(model);
		}

        [HttpGet]
        [Authorize(Policy = ("Room.Edit"))]
        public async Task<IActionResult> EditRoom(int Id)
		{
			var room = await _roomService.GetRoomToEditByIdAsync(Id);
			ViewBag.Options = await _optionService.GetAllOptionsAsync();

			return PartialView("_EditRoom" ,room);
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
        [Authorize(Policy = ("Room.Edit"))]
        public async Task<IActionResult> EditRoom(EditRoomDTO model)
		{
			if (!ModelState.IsValid)
			{
				ViewBag.Options = await _optionService.GetAllOptionsAsync();
				return PartialView("_EditRoom" , model);
			}
            var result = await _roomService.EditRoomAsync(model);
            
            return Json(new { success = result.Success, message = result.Message });
			
		}

        [HttpGet]
        [Authorize(Policy = "Room.View")]
        public async Task<IActionResult> GetRoomDetails(int Id)
        {
            var room = await _roomService.GetRoomByIdAsync(Id);

            return PartialView("_RoomDetails", room);
        }
        
        [HttpDelete]
        [Authorize(Policy = "Room.Delete")]
        public async Task<IActionResult> DeleteRoom(int Id)
        {
            var result = await _roomService.DeleteRoomAsync(Id);

            return Json(new {success = result.Success, message = result.Message});
        }


	}
}
