using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HotelApp.Application.DTOs.Dashboard
{
	public class TodayReservationsSummaryDto
	{
		public int Arrivals { get; set; } = 0;
		public int Departures { get; set; } = 0;
		public int NewBookings { get; set; } = 0;
		public int StayOvers { get; set; } = 0;
		public int Cancellations { get; set; } = 0;
	}

}
