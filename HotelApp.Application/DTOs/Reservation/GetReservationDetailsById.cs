using HotelApp.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HotelApp.Application.DTOs.Reservation
{
    public class GetReservationDetailsById
	{
		public DateTime CheckInDate { get; set; }
		public DateTime CheckOutDate { get; set; }
		public string Status { get; set; }

		public int NumberOfNights { get; set; }
		public int NumberOfPeople { get; set; }
		public decimal PricePerNight { get; set; }
		public decimal TotalPrice { get; set; }
		public string CreatedBy { get; set; }

		public List<Guest> guests { get; set; } = new List<Guest>();
	}
}
