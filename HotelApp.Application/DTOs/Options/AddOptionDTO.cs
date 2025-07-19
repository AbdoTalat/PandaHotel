using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HotelApp.Application.DTOs.Options
{
    public class AddOptionDTO
    {
        public string Name { get; set; }
		public string Code { get; set; }
		public decimal HourlyPrice { get; set; }
		public decimal DailyPrice { get; set; }
		public decimal ExtraDailyPrice { get; set; }
		public decimal WeeklyPrice { get; set; }
		public decimal MonthlyPrice { get; set; }
		public bool IsActive { get; set; }
		public bool DisplayOnline { get; set; }
		public int? BranchId { get; set; }
	}
}
