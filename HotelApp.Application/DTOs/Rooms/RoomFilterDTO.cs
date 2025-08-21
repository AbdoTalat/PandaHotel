using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HotelApp.Application.DTOs.Rooms
{

	public class RoomFilterDTO
	{
		public string? RoomNumber { get; set; }
		public int? MaxNumOfAdults { get; set; }
		public int? MaxNumOfChildrens { get; set; }
		public int? RoomTypeId { get; set; }
		public int? RoomStatusId { get; set; }
		public bool? IsActive { get; set; }
	}
}
