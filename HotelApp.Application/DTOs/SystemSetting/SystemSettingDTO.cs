using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HotelApp.Application.DTOs.SystemSetting
{
	public class SystemSettingDTO
	{
		public int Id {  get; set; }
		public int CheckInStatusId { get; set; }
		public int CheckOutStatusId { get; set; }
	}
}
