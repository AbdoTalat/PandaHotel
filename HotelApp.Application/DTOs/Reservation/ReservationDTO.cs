using HotelApp.Application.DTOs.Guests;
using HotelApp.Domain.Common.Validation;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore.Metadata;
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
        public List<ReservationGuestDTO> GuestDtos { get; set; } = new List<ReservationGuestDTO>();
		public ReservationInfoDTO ReservationInfoDto { get; set; } = new ReservationInfoDTO();
		public ConfirmReservationDTO ConfirmDto { get; set; } = new ConfirmReservationDTO();
	}

	public class ReservationInfoDTO
	{
        public IEnumerable<SelectListItem> ReservationSources { get; set; } = new List<SelectListItem>();
        public int ReservationId { get; set; }
		public string ReservationNumber { get; set; }
		[Required]
        public DateTime? CheckInDate { get; set; }
		[Required]
		public DateTime? CheckOutDate { get; set; }
		public int NumOfNights { get; set; }
		public int RateId { get; set; }
		public int ReservationSourceId { get; set; }
		public int? CompanyId { get; set; }

		public List<RoomTypeToBookDTO> RoomTypeToBookDTOs { get; set; } = new List<RoomTypeToBookDTO>();
	}
    public class RoomTypeToBookDTO
    {
        public int RoomTypeId { get; set; }
        public int NumOfRooms { get; set; }
        public int NumOfAdults { get; set; }
        public int NumOfChildrens { get; set; }
        public List<int> RoomIds { get; set; } = new List<int>(); 
    }
    public class ReservationGuestDTO
	{
		public int GuestId { get; set; }
		public bool IsPrimary { get; set; }
		public string FullName { get; set; }
		public string? Email { get; set; }
		public string? Phone { get; set; }
		public string? ProofNumber { get; set; }
		public DateOnly? DateOfBirth { get; set; }
        public string? Address { get; set; }
        public int? ProofTypeId { get; set; }

    }
	public class ConfirmReservationDTO
	{
		[MaxLength(100)]
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
