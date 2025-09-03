using HotelApp.Domain.Common;
using HotelApp.Domain.Enums;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HotelApp.Domain.Entities
{
    public class Reservation : BaseEntity, IHasBranch
	{
        public int Id { get; set; }
        public DateTime CheckInDate { get; set; }
        public DateTime CheckOutDate { get; set; }
        public int NumberOfNights { get; set; }
        public int NumberOfPeople { get; set; }
        public decimal PricePerNight { get; set; }
        public decimal TotalPrice { get; set; }
        public string? Comment { get; set; }

		public bool IsConfirmed { get; set; }
		public bool IsPending { get; set; }
		public bool IsCheckedIn { get; set; }
		public bool IsCheckedOut { get; set; }
		public bool IsClosed { get; set; }
		public bool IsCancelled { get; set; }
		public string? CancellationReason { get; set; }

        public int ReservationSourceId { get; set; }
        public ReservationSource? ReservationSource { get; set; }

        public int? CompanyId { get; set; }
        public Company? Company { get; set; }

        public int BranchId { get; set; }
        public Branch? Branch { get; set; }

		public ICollection<ReservationRoomType> ReservationRoomTypes { get; set; } = new HashSet<ReservationRoomType>();
		public ICollection<GuestReservation> guestReservations { get; set; } = new HashSet<GuestReservation>();
        public ICollection<ReservationRoom> ReservationsRooms { get; set; } = new HashSet<ReservationRoom>();

    }
}
