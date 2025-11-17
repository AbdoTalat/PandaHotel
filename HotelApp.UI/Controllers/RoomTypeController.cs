using HotelApp.Application.DTOs.RoomTypes;
using HotelApp.Application.Services.RoomTypeService;
using HotelApp.Domain.Entities;
using HotelApp.Helper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace HotelApp.UI.Controllers
{
    public class RoomTypeController : BaseController
    {
        private readonly IRoomTypeService _roomTypeService;

        public RoomTypeController(IRoomTypeService roomTypeService)
        {
            _roomTypeService = roomTypeService;
        }
        [HttpGet]
        [Authorize(Policy = "RoomType.View")]
        public async Task<IActionResult> Index()
        {
            var roomTypes = await _roomTypeService.GetAllRoomTypesAsync();
            return View(roomTypes);
        }

  //      public async Task<IActionResult> GetRoomTypesJson()
  //      {
  //          var roomTypes = await _roomTypeService.GetRoomTypesDropDownAsync();

  //          var result = roomTypes.Select(rt => new
  //          {
  //              Id = rt.Id,
  //              typeName = rt.DisplayText
  //          });

		//	return Json(result);
		//}


        [HttpGet]
		[Authorize(Policy = "RoomType.Add")]
		public IActionResult AddRoomType()
        {
            return PartialView("_AddRoomType");
        }

		[HttpPost]
        [ValidateAntiForgeryToken]
		[Authorize(Policy = "RoomType.Add")]
		public async Task<IActionResult> AddRoomType(RoomTypeDTO model)
		{
            if(!ModelState.IsValid)
            {
			    return PartialView("_AddRoomType", model);
            }

            var result = await _roomTypeService.AddRoomTypeAsync(model);
            return Json(new {success = result.Success, message = result.Message});
		}

        [HttpGet]
		[Authorize(Policy = "RoomType.Edit")]
		public async Task<IActionResult> EditRoomType(int Id)
        {
            var roomType = await _roomTypeService.GetRoomTypeToEditByIdAsync(Id);
            return PartialView("_EditRoomType", roomType);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
		[Authorize(Policy = "RoomType.Edit")]
		public async Task<IActionResult> EditRoomType(RoomTypeDTO model)
        {
            if (!ModelState.IsValid)
            {
                return PartialView("_EditRoomType", model);
            }
            var result = await _roomTypeService.EditRoomTypeAsync(model);

            return Json(new { success = result.Success, message = result.Message });
        }

        [HttpDelete]
		[Authorize(Policy = "RoomType.Delete")]
		public async Task<IActionResult> DeleteRoomType(int Id)
        {
            var result = await _roomTypeService.DeleteRoomTypeAsync(Id);

            return Json(new { success = result.Success, message = result.Message });
        }



		[HttpGet]
		public async Task<JsonResult> GetRoomTypeData(int roomTypeId)
		{
			var roomType = await _roomTypeService.GetRoomTypeByIdAsync(roomTypeId);

			if (roomType == null)
			{
				return Json(new { success = false, message = "Room type not found." });
			}

			return Json(new
			{
				success = true,
				price = roomType.PricePerNight,
				maxAdults = roomType.MaxNumOfAdults,
				maxChildren = roomType.MaxNumOfChildrens
			});
		}

        //[HttpGet]
        //public async Task<IActionResult> GetRoomTypesForReservationJson()
        //{
        //    var roomTypes = await _roomTypeService.GetRoomTypesForReservationAsync();

        //    var result = roomTypes.Select(rt => new
        //    {
        //        id = rt.Id,
        //        name = rt.Name,
        //        numOfAvailableRooms = rt.NumOfAvailableRooms,
        //        maxNumOfAdults = rt.MaxNumOfAdults,
        //        maxNumOfChildrens = rt.MaxNumOfChildrens
        //    }).ToList();

        //    return Json(result);
        //}

    }
}
