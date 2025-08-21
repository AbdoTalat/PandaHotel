using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HotelApp.Application.DTOs.Guests
{
	public class GetGuestByIdDTO
	{
		public int Id { get; set; }
		public string FullName { get; set; }
		public DateOnly DateOfBirth { get; set; }
		public string Address { get; set; }
		public string Phone { get; set; }
		public string Email { get; set; }
		public int? ProofTypeId { get; set; }
		public string? ProofTypeName { get; set; }
		public string ProofNumber { get; set; }
		public int BranchId { get; set; }
	}
}
