using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HotelApp.Domain.Common;

namespace HotelApp.Domain.Entities
{
    public class RoomType : BaseEntity, IHasBranch
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string? Description { get; set; }
        public decimal PricePerNight { get; set; }
        public int MaxNumOfAdults { get; set; }
        public int MaxNumOfChildrens { get; set; }
        public bool IsActive { get; set; }
		public int BranchId { get; set; }
        public Branch? Branch { get; set; }

        public ICollection<ReservationRoomType> ReservationRoomTypes { get; set; } = new HashSet<ReservationRoomType>();
        public ICollection<Room> Rooms { get; set; } = new HashSet<Room>();
        public ICollection<RoomTypeRate> RoomTypeRates { get; set; } = new HashSet<RoomTypeRate>();
    }
}
