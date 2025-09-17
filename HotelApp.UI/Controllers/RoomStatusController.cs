using HotelApp.Application.DTOs.RoomStatus;
using HotelApp.Application.Services.RoomStatusService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HotelApp.UI.Controllers
{
	public class RoomStatusController : BaseController
	{
		private readonly IRoomStatusService _roomStatusService;

		public RoomStatusController(IRoomStatusService roomStatusService)
        {
			_roomStatusService = roomStatusService;
		}

		[HttpGet]
		[Authorize(Policy = "RoomStatus.View")] 
		public async Task<IActionResult> Index()
		{
			var roomStatus = await _roomStatusService.GetAllRoomStatusAsync();
			return View(roomStatus);
		}

		[HttpGet]
		[Authorize(Policy = "RoomStatus.Add")]
		public IActionResult AddRoomStatus()
		{
			return PartialView("_AddRoomStatus");
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		[Authorize(Policy = "RoomStatus.Add")]
		public async Task<IActionResult> AddRoomStatus(RoomStatusDTO model)
		{
			if (!ModelState.IsValid)
			{
				return PartialView("_AddRoomStatus", model);
			}
			var result = await _roomStatusService.AddRoomStatusAsync(model);

			return Json(new { success = result.Success, message = result.Message });
		}


		[HttpGet]
		[Authorize(Policy = "RoomStatus.Edit")]
		public async Task<IActionResult> EditRoomStatus(int Id)
		{
			var roomStatus = await _roomStatusService.GetRoomStatusToEditByIdAsync(Id);
			return PartialView("_EditRoomStatus", roomStatus);
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		[Authorize(Policy = "RoomStatus.Edit")]
		public async Task<IActionResult> EditRoomStatus(RoomStatusDTO model)
		{
			if (!ModelState.IsValid)
			{
				return PartialView("_EditRoomStatus", model);
			}
			var result = await _roomStatusService.EditRoomStatusAsync(model);

			return Json(new { success = result.Success, message = result.Message });
		}

		[HttpDelete]	
		[Authorize(Policy = "RoomStatus.Delete")]
		public async Task<IActionResult> DeleteRoomStatus(int Id)
		{
			var result = await _roomStatusService.DeleteRoomStatusByIdAsync(Id);

			return Json(new {success = result.Success, message = result.Message});
		}

		[HttpGet]
		public async Task<IActionResult> GetRoomStatusJson()
		{
			var roomStatus = await _roomStatusService.GetRoomStatusDropDownAsync();

			return Json(roomStatus);
		}

		public async Task<IActionResult> GetRoomStatusWithOutBranchFilterJson()
		{
			var roomStatus = await _roomStatusService.GetRoomStatusDropDownWithoutBranchFilterAsync();

			var result = roomStatus.Select(rs => new
			{
				Id = rs.Id,
				Name = rs.DisplayText
			});

			return Json(result);
		}
	}
}
