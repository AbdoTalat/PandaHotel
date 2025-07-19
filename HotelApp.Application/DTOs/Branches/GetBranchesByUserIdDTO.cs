using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HotelApp.Application.DTOs.Branches
{
	public class GetBranchesByUserIdDTO
	{
		public int BranchId { get; set; }
		public string BranchName { get; set; }
	}
}
