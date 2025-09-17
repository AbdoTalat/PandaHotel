using HotelApp.Application.Services.CalculationTypeService;
using Microsoft.AspNetCore.Mvc;

namespace HotelApp.UI.Controllers
{
    public class CalculationTypeController : BaseController
    {
        private readonly ICalculationTypeService _calculationTypeService;

        public CalculationTypeController(ICalculationTypeService calculationTypeService)
        {
            _calculationTypeService = calculationTypeService;
        }

        [HttpGet]
        public async Task<IActionResult> GetCalculationTypesJson()
        {
            var CalculationTypes = await _calculationTypeService.GetAllCalculationTypesDropDown();
            return Json(CalculationTypes);
        }
    }
}
