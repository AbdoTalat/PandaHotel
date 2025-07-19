using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HotelApp.Application.DTOs.RoomStatus
{
	public class EditRoomStatusDTO
	{
		public int Id { get; set; }

		[MaxLength(20)]
		public string Name { get; set; }

		[MaxLength(200)]
		public string? Description { get; set; }
		public string Color { get; set; }
		public bool IsReservable { get; set; }
		public bool IsActive { get; set; }
	}
}
