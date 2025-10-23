using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HotelApp.Domain.Entities
{
    public class ReservationRoom
    {
        public int Id { get; set; }

        public int ReservationId { get; set; }
        public Reservation? Reservation { get; set; }

        public int RoomId { get; set; }
        public Room? Room { get; set; }

        public bool IsLastRoom { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
    }
}
