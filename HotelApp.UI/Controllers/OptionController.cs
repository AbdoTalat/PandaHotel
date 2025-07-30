using HotelApp.Application.DTOs.Options;
using HotelApp.Application.Services.OptionService;
using HotelApp.Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace HotelApp.UI.Controllers
{
	public class OptionController : BaseController
	{
		private readonly IOptionService _optionService;

		public OptionController(IOptionService optionService)
        {
			_optionService = optionService;
		}

		[HttpGet]
		[Authorize(Policy = "Option.View")]
        public async Task<IActionResult> Index()
		{
			var options = await _optionService.GetAllOptionsAsync();
			return View(options);
		}

		public async Task<IActionResult> GetAllOptionsJson()
		{
			var options = await _optionService.GetOptionsDropDownAsync();

			return Json(options);
		}

		[HttpGet]
		[Authorize(Policy = "Option.Add")]
		public IActionResult AddOption()
		{
			return PartialView("_AddOption");
		}

        [HttpPost]
		[ValidateAntiForgeryToken]
        [Authorize(Policy = "Option.Add")]
        public async Task<IActionResult> AddOption(AddOptionDTO model)
        {
			if(!ModelState.IsValid)
			{
				return PartialView("_AddOption");
			}

			var result = await _optionService.AddOptionAsync(model);

			return Json(new {success = result.Success, message = result.Message});
            
        }


		[HttpGet]
		[Authorize(Policy = "Option.Edit")]
		public async Task<IActionResult> EditOption(int Id)
		{
			var option = await _optionService.GetOptionToEditByIdAsync(Id);
			return PartialView("_EditOption", option);
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		[Authorize(Policy = "Option.Edit")]
		public async Task<IActionResult> EditOption(EditOptionDTO model)
		{
			if (!ModelState.IsValid)
			{
				return PartialView("_EditOption", model);
			}

			var result = await _optionService.EditOptionAsync(model);

			return Json(new {success = result.Success, message = result.Message});
		}

		[HttpDelete]
		[Authorize(Policy = "Option.Delete")]
		public async Task<IActionResult> DeleteOption(int Id)
		{
			var result = await _optionService.DeleteOptionAsync(Id);

			return Json(new { success = result.Success, message = result.Message });
		}
	}
}
