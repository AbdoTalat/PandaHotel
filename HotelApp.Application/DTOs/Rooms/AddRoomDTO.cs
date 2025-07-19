using HotelApp.Domain.Entities;
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

        [Required(ErrorMessage = "Required")]
        public string RoomNumber { get; set; }

		[Required(ErrorMessage = "Required")]
		[MinLength(5, ErrorMessage = "Min length is 5")]
		public string Description { get; set; }

		[Required(ErrorMessage = "Required")]
		public int FloorId { get; set; }

		[Required(ErrorMessage = "Required")]
		public decimal PricePerNight { get; set; }

		[Required(ErrorMessage = "Required")]
		[Range(1,10, ErrorMessage = "1 to 10")]
		public int MaxNumOfAdults { get; set; }

		[Required(ErrorMessage = "Required")]
		public int MaxNumOfChildrens { get; set; }

		[Required(ErrorMessage = "Required")]
		public string RoomStatusId { get; set; }

		[Required(ErrorMessage = "Required")]
		public int RoomTypeId { get; set; }

		[Required(ErrorMessage = "Required")]
		public int? BranchId { get; set; }
        public bool IsActive { get; set; } 
        public bool IsAffectedByRoomType { get; set; }

		public List<int> SelectedOptions { get; set; } = new List<int>();
	}
}
