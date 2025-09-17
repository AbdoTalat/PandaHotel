using HotelApp.Application.DTOs.Branches;
using HotelApp.Application.DTOs.RoleBased;
using HotelApp.Domain.Common.Validation;
using HotelApp.Domain.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HotelApp.Application.DTOs.Users
{
    public class EditUserDTO
    {
        public int Id { get; set; }
		[RequiredEx]
        public string FirstName { get; set; }
		[RequiredEx]
		public string LastName { get; set; }
		[RequiredEx]
		public string UserName { get; set; }
        [EmailAddress]
		[RequiredEx]
		public string Email { get; set; }
		public int DefaultBranchId { get; set; }
		public bool IsActive { get; set; }

		public List<string> SelectedRoles { get; set; } = new List<string>();
		public List<int> SelectedBranchIds { get; set; } = new List<int>();

		public List<string> AllRoles { get; set; } = new();
		public IEnumerable<DropDownDTO<string>> AllBranches { get; set; } = Enumerable.Empty<DropDownDTO<string>>();
	}
}
