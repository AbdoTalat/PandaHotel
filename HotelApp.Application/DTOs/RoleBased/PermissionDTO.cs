using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HotelApp.Application.DTOs.RoleBased
{
    public class PermissionDTO
    {
		public string? Action { get; set; }
		public bool IsAssigned { get; set; }
	}
}
