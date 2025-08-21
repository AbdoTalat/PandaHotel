using HotelApp.Application.Services.RoleService;
using HotelApp.Application.Services.UserService;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using HotelApp.Application.DTOs.RoleBased;
using System.Data;
using Microsoft.AspNetCore.Authorization;
using Azure.Core;
using HotelApp.Domain.Entities;
//using HotelApp.UI.Helper;
using HotelApp.Domain.Common;

namespace HotelApp.UI.Controllers
{
    public class RoleController : Controller
    {
        private readonly UserManager<User> _userManager;
        private readonly RoleManager<Role> _roleManager;
        private readonly IUserService _userService;
        private readonly IRoleService _roleService;

        public RoleController(UserManager<User> userManager, RoleManager<Role> roleManager,
            IUserService userService, IRoleService roleService)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _userService = userService;
            _roleService = roleService;
        }

        #region Roles

        [Authorize(Policy = "Role.View")]
        public async Task<IActionResult> Index()
        {
            var roles = await _roleService.GetRolesAsync();

            return View(roles);
        }

        public async Task<IActionResult> GetRolesJson()
        {
            var roles = await _roleService.GetRolesAsync();
            var result = roles.Select(r => new
            {
                r.Id,
                r.Name
            });
            return Json(result);
        }

        [HttpGet]
        [Authorize(Policy = "Role.Add")]
        public IActionResult AddRole()
        {
            return PartialView("_AddRole");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Policy = "Role.Add")]
        public async Task<IActionResult> AddRole(RoleDTO roleDTO)
        {
            if (!ModelState.IsValid)
            {
                return PartialView(roleDTO);
            }

            var result = await _roleService.AddRoleAsync(roleDTO);
            if (result.Success)
            {
                TempData["Created"] = result.Message;
                return Json(new { success = result.Success, message = result.Message });
            }
            foreach (var error in result.Message.Split(", "))
            {
                ModelState.AddModelError("", error);
            }
            return PartialView(roleDTO);
        }



        //[HttpGet]
        //[Authorize(Policy = "CanEditRole")]
        //public async Task<IActionResult> AssignPermissions(int Id)
        //{
        //    var role = await _roleService.GetRoleByIdAsync(Id);

        //    var entities = await _roleService.GetAllEntitiesAsync();
        //    var permissions = await _roleService.GetAllPermissionsAsync(Id);

        //    var model = new AssignPermissionDTO
        //    {
        //        Id = Id,
        //        RoleName = role.Name,
        //        IsBasic = role.IsBasic,
        //        Entities = entities.Select(e => new EntityDTO { Id = e.Id, Name = e.Name }).ToList(),
        //        Permissions = permissions.ToList()
        //    };

        //    return PartialView("_AssignPermissions", model);
        //}

        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //[Authorize(Policy = "CanEditRole")]
        //public async Task<IActionResult> AssignPermissions(int roleId, List<int> permissionIds)
        //{
        //    var result = await _roleService.AssignPermissionsAsync(roleId, permissionIds);
        //    if (result.Success)
        //    {
        //        return Json(new { success = result.Success, message = result.Message });
        //    }
        //    return PartialView("_AssignPermissions");
        //}


        //[HttpGet]
        //[Authorize(Policy = "Role.Edit")]
        //public async Task<IActionResult> AssignPermissions(int Id)
        //{
        //	var role = await _roleService.GetRoleByIdAsync(Id);
        //	if (role == null)
        //		return NotFound();

        //	var permissions = await _roleService.GetAllPermissionsAsync(Id);

        //	var model = new AssignPermissionDTO
        //	{
        //		Id = Id,
        //		RoleName = role.Name,
        //		IsBasic = role.IsBasic,
        //		Permissions = permissions.ToList()
        //	};

        //	return PartialView("_AssignPermissions", model);
        //}

        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //[Authorize(Policy = "Role.Edit")]
        //public async Task<IActionResult> AssignPermissions(int roleId, List<string> permissionStrings)
        //{
        //	if (!ModelState.IsValid)
        //	{
        //		// Optionally reload permissions to show the same view again
        //		var permissions = await _roleService.GetAllPermissionsAsync(roleId);
        //		var role = await _roleService.GetRoleByIdAsync(roleId);

        //		var model = new AssignPermissionDTO
        //		{
        //			Id = roleId,
        //			RoleName = role?.Name,
        //			IsBasic = role?.IsBasic ?? false,
        //			Permissions = permissions.ToList()
        //		};

        //		return PartialView("_AssignPermissions", model);
        //	}

        //	var result = await _roleService.AssignPermissionsAsync(roleId, permissionStrings);

        //	if (result.Success)
        //	{
        //		return Json(new { success = true, message = result.Message });
        //	}

        //	return Json(new { success = false, message = result.Message });
        //}
        [HttpGet]
        [Authorize(Policy = "Role.Edit")]
        public async Task<IActionResult> AssignPermissions(int Id)
        {
            var role = await _roleService.GetRoleByIdAsync(Id);
            if (role == null)
                return NotFound();

            var dto = await _roleService.GetAssignPermissionsDTOAsync(Id);
            return View(dto);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Policy = "Role.Edit")]
        public async Task<IActionResult> AssignPermissions(int Id, List<string> permissionStrings)
        {
            if (!ModelState.IsValid)
            {
                var model = await _roleService.GetAssignPermissionsDTOAsync(Id);
                return View(model);
            }

            var result = await _roleService.AssignPermissionsAsync(Id, permissionStrings);

            if (result.Success)
            {
                TempData["Success"] = result.Message;
                return RedirectToAction("Index");
            }

            TempData["Error"] = result.Message;
            var dto = await _roleService.GetAssignPermissionsDTOAsync(Id);
            return View(dto);
        }



        [HttpGet]
        [Authorize(Policy = "Role.Edit")]
        public async Task<IActionResult> EditRole(int Id)
        {
            var role = await _roleService.GetRoleByIdAsync(Id);
            if (role == null)
            {
                return Json(new { success = false, message = "Role Cannot be found" });
            }
            return PartialView("_EditRole", role);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Policy = "Role.Edit")]
        public async Task<IActionResult> EditRole(Role role)
        {
            if (!ModelState.IsValid)
            {
                return PartialView("_EditRole", role);
            }
            var result = await _roleService.EditRoleAsync(role);

            return Json(new { success = result.Success, message = result.Message });
        }



        [HttpDelete]
        [Authorize(Policy = "Role.Delete")]
        public async Task<IActionResult> DeleteRole(int Id)
        {
            var result = await _roleService.DeleteRoleAsync(Id);

            return Json(new { success = result.Success, message = result.Message });
        }


        #endregion

    }
}
