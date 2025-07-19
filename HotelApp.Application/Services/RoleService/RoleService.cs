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

namespace HotelApp.Application.Services.RoleService
{
    public class RoleService : IRoleService
	{
		private readonly RoleManager<Role> _roleManager;
		private readonly IMapper _mapper;
		private readonly IRoleRepository _roleRepository;
		private readonly UserManager<User> _userManager;
		private readonly int _userId;

		public RoleService(
			RoleManager<Role> roleManager,
			IMapper mapper,
			IRoleRepository roleRepository,
			UserManager<User> userManager)
		{
			_roleManager = roleManager;
			_mapper = mapper;
			_roleRepository = roleRepository;
			_userManager = userManager;
			_userId = 1; 
		}
		// ✅ Get all available permissions with info about which are assigned
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
		// ✅ Get all roles
		public async Task<IEnumerable<GetRolesDTO>> GetRolesAsync()
		{
			var roles = await _roleManager.Roles.ToListAsync();
			return _mapper.Map<List<GetRolesDTO>>(roles);
		}

		// ✅ Get a single role
		public async Task<Role?> GetRoleByIdAsync(int roleId)
		{
			return await _roleManager.FindByIdAsync(roleId.ToString());
		}

		// ✅ Add new role
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
				CreatedById = _userId,
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

		
		//// ✅ Assign selected permissions to a role
		//public async Task<ServiceResponse<string>> AssignPermissionsAsync(int roleId, List<string> permissions)
		//{
		//	var role = await GetRoleByIdAsync(roleId);
		//	if (role == null)
		//		return ServiceResponse<string>.ResponseFailure($"Role ID {roleId} not found");

		//	await _roleRepository.RemoveAllRolePermissionsAsync(role.Id);
		//	await _roleRepository.AddRolePermissionsAsync(role.Id, permissions);

		//	role.LastModifiedById = _userId;
		//	role.LastModifiedDate = DateTime.UtcNow;
		//	await _roleManager.UpdateAsync(role);

		//	return ServiceResponse<string>.ResponseSuccess($"Role '{role.Name}' permissions updated successfully");
		//}
		// ✅ Get detailed permissions grouped from JSON and assigned status
		public async Task<AssignPermissionDTO> GetAssignPermissionsDTOAsync(int roleId)
		{
			var role = await GetRoleByIdAsync(roleId);
			var assignedPermissions = await _roleRepository.GetAssignedPermissionsAsync(roleId);

			var groupedPermissions = await _roleRepository.GetAllPermissionsGroupedAsync(); // New method below

			var result = new Dictionary<string, List<PermissionViewModel>>();
			foreach (var group in groupedPermissions)
			{
				result[group.Key] = group.Value.Select(p => new PermissionViewModel
				{
					Value = p,
					IsAssigned = assignedPermissions.Contains(p)
				}).ToList();
			}

			return new AssignPermissionDTO
			{
				Id = roleId,
				RoleName = role.Name,
				IsBasic = role.IsBasic,
				PermissionGroups = result
			};
		}

		// ✅ Assign permissions to role (replace old with new)
		public async Task<ServiceResponse<object>> AssignPermissionsAsync(int roleId, List<string> newPermissions)
		{
			try
			{
				await _roleRepository.RemoveAllRolePermissionsAsync(roleId);
				await _roleRepository.AddRolePermissionsAsync(roleId, newPermissions);

				return ServiceResponse<object>.ResponseSuccess("Permissions Updated Successfully.");
			}
			catch (Exception ex)
			{
				return ServiceResponse<object>.ResponseFailure(ex.Message);
			}
		}

		// ✅ Edit role name / activation
		public async Task<ServiceResponse<RoleDTO>> EditRoleAsync(Role role)
		{
			var existingRole = await _roleManager.FindByIdAsync(role.Id.ToString());
			if (existingRole == null)
				return ServiceResponse<RoleDTO>.ResponseFailure("Role not found");

			if (existingRole.IsBasic)
				return ServiceResponse<RoleDTO>.ResponseFailure($"{role.Name} cannot be edited");

			existingRole.Name = role.Name;
			existingRole.IsActive = role.IsActive;
			existingRole.LastModifiedById = _userId;
			existingRole.LastModifiedDate = DateTime.UtcNow;

			var result = await _roleManager.UpdateAsync(existingRole);
			if (!result.Succeeded)
				return ServiceResponse<RoleDTO>.ResponseFailure(result.Errors.FirstOrDefault()?.Description ?? "Update failed");

			return ServiceResponse<RoleDTO>.ResponseSuccess("Role updated successfully");
		}

		// ✅ Delete role (if safe)
		public async Task<ServiceResponse<Role>> DeleteRoleAsync(int roleId)
		{
			var role = await _roleManager.FindByIdAsync(roleId.ToString());
			if (role == null)
				return ServiceResponse<Role>.ResponseFailure("Role not found");

			if (role.IsBasic)
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






	//   public class RoleService : IRoleService
	//{
	//	private readonly RoleManager<Role> _roleManager;
	//	private readonly IMapper _mapper;
	//	private readonly IRoleRepository _roleRepository;
	//	private readonly IUnitOfWork _unitOfWork;
	//	private readonly UserManager<User> _userManager;

	//       private readonly IHttpContextAccessor _httpContextAccessor;
	//       private readonly int? _userId;
	//       public RoleService(RoleManager<Role> roleManager, IMapper mapper,
	//		IRoleRepository roleRepository, IUnitOfWork unitOfWork, UserManager<User> userManager,
	//           IHttpContextAccessor httpContextAccessor)
	//	{
	//		_roleManager = roleManager;
	//		_mapper = mapper;
	//		_roleRepository = roleRepository;
	//		_unitOfWork = unitOfWork;
	//		_userManager = userManager;
	//		_httpContextAccessor = httpContextAccessor;

	//           //_userId = int.Parse(_httpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value);
	//       }


	//       public async Task<IEnumerable<GetRolesDTO>> GetRolesAsync()
	//	{
	//		var roles = await _unitOfWork.Repository<Role>().GetAllAsDtoAsync<GetRolesDTO>();

	//		return roles;
	//	}

	//	public async Task<Role?> GetRoleByIdAsync(int roleId)
	//	{
	//		return await _roleManager.FindByIdAsync(roleId.ToString());
	//	}

	//	public async Task<ServiceResponse<RoleDTO>> AddRoleAsync(RoleDTO roleDTO)
	//	{
	//		try
	//		{
	//			if (await _roleManager.RoleExistsAsync(roleDTO.Name) == true)
	//			{
	//				return ServiceResponse<RoleDTO>.ResponseFailure($"The role '{roleDTO.Name}' is alresdy exists");
	//			}
	//			Role newRole = new Role()
	//			{
	//				Name = roleDTO.Name,
	//				IsActive = roleDTO.IsActive,
	//				CreatedById = _userId,
	//				CreatedDate = DateTime.UtcNow
	//			};

	//			var result = await _roleManager.CreateAsync(newRole);
	//			if (!result.Succeeded)
	//			{
	//				var roleErrors = result.Errors.Select(r => r.Description).ToList();
	//				return ServiceResponse<RoleDTO>.ResponseFailure(string.Join(", ", roleErrors));
	//			}
	//			return ServiceResponse<RoleDTO>.ResponseSuccess($"Role '{roleDTO.Name}' created successfully");

	//		}
	//		catch (Exception ex)
	//		{
	//			return ServiceResponse<RoleDTO>.ResponseFailure($"An exception occurred: {ex.InnerException.Message}");

	//		}
	//	}

	//	public async Task<IEnumerable<Entity>> GetAllEntitiesAsync()
	//	{
	//		return await _roleRepository.GetAllEntitiesAsync();
	//	}

	//	public async Task<IEnumerable<PermissionDTO>> GetAllPermissionsAsync(int roleId)
	//	{
	//		var assignedIds = await _roleRepository.GetAssignedPermissionsAsync(roleId);
	//		var allPermissions = await _roleRepository.GetAllPermissionsAsync();

	//		return allPermissions.Select(p => new PermissionDTO
	//		{
	//			Id = p.Id,
	//			Action = p.Action,
	//			EntityId = p.EntityId,
	//			EntityName = p.Entity?.Name,
	//			IsAssigned = assignedIds.Contains(p.Id)
	//		});
	//	}

	//	public async Task<ServiceResponse<string>> AssignPermissionsAsync(int roleId, List<int> permissionIds)
	//	{
	//		var role = await GetRoleByIdAsync(roleId);
	//		if (role == null)
	//		{
	//			return ServiceResponse<string>.ResponseFailure($"Role with ID {roleId} not found");
	//		}
	//		await _roleRepository.RemoveAllRolePermissionsAsync(roleId);
	//		try
	//		{
	//			var permissionsToRemove = await _unitOfWork.Repository<RolePermission>().GetAllAsync(rp => rp.RoleId == roleId);

	//			_unitOfWork.Repository<RolePermission>().DeleteRange(permissionsToRemove);
	//			await _unitOfWork.CommitAsync();

	//			var PermissionstoAdd = permissionIds.Select(permissionId => new RolePermission
	//			{
	//				RoleId = roleId,
	//				PermissionId = permissionId
	//               }).ToList();
	//			await _unitOfWork.Repository<RolePermission>().AddRangeAsync(PermissionstoAdd);

	//			await _unitOfWork.CommitAsync();

	//			var ExistRole = await _roleManager.FindByIdAsync(roleId.ToString());
	//               ExistRole.LastModifiedById = _userId;
	//               ExistRole.LastModifiedDate = DateTime.UtcNow;

	//			await _roleManager.UpdateAsync(ExistRole);

	//			return ServiceResponse<string>.ResponseSuccess($"Role: {role.Name} Updated Succeffully");
	//		}
	//		catch (Exception ex)
	//		{
	//			return ServiceResponse<string>.ResponseFailure(ex.Message);
	//		}
	//	}



	//	public async Task<ServiceResponse<RoleDTO>> EditRoleAsync(Role role)
	//	{
	//		var Oldrole = await _roleManager.FindByIdAsync(role.Id.ToString());
	//		if (Oldrole == null)
	//		{
	//			return ServiceResponse<RoleDTO>.ResponseFailure($"Role con't be found");
	//		}
	//		if (Oldrole.IsBasic == true)
	//		{
	//			return ServiceResponse<RoleDTO>.ResponseFailure($"{role.Name} can't be Edited via permission");
	//		}

	//		Oldrole.Name = role.Name;
	//		Oldrole.LastModifiedById = _userId;
	//		Oldrole.LastModifiedDate = DateTime.UtcNow;
	//		Oldrole.IsActive = role.IsActive;
	//		try
	//		{
	//			var result = await _roleManager.UpdateAsync(Oldrole);
	//			if (result.Succeeded)
	//			{
	//				return ServiceResponse<RoleDTO>.ResponseSuccess($"The Role is updated successfully");
	//			}
	//			return ServiceResponse<RoleDTO>.ResponseFailure(result.Errors.FirstOrDefault()?.Description ?? "Somthing went wrong!");
	//		}
	//		catch (Exception ex)
	//		{
	//			return ServiceResponse<RoleDTO>.ResponseFailure($"Error Occurred while proccessing: {ex.Message}");
	//		}
	//	}
	//	public async Task<ServiceResponse<Role>> DeleteRoleAsync(int Id)
	//	{
	//		var role = await _roleManager.FindByIdAsync(Id.ToString());
	//		if(role == null)
	//		{
	//			return ServiceResponse<Role>.ResponseFailure($"Role ID:{Id} con't be found");
	//		}
	//		if(role.IsBasic == true)
	//		{
	//			return ServiceResponse<Role>.ResponseFailure($"{role.Name} can't be deleted via permission");
	//		}

	//		var usersInRole = await _userManager.GetUsersInRoleAsync(role.Name);
	//		if (usersInRole.Any())
	//		{
	//			return ServiceResponse<Role>.ResponseFailure($"The role '{role.Name}' cannot be deleted because it is assigned to one or more users.");
	//		}

	//		var result = await _roleManager.DeleteAsync(role);
	//		if (result.Succeeded)
	//		{
	//			return ServiceResponse<Role>.ResponseSuccess($"{role.Name} Role deleted succeefully");
	//		}

	//		return ServiceResponse<Role>.ResponseFailure("Somthing is wrong with role deletion"); 
	//	}
	//}
}
