using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HotelApp.Application.DTOs.Dashboard
{
	public class GuestDashboardDTO
	{
		public int CurrentCheckedIn { get; set; }
		public int TodayArrivals { get; set; }
		public int TodayDepartures { get; set; }
		public double ReturningGuestsPercentage { get; set; }
	}
}
