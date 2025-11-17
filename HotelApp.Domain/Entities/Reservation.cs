using HotelApp.Domain.Common;
using HotelApp.Domain.Enums;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace HotelApp.Domain.Entities
{
    public class Reservation : BaseEntity, IHasBranch
	{
        public int Id { get; set; }
        public string? ReservationNumber { get; set; }
        public ReservationStatus Status { get; set; } = ReservationStatus.Pending;
        public DateTime CheckInDate { get; set; }
        public DateTime CheckOutDate { get; set; }
        public int NumberOfNights { get; set; }
        public int NumberOfPeople { get; set; }
        public decimal PricePerNight { get; set; }
        public decimal TotalPrice { get; set; }

        public string? Notes { get; set; }
        public string? CancellationReason { get; set; }

        public int ReservationSourceId { get; set; }
        public ReservationSource? ReservationSource { get; set; }

        public int? CompanyId { get; set; }
        public Company? Company { get; set; }

        public int? RateId { get; set; }
        public Rate? Rate { get; set; }
        public int? BranchId { get; set; }
        public Branch? Branch { get; set; }

		public ICollection<ReservationRoomType> ReservationRoomTypes { get; set; } = new HashSet<ReservationRoomType>();
		public ICollection<GuestReservation> guestReservations { get; set; } = new HashSet<GuestReservation>();
        public ICollection<ReservationRoom> ReservationsRooms { get; set; } = new HashSet<ReservationRoom>();
        public ICollection<ReservationHistory> ReservationHistories { get; set; } = new HashSet<ReservationHistory>();
        public ICollection<Payment> Payments { get; set; } = new HashSet<Payment>();

    }

}

namespace HotelApp.Domain.Entities
{
    // NOTE:
    // - Reservation previously had an enum property: `ReservationStatus Status`.
    // - This file replaces that with a FK to the DropdownItems table:
    //     int ReservationStatusId
    //     DropdownItem? ReservationStatusItem
    // - Helper properties/methods are added to preserve common checks used across services/views.
    // - Ensure your EF Core model is configured to map ReservationStatusId -> DropdownItems(Id) (FK).
    // - When applying DB migrations, map existing enum values to DropdownItem rows (seed) and update data accordingly.

    public class Reservations : BaseEntity, IHasBranch
    {
        public int Id { get; set; }

        // Reservation number (unique)
        public string? ReservationNumber { get; set; } // e.g. "R-2025-0001234" UNIQUE

        // Replaced enum with FK to DropdownItem
        public int ReservationStatusId { get; set; }
        public DropdownItem? ReservationStatusItem { get; set; }

        // Convenience accessors to preserve previous enum-based checks
        // StatusKey is the identifier (Key) on the DropdownItem row (e.g. "Pending", "Confirmed", "Cancelled")
        public string? StatusKey => ReservationStatusItem?.Key;

        public bool IsPending => string.Equals(StatusKey, "Pending", StringComparison.OrdinalIgnoreCase);
        public bool IsConfirmed => string.Equals(StatusKey, "Confirmed", StringComparison.OrdinalIgnoreCase);
        public bool IsCancelled => string.Equals(StatusKey, "Cancelled", StringComparison.OrdinalIgnoreCase);
        public bool IsCheckedIn => string.Equals(StatusKey, "CheckedIn", StringComparison.OrdinalIgnoreCase);
        public bool IsCheckedOut => string.Equals(StatusKey, "CheckedOut", StringComparison.OrdinalIgnoreCase);

        // Safe setter that updates FK and nav prop
        public void SetStatus(DropdownItem? item)
        {
            if (item is null)
            {
                ReservationStatusId = 0;
                ReservationStatusItem = null;
                return;
            }

            ReservationStatusId = item.Id;
            ReservationStatusItem = item;
        }

        // Optional helper to set by key (requires a lookup from service)
        public void SetStatusByKey(string? key, DropdownItem? resolvedItem)
        {
            // resolvedItem should be the DropdownItem whose Key matches `key`.
            SetStatus(resolvedItem);
        }

        // Preserve other reservation fields
        public DateTime CheckInDate { get; set; }
        public DateTime CheckOutDate { get; set; }
        public int NumberOfNights { get; set; }
        public int NumberOfPeople { get; set; }
        public decimal PricePerNight { get; set; }
        public decimal TotalPrice { get; set; }

        public string? Notes { get; set; }
        public string? CancellationReason { get; set; }

        public int ReservationSourceId { get; set; }
        public ReservationSource? ReservationSource { get; set; }

        public int? CompanyId { get; set; }
        public Company? Company { get; set; }

        public int? RateId { get; set; }
        public Rate? Rate { get; set; }
        public int? BranchId { get; set; }
        public Branch? Branch { get; set; }

        public ICollection<ReservationRoomType> ReservationRoomTypes { get; set; } = new HashSet<ReservationRoomType>();
        public ICollection<GuestReservation> guestReservations { get; set; } = new HashSet<GuestReservation>();
        public ICollection<ReservationRoom> ReservationsRooms { get; set; } = new HashSet<ReservationRoom>();
        public ICollection<ReservationHistory> ReservationHistories { get; set; } = new HashSet<ReservationHistory>();
        public ICollection<Payment> Payments { get; set; } = new HashSet<Payment>();
    }

    // Lightweight reference to DropdownItem entity.
    // If you already have a `DropdownItem` entity in your project, remove/ignore this stub.
    public class DropdownItem
    {
        public int Id { get; set; }
        public string? Key { get; set; }    // use this to identify status values like "Pending", "Cancelled", etc.
        public string? Value { get; set; }  // human-readable text
        public string? Type { get; set; }   // optional, e.g. "ReservationStatus"
    }
}