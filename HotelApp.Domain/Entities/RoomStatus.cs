using HotelApp.Domain.Common;
using HotelApp.Domain.Enums;

namespace HotelApp.Domain.Entities
{
    public class RoomStatus : BaseEntity, IHasBranch
	{
        public int Id { get; set; }
        public string Name { get; set; }
        public RoomStatusEnum? Code { get; set; }
        public string? Description { get; set; }
        public string Color { get; set; }
        public bool IsReservable { get; set; }
        public bool IsActive { get; set; }
        public bool IsSystem { get; set; }

        public int? BranchId { get; set; }
        public Branch? Branch { get; set; }
        public ICollection<Room> Rooms { get; set; } = new HashSet<Room>();

    }
}
