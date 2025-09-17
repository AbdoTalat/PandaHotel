using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HotelApp.Application.DTOs.Dashboard
{
	public class RoomAvailabilityDto
	{
		public int RoomTypeId { get; set; }
		public string RoomTypeName { get; set; }
		public List<AvailabilityDayDto> Availability { get; set; } = new();
	}

	public class AvailabilityDayDto
	{
		public DateTime Date { get; set; }
		public string Label { get; set; }
		public int AvailableRooms { get; set; }
	}

}
