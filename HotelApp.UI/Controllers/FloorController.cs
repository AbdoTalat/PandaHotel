using Azure.Core;
using HotelApp.Application.DTOs.Floor;
using HotelApp.Application.Services.FloorService;
using HotelApp.Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HotelApp.UI.Controllers
{
	public class FloorController : BaseController
	{
		private readonly IFloorService _floorService;
        private readonly IAuthorizationService _authorizationService;
		public FloorController(IFloorService floorService,IAuthorizationService authorizationService)
        {
			_floorService = floorService;
            _authorizationService = authorizationService;

		}

		[Authorize(Policy = "Floor.View")]
		[HttpGet]
        public async Task<IActionResult> Index()
		{
			var floors = await _floorService.GetAllFloorsAsync();
			return View(floors);
		}

		public async Task<IActionResult> GetFloorsJson()
		{
			var floors = await _floorService.GetFloorItemsAsync();

			var result = floors.Select(f => new
			{
				Id = f.Id,
				Number = f.Number
			});
			return Json(result);
		}


        [HttpGet]
        [Authorize(Policy = "Floor.Add")]
        public IActionResult AddFloor()
		{
            return PartialView("_AddFloor");
		}

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Policy = "Floor.Add")]
        public async Task<IActionResult> AddFloor(AddFloorDTO floorDTO)
        {
            if(!ModelState.IsValid)
            {
				return PartialView("_AddFloor", floorDTO);
			}
            var result = await _floorService.AddFloorAsync(floorDTO);
			return Json(new { success = result.Success, message = result.Message });
        }

        [HttpGet]
        [Authorize(Policy = "Floor.Edit")]
        public async Task<IActionResult> EditFloor(int Id)
        {
			var floor = await _floorService.GetFloorToEditByIdAsync(Id);
            return PartialView("_EditFloor", floor);
        }


		[HttpPost]
		[ValidateAntiForgeryToken]
		[Authorize(Policy = "Floor.Edit")]
		public async Task<IActionResult> EditFloor(EditFloorDTO floorDTO)
		{
			if (!ModelState.IsValid)
			{
				return PartialView("_EditFloor", floorDTO);
			}
			var result = await _floorService.EditFloorAsync(floorDTO);

			return Json(new { success = result.Success, message = result.Message });
		}


		[HttpDelete]
		[Authorize(Policy = "Floor.Delete")]
		public async Task<IActionResult> DeleteFloor(int Id)
		{
			var result = await _floorService.DeleteFloorAsync(Id);

			return Json(new { success = result.Success, message = result.Message });
		}
	}
}
