using HotelApp.Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Security.Claims;

namespace HotelApp.UI.Controllers
{
	[Authorize]
	public class BaseController : Controller
	{		
		protected int BranchId => int.TryParse(User.FindFirstValue("DefaultBranchId"), out var Id) ? Id : 0;
		protected int UserId => int.TryParse(User.FindFirstValue(ClaimTypes.NameIdentifier), out int userId) ? userId : 0;
		
		public override void OnActionExecuting(ActionExecutingContext context)
		{
			ViewBag.BranchId = BranchId;
			ViewBag.UserId = UserId;
			
			if (BranchId == 0 || UserId == 0)
			{
				context.Result = new RedirectToActionResult("Logout", "Account", null);
				return;
			}

			base.OnActionExecuting(context);
		}
	}
}
