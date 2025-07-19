using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HotelApp.Application.DTOs.RoleBased;
using HotelApp.Domain.Entities;

namespace HotelApp.Application.Services.RoleService
{
    public interface IRoleService
	{
		Task<IEnumerable<GetRolesDTO>> GetRolesAsync();
		Task<Role?> GetRoleByIdAsync(int roleId);
		Task<ServiceResponse<RoleDTO>> AddRoleAsync(RoleDTO roleDTO);
		Task<IEnumerable<PermissionDTO>> GetAllPermissionsAsync(int roleId);
		Task<ServiceResponse<object>> AssignPermissionsAsync(int roleId, List<string> newPermissions);
		Task<AssignPermissionDTO> GetAssignPermissionsDTOAsync(int roleId);
		Task<ServiceResponse<RoleDTO>> EditRoleAsync(Role role);
		Task<ServiceResponse<Role>> DeleteRoleAsync(int Id);
	}
}
