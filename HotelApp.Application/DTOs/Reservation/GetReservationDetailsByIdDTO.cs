using HotelApp.Domain.Entities;
using HotelApp.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HotelApp.Application.DTOs.Reservation
{
    public class GetReservationDetailsByIdDTO
	{
		public int Id { get; set; }
		public string ReservationNumber { get; set; }
        public DateTime CheckInDate { get; set; }
		public DateTime CheckOutDate { get; set; }
		public ReservationStatus Status { get; set; }
        public int NumberOfNights { get; set; }
		public int NumberOfPeople { get; set; }
		public decimal TotalPrice { get; set; }
		public string ReservationSource { get; set; }
		public string Notes { get; set; }
		public string CreatedBy { get; set; }
		public string LastModifiedBy { get; set; }

		public int NumOfTotalRooms { get; set; }

        public List<GuestsDTO> GuestReservations { get; set; } = new List<GuestsDTO>();
		public List<ReservationRoomTypeDTO> ReservationRoomTypes { get; set; } = new List<ReservationRoomTypeDTO>();
        public List<RoomsDTO> ReservationRooms { get; set; } = new List<RoomsDTO>();
        public List<ReservationHistoryDTO> ReservationHistories { get; set; } = new List<ReservationHistoryDTO>();
	}

	public class GuestsDTO
	{
		public int Id { get; set; }
		public bool IsPrimaryGuest { get; set; }
        public string FullName { get; set; }
        public string? Phone { get; set; }
        public string? Email { get; set; }
    }
	public class ReservationHistoryDTO
	{
        public string PerformedByName { get; set; }
		//public string PerformedByRole { get; set; }
        public DateTime PerformedDate { get; set; }
        public string Status { get; set; }
    }
	public class ReservationRoomTypeDTO
	{
		public int Id { get; set; }
		public string RoomTypeName { get; set; }
		public int Quantity { get; set; }
		public int NumOfAdults { get; set; }
		public int NumOfChildren { get; set; }
	}
	public class RoomsDTO
	{
		public string RoomNumber { get; set; }
		public int RoomTypeId { get; set; }
	}
}
