using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HotelApp.Application.Services.CurrentUserService
{
    public interface ICurrentUserService
    {
		int? UserId { get; }
		//int? BranchId { get; }
	}
}
