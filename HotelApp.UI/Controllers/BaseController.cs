using HotelApp.Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Security.Claims;

namespace HotelApp.UI.Controllers
{
	public class BaseController : Controller
	{		
        //protected int? BranchId => HttpContext?.Session?.GetInt32("DefaultBranchId");

		protected int BranchId => int.TryParse(User.FindFirstValue("DefaultBranch"), out var Id) ? Id : 0;

		public override void OnActionExecuting(ActionExecutingContext context)
		{
			ViewBag.BranchId = BranchId;
			base.OnActionExecuting(context);
		}
	}
}
