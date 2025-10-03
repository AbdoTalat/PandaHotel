using HotelApp.Application.Interfaces.IRepositories;
using HotelApp.Infrastructure.DbContext;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HotelApp.Infrastructure.Repositories
{
    public class PermissionLoader : IPermissionLoader
    {
        private readonly ApplicationDbContext _context;

        public PermissionLoader(ApplicationDbContext context)
        {
            _context = context;
        }

        // Returns a flat list of all distinct permissions in the system
        public List<string?> LoadAllPermissions()
        {
            return _context.RoleClaims
                .AsNoTracking()
                .Where(rc => rc.ClaimType == "Permission")
                .Select(rc => rc.ClaimValue)
                .Distinct()
                .ToList();
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
