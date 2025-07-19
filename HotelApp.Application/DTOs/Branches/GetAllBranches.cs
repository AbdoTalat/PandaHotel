using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HotelApp.Application.DTOs.Branches
{
	public class GetAllBranches
	{
		public int Id { get; set; }
		public string Name { get; set; }
		public string Country { get; set; }
		public string State { get; set; }
		public string City { get; set; }
		public string Street { get; set; }
		public string ContactNumber { get; set; }
		public bool IsActive { get; set; } 
		public string CreatedBy { get; set; }
		public string LastModifiedBy { get; set; }

	}
}
