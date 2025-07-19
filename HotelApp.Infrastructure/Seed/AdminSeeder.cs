using HotelApp.Application.IRepositories;
using HotelApp.Domain.Common;
using HotelApp.Domain.Entities;
using HotelApp.Infrastructure.DbContext;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace HotelApp.Infrastructure.Seed
{
    public static class AdminSeeder
	{
		public static async Task SeedAsync(IServiceProvider serviceProvider)
		{
			using var scope = serviceProvider.CreateScope();
			var userManager = scope.ServiceProvider.GetRequiredService<UserManager<User>>();
			var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<Role>>();
			var permissionLoader = scope.ServiceProvider.GetRequiredService<IPermissionLoader>();
			var config = scope.ServiceProvider.GetRequiredService<IConfiguration>();
			var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>(); 
			var t = scope.ServiceProvider.GetRequiredService<IHostEnvironment>();

			var adminConfig = config.GetSection("AdminSeed");
			var adminUserName = adminConfig["UserName"];
			var adminEmail = adminConfig["Email"];
			var adminPassword = adminConfig["Password"];
			var adminRoleName = adminConfig["Role"];

			// Create Admin Role
			var role = await roleManager.FindByNameAsync(adminRoleName);
			if (role == null)
			{
				role = new Role { Name = adminRoleName };
				await roleManager.CreateAsync(role);
			}

			// Assign All Permissions to Admin Role
			var allPermissions = permissionLoader.LoadAllPermissions();


			var existingClaims = await roleManager.GetClaimsAsync(role);
			foreach (var permission in allPermissions)
			{
				if (!existingClaims.Any(c => c.Type == "Permission" && c.Value == permission))
				{
					await roleManager.AddClaimAsync(role, new Claim("Permission", permission));
				}
			}
			
			// Create Admin User
			var adminUser = await userManager.FindByEmailAsync(adminEmail);
			if (adminUser == null)
			{
				adminUser = new User
				{
					UserName = adminUserName,
					FirstName = "admin",
					LastName = "admin",
					Email = adminEmail,
					EmailConfirmed = true,
					isFullAdmin = true,
					isActive = true
				};

				var result = await userManager.CreateAsync(adminUser, adminPassword);
				if (result.Succeeded)
				{
					await userManager.AddToRoleAsync(adminUser, adminRoleName);
				}
			}
			else
			{
				var roles = await userManager.GetRolesAsync(adminUser);
				if (!roles.Contains(adminRoleName))
				{
					foreach (var r in roles)
						await userManager.RemoveFromRoleAsync(adminUser, r);

					await userManager.AddToRoleAsync(adminUser, adminRoleName);
				}
			}

			// Assign All Branches to Admin User
			var allBranchesIDs = await dbContext.Branches
				.Select(b => b.Id)
				.ToListAsync();
			var existingUserBranches = await dbContext.UserBranches
				.Where(ub => ub.UserId == adminUser.Id)
				.Select(ub => ub.BranchId)
				.ToListAsync();

			foreach (var branchId in allBranchesIDs)
			{
				if (!existingUserBranches.Contains(branchId))
				{
					dbContext.UserBranches.Add(new UserBranch
					{
						UserId = adminUser.Id,
						BranchId = branchId,
						AssignedAt = DateTime.UtcNow
					});
				}
			}

			// Set default branch if not already set
			if (adminUser.DefaultBranchId == 0 && allBranchesIDs.Any())
			{
				adminUser.DefaultBranchId = allBranchesIDs.First();
				dbContext.Users.Update(adminUser);
			}

			await dbContext.SaveChangesAsync();
		}
	}



}