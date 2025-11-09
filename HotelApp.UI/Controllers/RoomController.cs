using HotelApp.Application.Services.BranchService;
using HotelApp.Application.Services.RoomService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using HotelApp.Application.DTOs.Rooms;
using HotelApp.Application.Services.OptionService;
using Microsoft.EntityFrameworkCore;
using HotelApp.Application;
using BenchmarkDotNet.Attributes;
using HotelApp.Domain.Entities;
using System.Linq.Expressions;
using Microsoft.AspNetCore.Mvc.ActionConstraints;
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


		[HttpGet]
		public async Task<IActionResult> GetRoomsJson([FromQuery] RoomFilterDTO filter)
		{
			var filteredRooms = await _roomService.GetFilteredRoomsAsync(filter);

			var result = filteredRooms.Select(item => new
			{
				id = item.Id,
				roomNumber = item.RoomNumber,
				floor = item.Floor,
				typeName = item.TypeName,
				maxNumOfAdults = item.MaxNumOfAdults,
				maxNumOfChildren = item.MaxNumOfChildren,
				roomStatus = item.RoomStatusName,
				isActive = item.IsActive,
				roomStatusColor = item.RoomStatusColor
			}).ToList();

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

			var result = model.AddManyRooms 
				? await _roomService.AddManyRoomsAsync(model)
				: await _roomService.AddRoomAsync(model);

			if (result.Success)
			{
				TempData["Success"] = result.Message;
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

			return View(room);
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
        [Authorize(Policy = ("Room.Edit"))]
        public async Task<IActionResult> EditRoom(EditRoomDTO model)
		{
			if (!ModelState.IsValid)
			{
				ViewBag.Options = await _optionService.GetAllOptionsAsync();
				return View(model);
			}
            var result = await _roomService.EditRoomAsync(model);
			if(result.Success)
			{
				TempData["Success"] = result.Message;
				return RedirectToAction("Index");
			}

			TempData["Error"] = result.Message;
			ViewBag.Options = await _optionService.GetAllOptionsAsync();
			return View(model);

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


        [HttpGet]
        public async Task<IActionResult> GetAvailableRooms(string? name, int roomTypeId, DateTime checkInDate, DateTime checkOutDate)
        {
            var rooms = await _roomService.GetAvailableRoomsAsync(name, roomTypeId, checkInDate, checkOutDate);

            var result = rooms.Select(r => new
            {
                id = r.Id,
                text = $"{r.RoomNumber} ({r.RoomTypeName})"
            });
            return Json(new { success = true, data = result });
        }
        [HttpGet]
        public async Task<IActionResult> GetRoomsForEditReservationByIDs([FromQuery] List<int> ids)
        {
			var result = await _roomService.GetRoomsForEditReservationByIDs(ids);
            if (result.Count() <= 0)
                return Json(new { success = false, data = new List<object>() });

			var rooms = result.Select(r => new { id = r.Id, text = r.RoomNumber + " - " + r.RoomTypeName });

            return Json(new { success = true, data = rooms });
        }

        [HttpGet]
        public async Task<IActionResult> CheckOccupancy()
        {
            int occupancy = await _roomService.GetOccupancyPercentAsync();
            bool isHigh = occupancy >= 90;

            return Ok(new
            {
                OccupancyPercent = occupancy,
                IsHigh = isHigh
            });
        }

    }
}
