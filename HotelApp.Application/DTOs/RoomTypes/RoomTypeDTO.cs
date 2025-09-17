using HotelApp.Domain.Common.Validation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HotelApp.Application.DTOs.RoomTypes
{
    public class RoomTypeDTO
    {
        public int Id { get; set; }
		[RequiredEx]
		public string Name { get; set; }
		public string? Description { get; set; }
		[RequiredEx]
		public decimal PricePerNight { get; set; }
		[RequiredEx]
		public int MaxNumOfAdults { get; set; }
		[RequiredEx]
		public int MaxNumOfChildrens { get; set; }
		public bool IsActive { get; set; }
	}
}
