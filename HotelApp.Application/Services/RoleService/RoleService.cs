using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using HotelApp.Application.DTOs.RoleBased;
using System.Data;
using HotelApp.Domain;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;
using HotelApp.Application.IRepositories;
using HotelApp.Domain.Entities;
using HotelApp.Application.Services.CurrentUserService;

namespace HotelApp.Application.Services.RoleService
{
	public class RoleService : IRoleService
	{
		private readonly RoleManager<Role> _roleManager;
		private readonly IMapper _mapper;
		private readonly IRoleRepository _roleRepository;
		private readonly UserManager<User> _userManager;
		private readonly IUnitOfWork _unitOfWork;
		private readonly ICurrentUserService _currentUserService;

		public RoleService(
			RoleManager<Role> roleManager,
			IMapper mapper,
			IRoleRepository roleRepository,
			UserManager<User> userManager,
			IUnitOfWork unitOfWork,
			ICurrentUserService currentUserService)
		{
			_roleManager = roleManager;
			_mapper = mapper;
			_roleRepository = roleRepository;
			_userManager = userManager;
			_unitOfWork = unitOfWork;
			_currentUserService = currentUserService;
		}
		public async Task<IEnumerable<PermissionDTO>> GetAllPermissionsAsync(int roleId)
		{
			var assigned = await _roleRepository.GetAssignedPermissionsAsync(roleId);
			var all = await _roleRepository.GetAllPermissionsAsync();

			return all.Select(p => new PermissionDTO
			{
				Action = p,
				IsAssigned = assigned.Contains(p)
			});
		}
		public async Task<IEnumerable<GetRolesDTO>> GetRolesAsync()
		{
			var roles = await _roleManager.Roles.Select(r => new GetRolesDTO
			{
				Id = r.Id.ToString(),
				Name = r.Name,
				CreatedDate = r.CreatedDate,
				LastModifiedDate = r.LastModifiedDate,
				IsActive = r.IsActive,
				IsBasic = r.IsSystem
			})
			.ToListAsync();
			return roles;
		}

		public async Task<Role?> GetRoleByIdAsync(int roleId)
		{
			return await _roleManager.FindByIdAsync(roleId.ToString());
		}

		public async Task<ServiceResponse<RoleDTO>> AddRoleAsync(RoleDTO roleDTO)
		{
			if (await _roleManager.RoleExistsAsync(roleDTO.Name))
			{
				return ServiceResponse<RoleDTO>.ResponseFailure($"The role '{roleDTO.Name}' already exists");
			}

			Role newRole = new Role
			{
				Name = roleDTO.Name,
				IsActive = roleDTO.IsActive,
				CreatedById = _currentUserService.UserId,
				CreatedDate = DateTime.UtcNow
			};

			var result = await _roleManager.CreateAsync(newRole);
			if (!result.Succeeded)
			{
				var errors = result.Errors.Select(e => e.Description).ToList();
				return ServiceResponse<RoleDTO>.ResponseFailure(string.Join(", ", errors));
			}

			return ServiceResponse<RoleDTO>.ResponseSuccess($"Role '{roleDTO.Name}' created successfully");
		}

		public async Task<AssignPermissionDTO> GetAssignPermissionsDTOAsync(int roleId)
		{
			var role = await GetRoleByIdAsync(roleId);
			var assignedPermissions = await _roleRepository.GetAssignedPermissionsAsync(roleId);

			var groupedPermissions = await _roleRepository.GetAllPermissionsGroupedAsync(); // New method below

			var result = new Dictionary<string, List<PermissionDTO>>();
			foreach (var group in groupedPermissions)
			{
				result[group.Key] = group.Value.Select(p => new PermissionDTO
				{
					Action = p,
					IsAssigned = assignedPermissions.Contains(p)
				}).ToList();
			}

			return new AssignPermissionDTO
			{
				Id = roleId,
				RoleName = role.Name,
				IsBasic = role.IsSystem,
				PermissionGroups = result
			};
		}

		public async Task<ServiceResponse<object>> AssignPermissionsAsync(int roleId, List<string> newPermissions)
		{
            bool IsExist = await _unitOfWork.Repository<Role>()
                .IsExistsAsync(r => r.Id == roleId);
            if (!IsExist)
            {
                return ServiceResponse<object>.ResponseFailure("Role not exist!");
            }

            bool IsBasic = await _unitOfWork.Repository<Role>()
				.IsExistsAsync(r => r.Id == roleId && r.IsSystem);
			if (IsBasic)
			{
				return ServiceResponse<object>.ResponseFailure("Can\'t update basic Role.");
			}

			try
			{
				await _roleRepository.RemoveAllRolePermissionsAsync(roleId);
				await _roleRepository.AddRolePermissionsAsync(roleId, newPermissions);

                var role = await _roleManager.FindByIdAsync(roleId.ToString());
                role.LastModifiedById = _currentUserService.UserId;
				role.LastModifiedDate = DateTime.UtcNow;

				await _roleManager.UpdateAsync(role);

                return ServiceResponse<object>.ResponseSuccess("Permissions Updated Successfully.");
			}
			catch (Exception ex)
			{
				return ServiceResponse<object>.ResponseFailure(ex.Message);
			}
		}

		public async Task<ServiceResponse<RoleDTO>> EditRoleAsync(Role role)
		{
			var existingRole = await _roleManager.FindByIdAsync(role.Id.ToString());
			if (existingRole == null)
				return ServiceResponse<RoleDTO>.ResponseFailure("Role not found");

			if (existingRole.IsSystem)
				return ServiceResponse<RoleDTO>.ResponseFailure($"{role.Name} cannot be edited");

			existingRole.Name = role.Name;
			existingRole.IsActive = role.IsActive;
			existingRole.LastModifiedById = _currentUserService.UserId;
			existingRole.LastModifiedDate = DateTime.UtcNow;

			var result = await _roleManager.UpdateAsync(existingRole);
			if (!result.Succeeded)
				return ServiceResponse<RoleDTO>.ResponseFailure(result.Errors.FirstOrDefault()?.Description ?? "Update failed");

			return ServiceResponse<RoleDTO>.ResponseSuccess("Role updated successfully");
		}

		public async Task<ServiceResponse<Role>> DeleteRoleAsync(int roleId)
		{
			var role = await _roleManager.FindByIdAsync(roleId.ToString());
			if (role == null)
				return ServiceResponse<Role>.ResponseFailure("Role not found");

			if (role.IsSystem)
				return ServiceResponse<Role>.ResponseFailure($"{role.Name} cannot be deleted");

			var users = await _userManager.GetUsersInRoleAsync(role.Name);
			if (users.Any())
				return ServiceResponse<Role>.ResponseFailure($"Role '{role.Name}' is assigned to users and cannot be deleted");

			var result = await _roleManager.DeleteAsync(role);
			if (result.Succeeded)
				return ServiceResponse<Role>.ResponseSuccess($"Role '{role.Name}' deleted successfully");

			return ServiceResponse<Role>.ResponseFailure("Role deletion failed");
		}

	}
}
