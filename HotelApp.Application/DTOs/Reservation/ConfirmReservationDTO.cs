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
		public bool IsConfirmed { get; set; } = false;
		public bool IsPending { get; set; } = false;
		public bool IsStarted { get; set; } = false;
		public bool IsCheckedIn { get; set; } = false;
		public bool IsCheckedOut { get; set; } = false;
		public bool IsClosed { get; set; } = false;
		public bool IsCancelled { get; set; } = false;
		public string? CancellationReason { get; set; }
	}
}
