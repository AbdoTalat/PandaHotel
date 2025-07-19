using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HotelApp.Application.DTOs.Rates
{
	public class EditRateDTO
	{
		public int Id { get; set; }
		public string Code { get; set; }
		[Required]
		public DateTime? EffectiveDate { get; set; }

		[Required]
		public DateTime? EndDate { get; set; }

		[Required]
		public int MinChargeDayes { get; set; }
		public bool IsActive { get; set; }
		public bool SkipHourly { get; set; }
		public bool DisplayOnline { get; set; }

		public List<EditRoomTypeRateDTO> roomTypeRateDTOs { get; set; } = new List<EditRoomTypeRateDTO>();
	}
}
