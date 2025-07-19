using HotelApp.Application.DTOs.SystemSetting;
using HotelApp.Application.Services.SystemSettingService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HotelApp.UI.Controllers
{
	public class SystemSettingController : BaseController
	{
		private readonly ISystemSettingService _systemSettingService;

		public SystemSettingController(ISystemSettingService systemSettingService)
        {
			_systemSettingService = systemSettingService;
		}

		[HttpGet]
		[Authorize(Policy = "SystemSetting.Edit")]
        public async Task<IActionResult> Edit()
		{
			var model = await _systemSettingService.GetSystemSettingForEditAsync();
			return View(model);
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		[Authorize(Policy = "SystemSetting.Edit")] 
		public async Task<IActionResult> Edit(SystemSettingDTO model)
		{
			if(!ModelState.IsValid)
			{
				return View(model);
			}

			var result = await _systemSettingService.EditSystemSettingAsync(model);

			if (result.Success)
			{
				TempData["Success"] = result.Message;
				var newModel = await _systemSettingService.GetSystemSettingForEditAsync();
				return View(newModel);
			}

			TempData["Error"] = result.Message;
			return View(model);
		}
	}
}
