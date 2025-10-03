using HotelApp.Application.DTOs.RateCalculation;
using HotelApp.Application.DTOs.Rates;
using HotelApp.Application.Services.RateCalculationService;
using HotelApp.Application.Services.RateService;
using HotelApp.Application.Services.RoomTypeService;
using HotelApp.Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Xml.Linq;

namespace HotelApp.UI.Controllers
{
    public class RateController : BaseController
    {
        private readonly IRateService _rateService;
		private readonly IRoomTypeService _roomTypeService;
        private readonly IRateCalculationService _rateCalculationService;

        public RateController(IRateService rateService, IRoomTypeService roomTypeService, IRateCalculationService rateCalculationService)
        {
            _rateService = rateService;
			_roomTypeService = roomTypeService;
            _rateCalculationService = rateCalculationService;
        }

        [Authorize(Policy = "Rate.View")]
        public async Task<IActionResult> Index()
        {
            var rates = await _rateService.GetAllRatesAsync();
            return View(rates);
        }

		[HttpGet]
		[Authorize(Policy = "Rate.Add")]
		public async Task<IActionResult> AddRate()
        {
            ViewBag.RoomTypes = await _roomTypeService.GetAllRoomTypesAsync();
			var RoomTypes = await _roomTypeService.GetAllRoomTypesAsync();

			var model = new RateDTO
			{
				RoomTypeRates = RoomTypes.Select(rt => new RoomTypeRateDTO
				{
					RoomTypeId = rt.Id,
					RoomTypeName = rt.Name
				}).ToList()
			};

			return View(model);
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		[Authorize(Policy = "Rate.Add")]
		public async Task<IActionResult> AddRate(RateDTO model)
		{
			if (!ModelState.IsValid)
			{
				return View(model);
			}
			var result = await _rateService.AddRateAsync(model);

			if (result.Success)
			{
				TempData["Success"] = result.Message;
				return RedirectToAction("Index");
			}

			TempData["Failed"] = result.Message;
			return View(model);
		}

		[HttpGet]
		[Authorize(Policy = "Rate.Edit")]
		public async Task<IActionResult> EditRate(int Id)
		{
			var model = await _rateService.GetRateToEditByIdAsync(Id);
			
			return View(model);
		}


		[HttpPost]
		[ValidateAntiForgeryToken]
		[Authorize(Policy = "Rate.Edit")]
		public async Task<IActionResult> EditRate(RateDTO model)
		{
			if (!ModelState.IsValid)
			{
				return View(model);
			}
			
			var result = await _rateService.EditRateAsync(model);

			if (result.Success)
			{
				TempData["Success"] = result.Message;
				return RedirectToAction("Index");
			}

			TempData["Error"] = result.Message;
			return View(model);
		}
		
		
		[HttpPost]
		public async Task<IActionResult> GetRatesByRoomTypes([FromBody] RatesForReservationRequestDTO model)
		{
			var rates = await _rateService.GetRatesForReservationAsync(model);

			return Json(rates);
		}

		[HttpGet]
		public async Task<IActionResult> GetRateDetailsForReservation(int rateId)
		{
			var rateDetails = await _rateService.GetRateDetailsForReservation(rateId, BranchId);

			return Json(rateDetails);
		}

        [HttpPost]
        public async Task<IActionResult> GetReservationChargesSummary([FromBody] GetRateCalculationDTORequest model)
        {
            var result = await _rateCalculationService.GetRateCalculation(model);
            return Json(new { success = result.Success, message = result.Message, data = result.Data });
        }

    }
}
