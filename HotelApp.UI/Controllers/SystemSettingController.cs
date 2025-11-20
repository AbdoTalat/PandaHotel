using HotelApp.Application.DTOs.Rooms;
using HotelApp.Application.DTOs.SystemSetting;
using HotelApp.Application.Services.MasterDataService;
using HotelApp.Application.Services.RoomStatusService;
using HotelApp.Application.Services.SystemSettingService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HotelApp.UI.Controllers
{
	public class SystemSettingController : BaseController
	{
		private readonly ISystemSettingService _systemSettingService;
        private readonly IMasterDataService _masterDataService;
        private readonly IRoomStatusService _roomStatusService;

        public SystemSettingController(
			ISystemSettingService systemSettingService, 
			IMasterDataService masterDataService, 
			IRoomStatusService roomStatusService)
        {
			_systemSettingService = systemSettingService;
            _masterDataService = masterDataService;
            _roomStatusService = roomStatusService;
        }

		[HttpGet]
		[Authorize(Policy = "SystemSetting.Edit")]
        public async Task<IActionResult> Edit()
		{
			var model = await _systemSettingService.GetSystemSettingForEditAsync();
			await LoadSystemSettingsDropdownsAsync(model);
            return View(model);
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		[Authorize(Policy = "SystemSetting.Edit")] 
		public async Task<IActionResult> Edit(SystemSettingDTO model)
		{
			if(!ModelState.IsValid)
			{
                await LoadSystemSettingsDropdownsAsync(model);
                return View(model);
			}

			var result = await _systemSettingService.EditSystemSettingAsync(model);

			if (result.Success)
			{
				TempData["Success"] = result.Message;
				var newModel = await _systemSettingService.GetSystemSettingForEditAsync();
                await LoadSystemSettingsDropdownsAsync(newModel);
                return View(newModel);
			}
            await LoadSystemSettingsDropdownsAsync(model);
            TempData["Error"] = result.Message;
			return View(model);
		}

        #region Helper Methods
        private async Task LoadSystemSettingsDropdownsAsync(SystemSettingDTO model)
        {
            model.CalculationTypes = await _masterDataService.GetCalculationTypesAsync();
            model.CheckInRoomStatuses = await _roomStatusService.GetRoomStatusDropDownAsync();
            model.CheckOutRoomStatuses = await _roomStatusService.GetRoomStatusDropDownAsync();
        }
        #endregion
    }
}
