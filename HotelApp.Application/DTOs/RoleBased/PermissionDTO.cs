using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HotelApp.Application.DTOs.RoleBased
{
    public class PermissionDTO
    {
		public string Action { get; set; } = string.Empty;
		public bool IsAssigned { get; set; }
		//public int Id { get; set; } // Permission ID
		//public int EntityId { get; set; } // Associated entity ID
		//public string EntityName { get; set; } // Entity name (e.g., "Room")
		//public string Action { get; set; } // Permission action (e.g., "Create", "Read", "Update", "Delete")
		//public bool IsAssigned { get; set; } // Whether the permission is assigned to the role
	}
}
