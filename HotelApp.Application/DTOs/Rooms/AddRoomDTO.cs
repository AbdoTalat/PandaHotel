using HotelApp.Domain.Common.Validation;
using HotelApp.Domain.Entities;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HotelApp.Application.DTOs.Rooms
{
    public class AddRoomDTO
	{
        public int Id { get; set; }
        //[RequiredEx]
        public string? RoomNumber { get; set; }
		public string? Description { get; set; }
		[RequiredEx]
		public int FloorId { get; set; }
		[RequiredEx]
		public decimal PricePerNight { get; set; }
		[RequiredEx]
		public int MaxNumOfAdults { get; set; }
		[RequiredEx]
		public int MaxNumOfChildrens { get; set; }
		[RequiredEx]
		public int RoomStatusId { get; set; }
		[RequiredEx]
		public int RoomTypeId { get; set; }
        public bool IsActive { get; set; } 
        public bool IsAffectedByRoomType { get; set; }

		public bool AddManyRooms { get; set; }
		public string? RoomNumberText { get; set; }
		public int? RoomNumberFrom { get; set; }
		public int? RoomNumberTo { get; set; }

		public List<int> SelectedOptions { get; set; } = new List<int>();
		public IEnumerable<SelectListItem> Floors = new List<SelectListItem>();
        public IEnumerable<SelectListItem> RoomStatuses = new List<SelectListItem>();
        public IEnumerable<SelectListItem> RoomTypes = new List<SelectListItem>();
    }
}
