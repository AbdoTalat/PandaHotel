using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HotelApp.Application.DTOs.Guests
{
	public class ReservationGuestDTO
	{
		public int GuestId { get; set; }
		public bool IsPrimary { get; set; }
	}
}
