using HotelApp.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HotelApp.Application.DTOs.Rooms
{
	public class GetRoomByIdDTO
	{
		public int Id { get; set; }
		public string RoomNumber { get; set; }
		public string Description { get; set; }
		public Int16 Floor { get; set; }
		public decimal PricePerNight { get; set; }
		public string TypeName { get; set; }
		public int MaxNumOfAdults { get; set; } 
		public int MaxNumOfChildrens { get; set; }
		public string StatusName { get; set; }
	}
}
