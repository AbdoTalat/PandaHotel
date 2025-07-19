using HotelApp.Application.Services.LocationsService;
using Microsoft.AspNetCore.Mvc;

namespace HotelApp.UI.Controllers
{
	public class LocationController : BaseController
	{
		private readonly ILocationsService _locationsService;

		public LocationController(ILocationsService locationsService)
        {
			_locationsService = locationsService;
		}


		public async Task<IActionResult> GetAllCountriesJson()
		{
			var countries = await _locationsService.GetAllCountriesAsync();

			var result = countries.Select(c => new
			{
				Id = c.Id,
				Name = c.Name,
				PhoneCode = c.PhoneCode
			});


			return Json(result);
		}
		public async Task<IActionResult> GetAllStatesJson(int countryId)
		{
			var states = await _locationsService.GetAllStatesByCountryIdAsync(countryId);

			var result = states.Select(c => new
			{
				Id = c.Id,
				Name = c.Name
			});


			return Json(result);
		}
	}
}
