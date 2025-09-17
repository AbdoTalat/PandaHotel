using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HotelApp.Application.DTOs.Company
{
	public class GetAllCompaniesDTO
	{
		public int Id { get; set; }
		public string Name { get; set; }
		public string Phone { get; set; }
		public string Address { get; set; }
		public bool IsActive { get; set; }
	}
}
