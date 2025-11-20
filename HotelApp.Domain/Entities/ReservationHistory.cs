using HotelApp.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HotelApp.Domain.Entities
{
    public class ReservationHistory
    {
        public int Id { get; set; }

        public int ReservationId { get; set; }
        public Reservation? Reservation { get; set; }

        public int PerformedById { get; set; }
        public User? PerformedBy { get; set; }

        public DateTime PerformedDate { get; set; }
        public ReservationStatus Status { get; set; } = ReservationStatus.Pending;
    }
}
