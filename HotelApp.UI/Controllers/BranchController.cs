using HotelApp.Application.DTOs.Branches;
using HotelApp.Application.Services.BranchService;
using HotelApp.Domain.Entities;
using HotelApp.Infrastructure.DbContext;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System.Security.Claims;

namespace HotelApp.UI.Controllers
{
    public class BranchController : Controller
    {
        // Salam Alykom
        private readonly IBranchService _branchService;
        private readonly ApplicationDbContext _context;
		private readonly UserManager<User> _userManager;
		private readonly SignInManager<User> _signInManager;

		public BranchController(IBranchService branchService, ApplicationDbContext context,
            UserManager<User> userManager, SignInManager<User> signInManager)
        {
            _branchService = branchService;
            _context = context;
			_userManager = userManager;
			_signInManager = signInManager;
		}

        [Authorize(Policy = "Branch.View")]
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var branches = await _branchService.GetAllBranchesAsync();
            return View(branches);
        }
        public async Task<IActionResult> GetBranchesJson()
        {
            var branches = await _branchService.GetBranchsDropDownAsync();
            var result = branches.Select(b => new
            {
                id = b.Id,
                name = b.DisplayText
            });
            return Json(result);
        }

        public async Task<IActionResult> GetFilteredBranches(string? name, string? country, string? state)
        {
            var query = _context.Branches.Include(b => b.Country).Include(b => b.State).AsQueryable();

            if (!string.IsNullOrEmpty(name))
                query = query.Where(b => b.Name.Contains(name));

            if (!string.IsNullOrEmpty(country))
                query = query.Where(b => b.Country.Name.Contains(country));

            if (!string.IsNullOrEmpty(state))
                query = query.Where(b => b.State.Name.Contains(state));

            var branches = await query.Select(b => new
            {
                id = b.Id,
                name = b.Name,
                contactNumber = b.ContactNumber,
                countryName = b.Country != null ? b.Country.Name : null,
                stateName = b.State != null ? b.State.Name : null,
                createdBy = b.CreatedBy,
                lastModifiedBy = b.LastModifiedBy,
                isActive = b.IsActive,
                canEditBranch = User.IsInRole("Admin"), // Adjust permission logic
                canDeleteBranch = User.IsInRole("Admin") // Adjust permission logic
            }).ToListAsync();

            return Json(new { success = true, data = branches });
        }


        [HttpGet]
        [Authorize(Policy = "Branch.Add")]
        public IActionResult AddBranch()
        {
            return PartialView("_AddBranch");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Policy = "Branch.Add")]
        public async Task<IActionResult> AddBranch(BranchDTO branchDTO)
        {
            if(!ModelState.IsValid)
            {
                return PartialView("_AddBranch", branchDTO);
            } 
            var result = await _branchService.AddNewBranchAsync(branchDTO);

			return Json(new {success = result.Success, message = result.Message});
        }


		[HttpGet]
        [Authorize(Policy = "Branch.Edit")]
        public async Task<IActionResult> EditBranch(int Id)
		{
            var branch = await _branchService.GetBranchToEditByIdAsync(Id);
           
			return PartialView("_EditBranch", branch);
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
        [Authorize(Policy = "Branch.Edit")]
        public async Task<IActionResult> EditBranch(BranchDTO branchDTO)
		{
			if (!ModelState.IsValid)
			{
				return PartialView("_EditBranch", branchDTO);
			}
			
			var result = await _branchService.EditBranchAsync(branchDTO);
			
			return Json(new { success = result.Success, message = result.Message });
		}



		[HttpDelete]
        [Authorize(Policy = "Branch.Delete")]
        public async Task<IActionResult> DeleteBranch(int Id)
        {
            var result = await _branchService.DeleteBranchAsync(Id);

			if (result.Success)
			{
				var user = await _userManager.GetUserAsync(User);
				await _signInManager.RefreshSignInAsync(user); // Refresh claims
			}

			//var userBranches = await _branchService.GetBranchesByUserId(int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)));

			//HttpContext.Session.SetString("UserBranches", JsonConvert.SerializeObject(userBranches));

			return Json(new {success = result.Success, message = result.Message});
        }
    }
}
