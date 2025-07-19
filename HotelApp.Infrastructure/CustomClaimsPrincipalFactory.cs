using HotelApp.Application.DTOs.UserBranches;
using HotelApp.Domain.Entities;
using HotelApp.Infrastructure.DbContext;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace HotelApp.Infrastructure
{
    public class CustomClaimsPrincipalFactory : UserClaimsPrincipalFactory<User, Role>
	{
		private readonly UserManager<User> _userManager;
		private readonly RoleManager<Role> _roleManager;
		private readonly ApplicationDbContext _context;

		public CustomClaimsPrincipalFactory(
			UserManager<User> userManager,
			RoleManager<Role> roleManager,
			IOptions<IdentityOptions> optionsAccessor, ApplicationDbContext context)
			: base(userManager, roleManager, optionsAccessor)
		{
			_userManager = userManager;
			_roleManager = roleManager;
			_context = context;
		}

		protected override async Task<ClaimsIdentity> GenerateClaimsAsync(User user)
		{
			var identity = await base.GenerateClaimsAsync(user);
			var roles = await _userManager.GetRolesAsync(user);

			foreach (var roleName in roles)
			{
				var role = await _roleManager.FindByNameAsync(roleName);
				if (role == null) continue;

				var roleClaims = await _roleManager.GetClaimsAsync(role);

				foreach (var claim in roleClaims.Where(c => c.Type == "Permission"))
				{
					identity.AddClaim(claim);
				}
			}

			if (user.DefaultBranchId != 0)
			{
				identity.AddClaim(new Claim("DefaultBranch", user.DefaultBranchId.ToString()));

				var defaultBranchData = await _context.Branches
					.Where(b => b.Id == user.DefaultBranchId)
					.Select(b => new UserBranchesDataDTO
					{
						Id = b.Id,
						Name = b.Name
					})
					.FirstOrDefaultAsync();

				var defaultBranchJson = JsonConvert.SerializeObject(defaultBranchData);
				identity.AddClaim(new Claim("DefaultBranchData", defaultBranchJson));
			}

			// Add UserBranches
			var userBranchesData = await _context.UserBranches
				.Where(ub => ub.UserId == user.Id)
				.Select(ub => new UserBranchesDataDTO
				{
					Id = ub.BranchId,
					Name = ub.Branch.Name
				})
				.ToListAsync();

			if (userBranchesData.Any())
			{
				identity.AddClaim(new Claim("UserBranches", string.Join(",", userBranchesData.Select(b => b.Id))));
				identity.AddClaim(new Claim("UserBranchesData", JsonConvert.SerializeObject(userBranchesData)));
			}

			return identity;
		}
	}

}
