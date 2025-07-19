using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using HotelApp.Application.DTOs.Users;
using Microsoft.AspNetCore.Authorization;
using Newtonsoft.Json;
using HotelApp.Application.Services.BranchService;
using HotelApp.Application.DTOs.Account;
using Microsoft.EntityFrameworkCore;
using HotelApp.Domain.Entities;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.Extensions.Options;

namespace HotelApp.UI.Controllers
{
    public class AccountController : Controller
	{
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
		private readonly IBranchService _branchService;

		public AccountController(UserManager<User> userManager, 
			SignInManager<User> signInManager, IBranchService branchService)
        {
            _userManager = userManager;
            _signInManager = signInManager;
			_branchService = branchService;
		}
        [HttpGet]
		public IActionResult Login()
		{
			return View();
		}

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginDTO model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var user = await _userManager.FindByEmailAsync(model.email);

            if (user == null)
            {
                ModelState.AddModelError("email", "This Email Does Not Exist.");
                return View(model);
            }

            if (!await _userManager.CheckPasswordAsync(user, model.password))
            {
                ModelState.AddModelError("password", "Invalid Password!");
                return View(model);
            }

            if (!user.isActive)
            {
                TempData["NotActive"] = "This user account is not active.";
                return View(model);
            }

            //var userBranches = await _branchService.GetBranchesByUserId(user.Id);
            //var branchData = await _branchService.GetBranchByIdAsync(user.DefaultBranchId);

            //HttpContext.Session.SetString("UserBranches", JsonConvert.SerializeObject(userBranches));
            //HttpContext.Session.SetInt32("DefaultBranchId", user.DefaultBranchId);
            //HttpContext.Session.SetString("BranchData", JsonConvert.SerializeObject(branchData));

			await _signInManager.SignInAsync(user, isPersistent: model.rememberMe); // ← this adds all claims via your factory

            return RedirectToAction("Index", "Home");
        }


        [Authorize]
		public async Task<IActionResult> LogOut()
		{
			await _signInManager.SignOutAsync();
			return RedirectToAction("Login", "Account");
		}


	}
}
