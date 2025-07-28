using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HotelApp.Application.DTOs.Reservation
{
	public class BookRoomDTO
	{
		public DateTime CheckInDate { get; set; }
		public DateTime CheckOutDate { get; set; }
		public int NumOfAdults { get; set; }
		public int NumOfChildrens { get; set; }
		public int NumOfNights { get; set; }

		public int RateId { get; set; }
		public int BranchId { get; set; }
		public int ReservationSourceId { get; set; }
		public int? CompanyId { get; set; }

		public List<RoomTypeToBookDTO> roomTypeToBookDTOs { get; set; } = new List<RoomTypeToBookDTO>();
	}
	public class RoomTypeToBookDTO
	{
		public int Id { get; set; }
		public int NumOfRooms { get; set; }
	}
}
