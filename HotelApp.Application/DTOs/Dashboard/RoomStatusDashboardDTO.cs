using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HotelApp.Application.DTOs.Dashboard
{
	public class RoomStatusDashboardDTO 
	{		
		public int Total { get; set; }
		public int Available { get; set; }
		public int Occupied { get; set; }
		public int OutOfService { get; set; }
	}
}
