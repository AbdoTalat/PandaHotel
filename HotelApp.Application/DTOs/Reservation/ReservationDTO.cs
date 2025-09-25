using HotelApp.Application.DTOs.Guests;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HotelApp.Application.DTOs.Reservation
{
	public class ReservationDTO
	{
		public List<ReservationGuestDTO?>? GuestDTOs { get; set; }
		public ReservationDetailsDTO? bookRoomDTO { get; set; }
		public ConfirmReservationDTO confirmDTO { get; set; }
	}
}
