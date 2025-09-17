using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HotelApp.Application.DTOs.RoleBased
{
	public class AssignPermissionDTO
	{
		public int Id { get; set; }
		public string? RoleName { get; set; }
		public bool IsBasic { get; set; }

		public Dictionary<string, List<PermissionDTO>> PermissionGroups { get; set; } = new();
	}
}
