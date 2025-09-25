using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HotelApp.Application.DTOs.RoomStatus
{
	public class GetAllRoomStatusDTO
	{
		public int Id { get; set; }
		public string Name { get; set; }
		public string Description { get; set; }
		public string Color { get; set; }
		public bool IsActive { get; set; }
		public bool IsSystem { get; set; }
	}
}
