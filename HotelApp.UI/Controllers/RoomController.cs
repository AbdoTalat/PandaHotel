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
using HotelApp.Application.Services.RoomTypeService;
using HotelApp.Application.Services.FloorService;
using HotelApp.Application.Services.RoomStatusService;

namespace HotelApp.UI.Controllers
{


	public class RoomController : BaseController
	{
		private readonly IRoomService _roomService;
		private readonly IOptionService _optionService;
        private readonly IRoomTypeService _roomTypeService;
        private readonly IFloorService _floorService;
        private readonly IRoomStatusService _roomStatusService;

        public RoomController(IRoomService roomService, IOptionService optionService, 
			IRoomTypeService roomTypeService, IFloorService floorService, IRoomStatusService roomStatusService)
		{
			_roomService = roomService;
			_optionService = optionService;
            _roomTypeService = roomTypeService;
            _floorService = floorService;
            _roomStatusService = roomStatusService;
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
            var model = new AddRoomDTO();
            await LoadAddRoomDropdownsAsync(model);
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Policy = ("Room.Add"))]
		public async Task<IActionResult> AddRoom(AddRoomDTO model)
        {
			if (!ModelState.IsValid)
			{
                await LoadAddRoomDropdownsAsync(model);
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

			TempData["ErrorAlert"] = result.Message;
            await LoadAddRoomDropdownsAsync(model);
            return View(model);
		}

        [HttpGet]
        [Authorize(Policy = ("Room.Edit"))]
        public async Task<IActionResult> EditRoom(int Id)
		{
			var model = await _roomService.GetRoomToEditByIdAsync(Id);
            await LoadEditRoomDropdownsAsync(model);

            return View(model);
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
        [Authorize(Policy = ("Room.Edit"))]
        public async Task<IActionResult> EditRoom(EditRoomDTO model)
		{
			if (!ModelState.IsValid)
			{
                await LoadEditRoomDropdownsAsync(model);
                return View(model);
			}
            var result = await _roomService.EditRoomAsync(model);
			if(result.Success)
			{
				TempData["Success"] = result.Message;
				return RedirectToAction("Index");
			}

			TempData["ErrorAlert"] = result.Message;
            await LoadEditRoomDropdownsAsync(model);
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


        #region Helper Methods
        private async Task LoadEditRoomDropdownsAsync(EditRoomDTO model)
        {
            ViewBag.Options = await _optionService.GetAllOptionsAsync();
            model.RoomTypes = await _roomTypeService.GetRoomTypesDropDownAsync();
            model.Floors = await _floorService.GetFloorsDropDownAsync();
            model.RoomStatuses = await _roomStatusService.GetRoomStatusDropDownAsync();
        }
        private async Task LoadAddRoomDropdownsAsync(AddRoomDTO model)
        {
            ViewBag.Options = await _optionService.GetAllOptionsAsync();
            model.RoomTypes = await _roomTypeService.GetRoomTypesDropDownAsync();
            model.Floors = await _floorService.GetFloorsDropDownAsync();
            model.RoomStatuses = await _roomStatusService.GetRoomStatusDropDownAsync();
        }
        #endregion
    }
}
