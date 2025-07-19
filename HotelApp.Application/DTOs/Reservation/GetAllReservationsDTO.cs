using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HotelApp.Application.DTOs.Reservation
{
    public class GetAllReservationsDTO
    {
        public int Id {  get; set; }
        public string PrimaryGuestName { get; set; }
        public DateTime CheckInDate { get; set; }
        public DateTime CheckOutDate { get; set; }
		public bool IsConfirmed { get; set; }
		public bool IsPending { get; set; }
		public bool IsStarted { get; set; }
		public bool IsCheckedIn { get; set; }
		public bool IsCheckedOut { get; set; }
		public bool IsClosed { get; set; }
		public bool IsCancelled { get; set; }
        public string CreatedBy { get; set; }
		public string Status { get; set; }
	
	}
}
