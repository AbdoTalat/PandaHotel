using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HotelApp.Domain.Entities
{
    public class GuestReservation
    {
        public int Id { get; set; }

        public int GuestId { get; set; }
        public Guest? Guest { get; set; }

        public int ReservationId { get; set; }
        public Reservation? Reservation { get; set; }

        public bool IsPrimaryGuest { get; set; }
    }
}
