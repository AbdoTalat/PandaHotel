using HotelApp.Application.DTOs.Guests;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HotelApp.Application.DTOs.Reservation
{
	public class AddReservationDTO
	{
		public List<AddGuestDTO?>? addGuestDTOs { get; set; }
		public BookRoomDTO? bookRoomDTO { get; set; }
		public ConfirmReservationDTO confirmReservationDTO { get; set; }
	}
}
