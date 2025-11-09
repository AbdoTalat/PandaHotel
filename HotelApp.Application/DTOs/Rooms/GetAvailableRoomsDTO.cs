using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HotelApp.Application.DTOs.Rooms
{
    public class GetAvailableRoomsDTO
    {
        public int Id { get; set; }
        public string RoomNumber { get; set; }
        public string RoomTypeName { get; set; }
    }
    public class GetRoomsForEditReservationDTO
    {
        public int Id { get; set; }
        public string RoomNumber { get; set; }
        public string RoomTypeName { get; set; }
    }
}
