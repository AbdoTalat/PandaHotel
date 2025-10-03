using HotelApp.Application.DTOs.Guests;
using HotelApp.Domain.Common.Validation;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HotelApp.Application.DTOs.Reservation
{
	public class ReservationDTO
	{
		public List<ReservationGuestDTO>? GuestDtos { get; set; } 
		public ReservationInfoDTO? reservationInfoDto { get; set; }
		public ConfirmReservationDTO? confirmDto { get; set; }
	}

	public class ReservationInfoDTO
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
		public List<RoomTypeToBookDTO> RoomTypeToBookDTOs { get; set; } = new List<RoomTypeToBookDTO>();
		public List<int> RoomsIDs { get; set; } = new List<int>();
	}
	public class RoomTypeToBookDTO
	{
		public int RoomTypeId { get; set; }
		public int NumOfRooms { get; set; }
		public int NumOfAdults { get; set; }
		public int NumOfChildrens { get; set; }
	}
	public class ReservationGuestDTO
	{
		public int GuestId { get; set; }
		public bool IsPrimary { get; set; }
	}
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
