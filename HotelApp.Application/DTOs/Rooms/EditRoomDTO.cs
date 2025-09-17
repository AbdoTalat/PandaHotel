using HotelApp.Domain.Common.Validation;
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
		[RequiredEx]
		public string RoomNumber { get; set; }
		public string? Description { get; set; }
		[RequiredEx]
		public decimal PricePerNight { get; set; }
		[RequiredEx]
		public int MaxNumOfAdults { get; set; }
		[RequiredEx]
		public int MaxNumOfChildrens { get; set; }
		[RequiredEx]
		public int RoomStatusId { get; set; }
		[RequiredEx]
		public int FloorId { get; set; }
		[RequiredEx]
		public int RoomTypeId { get; set; }
		public bool IsActive { get; set; }
		public bool IsAffectedByRoomType { get; set; }

		public List<int> AssignOptions { get; set; } = new List<int>();
		public List<int> SelectedOptions { get; set; } = new List<int>();

	}
}
