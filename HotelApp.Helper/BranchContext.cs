using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Security.Claims;

namespace HotelApp.Helper
{
	public static class BranchContext
	{
		private static IHttpContextAccessor? _httpContextAccessor;

		public static void Configure(IHttpContextAccessor httpContextAccessor)
		{
			_httpContextAccessor = httpContextAccessor;
		}

		public static int? CurrentBranchId
		{
			get
			{
				var claimValue = _httpContextAccessor?.HttpContext?.User?
					.Claims.FirstOrDefault(c => c.Type == "DefaultBranch")?.Value;

				return int.TryParse(claimValue, out var branchId) ? branchId : null;
			}
		}
	}
}
