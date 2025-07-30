using HotelApp.Application.Services.BranchService;
using HotelApp.Application.Services.RoleService;
using HotelApp.Application.Services.UserService;
using Microsoft.AspNetCore.Mvc;
using HotelApp.Application.DTOs.Users;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using HotelApp.Domain;
using Newtonsoft.Json;
using HotelApp.Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using HotelApp.Infrastructure.DbContext;
using HotelApp.Application.DTOs.Branches;


namespace HotelApp.UI.Controllers
{
    public class UserController : Controller
    {
        private readonly IBranchService _branchService;
        private readonly IUserService _userService;
        private readonly IRoleService _roleService;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IAuthorizationService _authorizationService;
        private readonly SignInManager<User> _signInManager;
        private readonly UserManager<User> _userManager;
        private readonly RoleManager<Role> _roleManager;

        public UserController(IBranchService branchService, IUserService userService,
            IRoleService roleService, IUnitOfWork unitOfWork,
            IAuthorizationService authorizationService, SignInManager<User> signInManager,
            UserManager<User> userManager, RoleManager<Role> roleManager)
        {
            _branchService = branchService;
            _userService = userService;
            _roleService = roleService;
            _unitOfWork = unitOfWork;
            _authorizationService = authorizationService;
            _signInManager = signInManager;
            _userManager = userManager;
            _roleManager = roleManager;
        }
        #region Users

        [HttpGet]
        [Authorize(Policy = "User.View")]
        public async Task<IActionResult> GetUsers()
        {
            var users = await _userService.GetAllUsersAsync();

            return View(users);
        }


        public async Task<IActionResult> GetUsersJson()
        {
            int? branchId = HttpContext.Session.GetInt32("DefaultBranchId");
            var users = await _userService.GetAllUsersAsync();

            var result = users.Select((item, index) => new
            {
                Number = index + 1,
                item.Id,
                item.FirstName,
                item.LastName,
                item.UserName,
                item.Email,
                IsActive = item.IsActive ? "Active" : "Not Active",
                CreatedAt = item.CreatedDate.ToShortDateString(),
                CanEditUser = User != null && _authorizationService.AuthorizeAsync(User, "User.Edit").Result.Succeeded,
                CanDeleteUser = User != null && _authorizationService.AuthorizeAsync(User, "User.Delete").Result.Succeeded
            });

            return Json(new { success = true, data = result });
        }


        [HttpGet]
        [Authorize(Policy = "User.Add")]
        public IActionResult AddNewUser()
        {
            return PartialView("_AddNewUser");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Policy = "User.Add")]
        public async Task<IActionResult> AddNewUser(AddUserDTO userDTO)
        {
            if (!ModelState.IsValid)
            {
                return PartialView("_AddNewUser", userDTO);
            }

            var result = await _userService.AddUserAsync(userDTO);

            if (result.Success)
            {
                TempData["Created"] = result.Message;
                return Json(new { success = true, message = result.Message });
            }

            foreach (var error in result.Message.Split(", "))
            {
                ModelState.AddModelError("", error);
            }
            return PartialView("_AddNewUser", userDTO);
        }

        [HttpGet]
        public async Task<IActionResult> EditUser(int id)
        {
            var reult = await _userService.GetUserToEditByIdAsync(id);

            return View(reult.Data);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditUser(EditUserDTO model)
        {
            if (!ModelState.IsValid)
            {
                model.AllRoles = _roleManager.Roles.Select(r => r.Name).ToList();
                model.AllBranches = await _branchService.GetBranchsDropDownAsync();
                return View(model);
            }

            var result = await _userService.EditUserAsync(model);

            if (result.Success)
            {
                TempData["Success"] = "User updated successfully.";
                return RedirectToAction("GetUsers");
            }
            return View(result);
        }

        //[HttpGet]
        //[Authorize(Policy = "User.Edit")]
        //public async Task<IActionResult> EditUser(int Id)
        //{
        //    var result = await _userService.GetUserToEditByIdAsync(Id);
        //    if (result.Success)
        //    {
        //        var userBranches = await _unitOfWork.Repository<UserBranch>().GetAllAsync(ub => ub.UserId == Id, SkipBranchFilter:true);
        //        result.Data.AssignedBranches = userBranches.Select(ub => ub.BranchId).ToList();
        //        return View(result.Data);
        //    }
        //    return Json(new { success = result.Success, message = result.Message });
        //}

        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //[Authorize(Policy = "User.Edit")]
        //public async Task<IActionResult> EditUser(EditUserDTO userDTO)
        //{
        //    if (!ModelState.IsValid)
        //    {
        //        var userWithRoles = await _userService.GetUserToEditByIdAsync(userDTO.Id);
        //        userDTO.AvailableRoles = userWithRoles.Data.AvailableRoles; 
        //        return View(userDTO);
        //    }

        //    var result = await _userService.EditUserAsync(userDTO);

        //    if (!result.Success)
        //    {
        //        var userWithRoles = await _userService.GetUserToEditByIdAsync(userDTO.Id);
        //        userDTO.AvailableRoles = userWithRoles.Data.AvailableRoles;

        //        return View(userDTO);
        //    }

        //    TempData["UserUpdated"] = result.Message;

        //    var userBranchess = await _branchService.GetBranchesByUserId(int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)));
        //    HttpContext.Session.SetString("UserBranches", JsonConvert.SerializeObject(userBranchess));

        //    return RedirectToAction("GetUsers");
        //}




        [HttpDelete]
        [Authorize(Policy = "User.Delete")]
        public async Task<IActionResult> DeleteUser(int id)
        {
            var result = await _userService.DeleteUserAsync(id);

            return Json(new { success = result.Success, message = result.Message });
        }

        [HttpPost]
        public async Task<IActionResult> ChangeDefaultBranch([FromBody] int BranchId)
        {
            var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
            var user = await _userService.GetUserbyId(userId);

            var result = await _userService.UpdateDefaultBranchId(BranchId, userId);

            //if (result.Success)
            //{
            //	HttpContext.Session.SetInt32("DefaultBranchId", BranchId);

            //	var branchData = await _branchService.GetBranchByIdAsync(user.DefaultBranchId);
            //	HttpContext.Session.SetString("BranchData", JsonConvert.SerializeObject(branchData));
            //	return Ok();
            //}

            if (result.Success)
            {
                // 👇 Refresh claims to reflect new DefaultBranch
                await _signInManager.RefreshSignInAsync(user);

                return Ok(new { success = true });
            }
            return Json(new { success = result.Success, message = result.Message });
        }


        #endregion


    }
}
