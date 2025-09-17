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

			var roleIds = await _context.UserRoles
				.Where(ur => ur.UserId == user.Id)
				.Select(ur => ur.RoleId)
				.ToListAsync();

			var roleClaims = await _context.RoleClaims
				.Where(rc => roleIds.Contains(rc.RoleId) && rc.ClaimType == "Permission")
				.ToListAsync();

			foreach (var claim in roleClaims)
			{
				if (!string.IsNullOrEmpty(claim.ClaimType) && !string.IsNullOrEmpty(claim.ClaimValue))
				{
					identity.AddClaim(new Claim(claim.ClaimType, claim.ClaimValue));
				}
			}

			if (user.DefaultBranchId > 0)
			{
				identity.AddClaim(new Claim("DefaultBranchId", user.DefaultBranchId.ToString()));
			}


			return identity;
		}
	}

}
