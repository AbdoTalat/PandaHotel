using HotelApp.Helper.Validation;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HotelApp.Application.DTOs.Reservation
{
	public class BookRoomDTO
	{
		[RequiredEx]
		public DateTime CheckInDate { get; set; }
        [RequiredEx]
        public DateTime CheckOutDate { get; set; }

        [RequiredEx]
        public int NumOfNights { get; set; }

        [RequiredEx]
        public int RateId { get; set; }
       
        [RequiredEx]
        public int ReservationSourceId { get; set; }
		public int? CompanyId { get; set; }

		[RequiredEx]
		public List<RoomTypeToBookDTO> roomTypeToBookDTOs { get; set; } = new List<RoomTypeToBookDTO>();
		public List<int> RoomsIDs { get; set; } = new List<int>();
	}
	public class RoomTypeToBookDTO
	{
		public int Id { get; set; }
        [RequiredEx]
        public int NumOfRooms { get; set; }
		public int NumOfAdults { get; set; }
		public int NumOfChildren { get; set; }
	}
}
