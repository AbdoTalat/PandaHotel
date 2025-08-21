using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HotelApp.Application.DTOs.Reservation
{
	public class ReservationFilterDTO
	{
		public DateTime? CheckInDate { get; set; }
		public DateTime? CheckOutDate { get; set; }
        public string? PrimaryGuestName { get; set; }
    }
}
