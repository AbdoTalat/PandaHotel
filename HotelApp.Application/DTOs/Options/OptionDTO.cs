using HotelApp.Domain.Common.Validation;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HotelApp.Application.DTOs.Options
{
	public class OptionDTO
	{
		public int Id { get; set; }
		[RequiredEx]
		public string Name { get; set; }
		[RequiredEx]
		public string Code { get; set; }
		[RequiredEx]
		[RangeEx(0.0, double.MaxValue)]
		public decimal HourlyPrice { get; set; }
		[RequiredEx]
		[RangeEx(0.0, double.MaxValue)]
		public decimal DailyPrice { get; set; }
		[RequiredEx]
		[RangeEx(0.0, double.MaxValue)]
		public decimal ExtraDailyPrice { get; set; }
		[RequiredEx]
		[RangeEx(0.0, double.MaxValue)]
		public decimal WeeklyPrice { get; set; }
		[RequiredEx]
		[RangeEx(0.0, double.MaxValue)]
		public decimal MonthlyPrice { get; set; }
		public bool IsActive { get; set; }
		public bool DisplayOnline { get; set; }
	}
}
