using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HotelApp.Application.DTOs.Rates
{
    public class RatesForReservationRequestDTO
    {
        public List<int> RoomTypeIds { get; set; } = new List<int>();
        public DateTime CheckInDate { get; set; }
        public DateTime CheckOutDate { get; set; }
    }
}
