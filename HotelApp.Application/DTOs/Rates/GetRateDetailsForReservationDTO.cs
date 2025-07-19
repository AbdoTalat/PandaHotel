using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HotelApp.Application.DTOs.Rates
{
	public class GetRateDetailsForReservationDTO
	{
		public string typeName {  get; set; }
		public decimal hourlyPrice { get; set; }
		public decimal dailyPrice { get; set; }
		public decimal extraDailyPrice { get; set; }
		public decimal weeklyPrice { get; set; }
		public decimal monthlyPrice { get; set; }

	}
}
