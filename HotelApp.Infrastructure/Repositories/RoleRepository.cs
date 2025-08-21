using HotelApp.Infrastructure.DbContext;
using Microsoft.EntityFrameworkCore;
using HotelApp.Application.IRepositories;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;
using HotelApp.Domain.Common;
using HotelApp.Domain.Entities;

namespace HotelApp.Infrastructure.Repositories
{
    public class RoleRepository : IRoleRepository
	{
		private readonly RoleManager<Role> _roleManager;
		private readonly ApplicationDbContext _context;

		public RoleRepository(RoleManager<Role> roleManager, ApplicationDbContext context)
		{
			_roleManager = roleManager;
			_context = context;
		}

		// ✅ Get Role by Id
		public async Task<Role?> GetRoleByIdAsync(int roleId)
		{
			return await _roleManager.Roles.FirstOrDefaultAsync(r => r.Id == roleId);
		}

		// ✅ Get list of permission claim values assigned to a role
		public async Task<List<string>> GetAssignedPermissionsAsync(int roleId)
		{
			var role = await _roleManager.FindByIdAsync(roleId.ToString());
			if (role == null) return new List<string>();

			var claims = await _roleManager.GetClaimsAsync(role);
			return claims
				.Where(c => c.Type == "Permission")
				.Select(c => c.Value)
				.ToList();
		}

		// ✅ Get all available permissions (from your Permissions static class)
		public async Task<List<string>> GetAllPermissionsAsync()
		{
			return await _context.RoleClaims
				.Where(rc => rc.ClaimType == "Permission")
				.Select(rc => rc.ClaimValue)
				.Distinct()
				.ToListAsync();
		}

		// ✅ Remove all permission claims from the role
		public async Task RemoveAllRolePermissionsAsync(int roleId)
		{
			var role = await _roleManager.FindByIdAsync(roleId.ToString());
			if (role == null) return;

			var claims = await _roleManager.GetClaimsAsync(role);

			foreach (var claim in claims.Where(c => c.Type == "Permission"))
			{
				await _roleManager.RemoveClaimAsync(role, claim);
			}
		}

		// ✅ Add permission claims to the role
		public async Task AddRolePermissionsAsync(int roleId, List<string> permissions)
		{
			var role = await _roleManager.FindByIdAsync(roleId.ToString());
			if (role == null) 
				return;

			var existingClaims = await _roleManager.GetClaimsAsync(role);

			foreach (var permission in permissions)
			{
				if (!existingClaims.Any(c => c.Type == "Permission" && c.Value == permission))
				{
					await _roleManager.AddClaimAsync(role, new Claim("Permission", permission));
				}
			}
		}

		public async Task<Dictionary<string, List<string>>> GetAllPermissionsGroupedAsync()
		{
			var permissions = await _context.RoleClaims
				.Where(rc => rc.ClaimType == "Permission")
				.Select(rc => rc.ClaimValue)
				.Distinct()
				.ToListAsync();

			// Expected format: "Room.View", "Room.Edit", etc.
			var grouped = permissions
				.Where(p => p.Contains('.'))
				.GroupBy(p => p.Split('.')[0]) // "Room"
				.ToDictionary(
					g => g.Key,
					g => g.OrderBy(p => p).ToList()
				);

			return grouped;
		}

	}



}
