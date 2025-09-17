using HotelApp.Domain.Common.Validation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HotelApp.Application.DTOs.RoleBased
{
	public class RoleDTO
	{
		public int Id {  get; set; }
		[RequiredEx]
		public string Name { get; set; }
		public bool IsActive { get; set; }
	}
}
