using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HotelApp.Application.DTOs.Options
{
	public class GetAllOptionsDTO
	{
		public int Id { get; set; }
		public string Name { get; set; }
		public bool IsActive { get; set; }
	}
}
