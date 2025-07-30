using HotelApp.Application.DTOs.Branches;
using HotelApp.Application.DTOs.RoleBased;
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

        public string FirstName { get; set; }
		public string LastName { get; set; }
		public string UserName { get; set; }

        [EmailAddress]
		public string Email { get; set; }
		public int DefaultBranchId { get; set; }
		public bool IsActive { get; set; }

		public List<string> SelectedRoles { get; set; } = new List<string>();
		public List<int> SelectedBranchIds { get; set; } = new List<int>();

		public List<string> AllRoles { get; set; } = new();
		public IEnumerable<DropDownDTO<string>> AllBranches { get; set; } = Enumerable.Empty<DropDownDTO<string>>();



		//      public int? LastModifiedById { get; set; }
		//      public DateTime? LastModifiedDate { get; set; }

		//      public int DefaultBranchId { get; set; }
		//public List<int> AssignedBranches { get; set; } = new();


		//public List<string> AvailableRoles { get; set; } = new();
		//      public IList<string> SelectedRoles { get; set; } = new List<string>();
	}

}
