using HotelApp.Application.Services.DropdownService;
using Microsoft.AspNetCore.Mvc;

namespace HotelApp.UI.Controllers
{
    public class DropDownController : Controller
    {
        private readonly IDropdownService _dropdownService;

        public DropDownController(IDropdownService dropdownService)
        {
            _dropdownService = dropdownService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllCountriesDropDownAsync()
        {
            var countries = await _dropdownService.GetAllCountriesDropDownAsync();
            return Json(countries);
        }
        [HttpGet]
        public async Task<IActionResult> GetAllStatesDropDownAsync(int countryId)
        {
            var states = await _dropdownService.GetAllStatesByCountryIdDropDownAsync(countryId);
            return Json(states);
        }
    }
}
