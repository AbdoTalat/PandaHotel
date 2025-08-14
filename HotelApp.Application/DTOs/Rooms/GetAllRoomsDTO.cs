using System;
using System.Collections.Generic;
using System.Diagnostics.SymbolStore;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HotelApp.Application.DTOs.Rooms
{
    public class GetAllRoomsDTO
    {
        public int Id { get; set; }
        public string RoomNumber { get; set; }
        public string RoomStatusName { get; set; }
        public string RoomStatusColor { get; set; }
        public int Floor { get; set; }
        public string TypeName { get; set; }
        public int MaxNumOfAdults { get; set; }
		public int MaxNumOfChildren { get; set; }

        public bool IsActive { get; set; }
	}
}
