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
