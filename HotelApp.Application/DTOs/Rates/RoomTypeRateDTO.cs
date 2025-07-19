using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HotelApp.Application.DTOs.Rates
{
	public class RoomTypeRateDTO
	{
		public int RoomTypeId { get; set; }
		public string? RoomTypeName { get; set; } 
		public decimal HourlyPrice { get; set; }
		public decimal DailyPrice { get; set; }
		public decimal ExtraDailyPrice { get; set; }
		public decimal WeeklyPrice { get; set; }
		public decimal MonthlyPrice { get; set; }
		public bool IsSelected { get; set; } 
	}
}
