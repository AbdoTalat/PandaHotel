using HotelApp.Domain.Common;

namespace HotelApp.Domain.Entities
{
    public class RoomStatus : BaseEntity, IBranchEntity
	{
        public int Id { get; set; }
        public string Name { get; set; }
        public string? Description { get; set; }
        public string Color { get; set; }
        public bool IsReservable { get; set; }
        public bool IsActive { get; set; }

        public int BranchId { get; set; }
        public Branch? Branch { get; set; }
        public ICollection<Room> Rooms { get; set; } = new HashSet<Room>();

    }
}
