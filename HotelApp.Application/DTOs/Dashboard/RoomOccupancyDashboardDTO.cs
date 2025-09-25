using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HotelApp.Application.DTOs.Dashboard
{
	public class RoomOccupancyDashboardDTO
	{
		public DateTime Date { get; set; }
		public string Label { get; set; }
		public int OccupiedRooms { get; set; }
        public int TotalRooms { get; set; } 
    }

}
