using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HotelApp.Application.DTOs.RoomTypes
{
    public class AddRoomTypeDTO
    {
        public string Name { get; set; }
        public string? Description { get; set; }
        public decimal PricePerNight { get; set; }
        public int MaxNumOfAdults { get; set; }
        public int MaxNumOfChildrens { get; set; }
        public int BranchId { get; set; }
    }
}
