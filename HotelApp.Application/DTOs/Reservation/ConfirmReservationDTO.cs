using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HotelApp.Application.DTOs.Reservation
{
	public class ConfirmReservationDTO
	{
		[MaxLength(200)]
		public string? Comment { get; set; }
        public bool IsPending { get; set; }
        public bool IsConfirmed { get; set; }
        public bool IsCheckedIn { get; set; }
        public bool IsCheckedOut { get; set; }
        public bool IsCancelled { get; set; }
        public bool IsNoShow { get; set; }
        public string? CancellationReason { get; set; } = null;
	}
}
