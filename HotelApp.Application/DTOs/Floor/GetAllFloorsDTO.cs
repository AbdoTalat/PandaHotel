using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HotelApp.Application.DTOs.Floor
{
	public class GetAllFloorsDTO
	{
		public int Id { get; set; }
		public int Number { get; set; }
		public bool IsActive { get; set; }
	}
}
