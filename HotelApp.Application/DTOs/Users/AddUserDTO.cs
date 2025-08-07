using HotelApp.Helper.Validation;
using Microsoft.AspNetCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HotelApp.Application.DTOs.Users
{
    public class AddUserDTO
    {
        [MinLength(3)]
        [RequiredEx]
        public string FirstName { get; set; }

        [MinLength(3)]
        [RequiredEx]
        public string LastName { get; set; }
        [RequiredEx]
        [MinLength(3)]
        public string UserName { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [RequiredEx]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [RequiredEx]
        [DataType(DataType.Password)]
        [Compare("Password")]
        public string ConfirmPassword { get; set; }
        public int DefaultBranchId { get; set; }
        public bool IsActive { get; set; }

		public List<string> SelectedRoles { get; set; } = new List<string>();		
        public List<string> AllRoles { get; set; } = new();

		public List<int> SelectedBranchIds { get; set; } = new List<int>();
		public IEnumerable<DropDownDTO<string>> AllBranches { get; set; } = Enumerable.Empty<DropDownDTO<string>>();

	}
}
