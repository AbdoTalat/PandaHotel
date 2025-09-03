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
        //public List<AvailableRoomsDTO> availableRoomsDTOs { get; set; } = new List<AvailableRoomsDTO>();
    }
    public class AvailableRoomsDTO
    {
        public int Id { get; set; }
        public string RoomNumber { get; set; }
    }
}
