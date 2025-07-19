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
		public string RoleName { get; set; } = string.Empty;
		public bool IsBasic { get; set; }

		public Dictionary<string, List<PermissionViewModel>> PermissionGroups { get; set; } = new();
	}

	public class PermissionViewModel
	{
		public string Value { get; set; } = string.Empty;
		public bool IsAssigned { get; set; }
	}
}
