using HotelApp.Domain.Common;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using HotelApp.Application.IRepositories;
using HotelApp.Infrastructure.Repositories;
using HotelApp.Domain.Entities;


namespace HotelApp.Infrastructure.Seed
{
    public static class DefaultPermissionsSeeder
	{
		public static async Task SeedAsync(IServiceProvider serviceProvider)
		{
			using var scope = serviceProvider.CreateScope();
			var permissionLoader = scope.ServiceProvider.GetRequiredService<IPermissionLoader>();
			var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<Role>>();


			var allPermissions = permissionLoader.LoadAllPermissions();

			// 🔁 Seed permissions for Admin or any other roles
			await SeedPermissionsToRole(roleManager, "Admin", allPermissions);
		}

		public static async Task SeedPermissionsToRole(RoleManager<Role> roleManager, string roleName, List<string> Permissions)
		{
			var role = await roleManager.FindByNameAsync(roleName);
			if (role == null) return;

			var existingClaims = await roleManager.GetClaimsAsync(role);

			foreach (var permission in Permissions)
			{
				if (!existingClaims.Any(c => c.Type == "Permission" && c.Value == permission))
				{
					await roleManager.AddClaimAsync(role, new Claim("Permission", permission));
				}
			}
		}
	}

}
