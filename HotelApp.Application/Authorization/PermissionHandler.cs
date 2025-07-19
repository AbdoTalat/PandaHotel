using Microsoft.AspNetCore.Authorization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HotelApp.Application.Authorization
{
	public class PermissionHandler : AuthorizationHandler<PermissionRequirement>
	{
		protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, PermissionRequirement requirement)
		{
			var hasClaim = context.User.Claims.Any(c =>
				c.Type == "Permission" &&
				c.Value.Equals(requirement.Permission, StringComparison.OrdinalIgnoreCase));

			if (hasClaim)
			{
				context.Succeed(requirement);
			}

			return Task.CompletedTask;
		}
	}

}
