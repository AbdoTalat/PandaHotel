using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HotelApp.Application.DTOs.RoomTypes
{
    public class RoomTypeAvailabilityRequestDTO
    {
        public int BranchId { get; set; } = 0;
        public int? ReservationId { get; set; } = 0;
        public int? Quantity { get; set; } = 0;
        public int? RoomTypeId { get; set; } = 0;
        public DateTime CheckInDate { get; set; }
        public DateTime CheckOutDate { get; set; }
    }
    public class RoomTypeAvailabilityResultDTO
    {
        public int RoomTypeId { get; set; }
        public int TotalAvailableRooms { get; set; }
    }

}
