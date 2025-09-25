using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HotelApp.Application.DTOs.Dashboard
{
	public class RoomOccupancyDTO
	{
		public string Status { get; set; }
		public int Count { get; set; }
		public double Percentage { get; set; }
	}
}
