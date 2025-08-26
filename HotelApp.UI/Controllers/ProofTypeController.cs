using HotelApp.Application.DTOs.ProofType;
using HotelApp.Application.Services.ProofTypeService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HotelApp.UI.Controllers
{
    public class ProofTypeController : Controller
    {
        private readonly IProofTypeService _proofTypeService;

        public ProofTypeController(IProofTypeService proofTypeService)
        {
            _proofTypeService = proofTypeService;
        }

        [HttpGet]
        [Authorize(Policy = "ProofType.View")]
        public async Task<IActionResult> Index()
        {
            var model = await _proofTypeService.GetAllProofTypesAsync();
            return View(model);
        }

        [HttpGet]
        [Authorize(Policy = "ProofType.Add")]
        public IActionResult AddProofType()
        {
            return PartialView("_AddProofType");
        }

		[HttpPost]
        [ValidateAntiForgeryToken]
		[Authorize(Policy = "ProofType.Add")]
		public async Task<IActionResult> AddProofType(ProofTypeDTO model)
		{
			if (!ModelState.IsValid)
			{
				return PartialView("_AddProofType");
			}

			var result = await _proofTypeService.AddProofTypeAsync(model);

			return Json(new { success = result.Success, message = result.Message });
		}

		[HttpGet]
		[Authorize(Policy = "ProofType.Edit")]
		public async Task<IActionResult> EditProofType(int Id)
		{
			var model = await _proofTypeService.GetProofTypeToEditByIdAsync(Id);

			return PartialView("_EditProofType", model);
		}

		[HttpPost]
        [ValidateAntiForgeryToken]
		[Authorize(Policy = "ProofType.Edit")]
		public async Task<IActionResult> EditProofType(ProofTypeDTO model)
		{
            if (!ModelState.IsValid)
            {
                return PartialView("_EditProofType", model);
            }
			var result = await _proofTypeService.EditProofTypeAsync(model);

            return Json(new { success = result.Success, message = result.Message });
		}

		[HttpDelete]
		[Authorize(Policy = "ProofType.Delete")]
		public async Task<IActionResult> DeleteProofType(int Id)
		{
			var result = await _proofTypeService.DeleteProofTypeAsync(Id);

			return Json(new { success = result.Success, message = result.Message });
		}


		[HttpGet]
        public async Task<IActionResult> GetProofTypesJson()
        {
            var ProofTypes = await _proofTypeService.GetProofTypesDropDownAsync();
            var result = ProofTypes.Select(pt => new
            {
                Id = pt.Id,
                Name = pt.DisplayText
            });
            return Json(result);
        }
    }
}
