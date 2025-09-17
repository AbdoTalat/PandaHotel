using HotelApp.Domain.Common.Validation;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HotelApp.Application.DTOs.RoomStatus
{
	public class RoomStatusDTO
	{
		public int Id { get; set; }
		[RequiredEx]
		public string Name { get; set; }
		[MaxLengthEx(200)]
		public string? Description { get; set; }
		[RequiredEx]
		public string Color { get; set; }
		public bool IsReservable { get; set; }
		public bool IsActive { get; set; }
	}
}
