using HotelApp.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HotelApp.Application.Interfaces.IRepositories
{
    public interface IRoleRepository : IGenericRepository<Role>
    {
        //public Task<Role> GetRoleByIdAsync(int roleId);
        //public Task<List<int>> GetAssignedPermissionsAsync(int roleId);
        //public Task<IEnumerable<Permission>>? GetAllPermissionsAsync();
        //public Task<IEnumerable<Entity>>? GetAllEntitiesAsync();
        //public Task RemoveAllRolePermissionsAsync(int roleId);

        Task<Role?> GetRoleByIdAsync(int roleId);
        Task<List<string>> GetAssignedPermissionsAsync(int roleId);
        Task<List<string>> GetAllPermissionsAsync();
        Task RemoveAllRolePermissionsAsync(int roleId);
        Task AddRolePermissionsAsync(int roleId, List<string> permissions);
        Task<Dictionary<string, List<string>>> GetAllPermissionsGroupedAsync();
    }

}
