using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace HotelApp.Application.Services.CurrentUserService
{
    public class CurrentUserService : ICurrentUserService
    {
		private readonly IHttpContextAccessor _httpContextAccessor;

		public CurrentUserService(IHttpContextAccessor httpContextAccessor)
        {
			_httpContextAccessor = httpContextAccessor;
		}

		public int? UserId
		{
			get
			{
				var value = _httpContextAccessor.HttpContext?.User.FindFirstValue(ClaimTypes.NameIdentifier);
				return int.TryParse(value, out var Id) ? Id : null;
			}
		}
    }
}
