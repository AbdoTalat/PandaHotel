using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HotelApp.Application.DTOs.Rooms
{
	public class EditRoomDTO
	{
		public int Id { get; set; }
		public string RoomNumber { get; set; }
		public string? Description { get; set; }
		public decimal PricePerNight { get; set; }
		public int MaxNumOfAdults { get; set; }
		public int MaxNumOfChildrens { get; set; }

		public int RoomStatusId { get; set; }
		public int FloorId { get; set; }
		public int RoomTypeId { get; set; }
		public bool IsActive { get; set; }
		public bool IsAffectedByRoomType { get; set; }

		public List<int> AssignOptions { get; set; } = new List<int>();
		public List<int> SelectedOptions { get; set; } = new List<int>();

	}
}
