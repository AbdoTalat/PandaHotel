using AspNetCore;
using HotelApp.Application.DTOs.Company;
using HotelApp.Application.Services.CompanyService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HotelApp.UI.Controllers
{
	//[Authorize]
	public class CompanyController : BaseController
	{
		private readonly ICompanyService _companyService;

		public CompanyController(ICompanyService companyService)
        {
			_companyService = companyService;
		}

		[Authorize( Policy = "Company.View")]
		[HttpGet]
		public async Task<IActionResult> Index()
		{
			var model = await _companyService.GetAllCompaniesAsync();
			return View(model);
		}

		[HttpGet]		
		[Authorize(Policy = "Company.Add")]
		public IActionResult AddCompany()
		{
			return PartialView("_AddCompany");
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		[Authorize(Policy = "Company.Add")]
		public async Task<IActionResult> AddCompany(CompanyDTO model)
		{
			if (!ModelState.IsValid)
			{
				return PartialView("_AddCompany", model);
			}

			var result = await _companyService.AddCompanyAsync(model);

			return Json(new { success = result.Success, message = result.Message });
		}

		[HttpGet]
		[Authorize(Policy = "Company.Edit")]
		public async Task<IActionResult> EditCompany(int Id)
		{
			var company = await _companyService.GetCompanyToEditByIdAsync(Id);

			return PartialView("_EditCompany", company);
		}

        [HttpPost]
		[ValidateAntiForgeryToken]     
		[Authorize(Policy = "Company.Edit")]
        public async Task<IActionResult> EditCompany(CompanyDTO model)
        {
			if (!ModelState.IsValid)
			{
				return PartialView("_EditCompany", model);
			}
            
			var result = await _companyService.EditCompanyAsync(model);
			return Json(new { success = result.Success, message = result.Message});
        }

        #region JSON Methods
        public async Task<IActionResult> GetCompaniesDropDown()
		{
			var Companies = await _companyService.GetCompaniesDropDownAsync();
			return Json(Companies);
		}

		[HttpGet]
		public async Task<IActionResult> GetSerachedCompaniesByName(string Name)
		{
			var companies = await _companyService.SerachCompanyByNameAsync(Name);
			return Json(companies);
		}

		[HttpGet]
		public async Task<IActionResult> GetSerachedCompanyData(int Id)
		{
			var company = await _companyService.GetSearchedCompanyDataAsync(Id);
			return Json(company);
		}

        [HttpPost]
        public async Task<IActionResult> SaveCompany([FromBody] ReservationCompanyDTO companyDto)
        {
            if (!ModelState.IsValid)
			{
				return Json(new { success = false, message = "Invalid Data" });
			}

            var result = await _companyService.CreateOrUpdateCompanyAsync(companyDto);

			return Json(new
            {
                success = result.Success,
                message = result.Message,
                companyId = result.Data?.Id,
                name = result.Data?.Name
            });
        }
        #endregion

    }
}
