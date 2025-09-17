using HotelApp.Domain.Common.Validation;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HotelApp.Application.DTOs.Rates
{
	public class RateDTO
	{
		public int Id { get; set; }
		[RequiredEx]
		public string Code { get; set; }
		[RequiredEx]
		public DateTime EffectiveDate { get; set; }
		[RequiredEx]
		public DateTime EndDate { get; set; }
		[RequiredEx]
		public int MinChargeDayes { get; set; }
		public bool IsActive { get; set; }
		public bool SkipHourly { get; set; }
		public bool DisplayOnline { get; set; }

		public List<RoomTypeRateDTO> RoomTypeRates { get; set; } = new List<RoomTypeRateDTO>();
	}
}
