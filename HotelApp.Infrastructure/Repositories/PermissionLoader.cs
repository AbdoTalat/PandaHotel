using HotelApp.Application.Interfaces.IRepositories;
using HotelApp.Infrastructure.DbContext;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.Json;
using Microsoft.AspNetCore.Hosting;

namespace HotelApp.Infrastructure.Repositories
{
    public class PermissionLoader : IPermissionLoader
    {
        private readonly ApplicationDbContext _context;
        private readonly IWebHostEnvironment _env;

        public PermissionLoader(ApplicationDbContext context, IWebHostEnvironment env)
        {
            _context = context;
            _env = env;
        }
        public List<string?> LoadAllPermissionsFromJsonFile()
        {
            var jsonPath = Path.Combine(_env.ContentRootPath, "permissions.json");
            // ✅ Always load from the JSON file (flat format like "Reservation.Add")
            if (File.Exists(jsonPath))
            {
                var json = File.ReadAllText(jsonPath);

                var data = JsonSerializer.Deserialize<Dictionary<string, List<string>>>(json);
                if (data != null)
                {
                    return data
                        .SelectMany(x => x.Value) // Flatten all permission arrays
                        .Distinct()
                        .ToList();
                }
            }

            // Fallback: load from DB if JSON missing
            return _context.RoleClaims
                .AsNoTracking()
                .Where(rc => rc.ClaimType == "Permission")
                .Select(rc => rc.ClaimValue)
                .Distinct()
                .ToList();
        }

        // Returns a flat list of all distinct permissions in the system
        public async Task<List<string?>> LoadAllPermissions()
        {
            return await  _context.RoleClaims
                .AsNoTracking()
                .Where(rc => rc.ClaimType == "Permission")
                .Select(rc => rc.ClaimValue)
                .Distinct()
                .ToListAsync();
        }

        // Returns grouped permissions by role
        public Dictionary<string, List<string>> LoadGroupedPermissions()
        {
            return _context.Roles
                .AsNoTracking()
                .GroupJoin(
                    _context.RoleClaims.Where(rc => rc.ClaimType == "Permission"),
                    role => role.Id,
                    rc => rc.RoleId,
                    (role, claims) => new { role.Name, Claims = claims.Select(c => c.ClaimValue).ToList() }
                )
                .ToDictionary(r => r.Name, r => r.Claims);
        }
    }

}
