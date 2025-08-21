using HotelApp.Domain.Common;

namespace HotelApp.Domain.Entities
{
    public class Room : BaseEntity, IHasBranch
	{
        public int Id { get; set; }
        public string RoomNumber { get; set; }
        public string? Description { get; set; }
        public decimal PricePerNight { get; set; }
        public int MaxNumOfAdults { get; set; }
        public int MaxNumOfChildrens { get; set; }
        public bool IsActive { get; set; }
        public bool IsAffectedByRoomType { get; set; }

        public int RoomTypeId { get; set; }
        public RoomType? RoomType { get; set; }

        public int BranchId { get; set; }
        public Branch? Branch { get; set; }

        public int? RoomStatusId { get; set; }
        public RoomStatus? RoomStatus { get; set; }

        public int FloorId { get; set; }
        public Floor? Floor { get; set; }

        public ICollection<RoomOption> RoomOptions { get; set; } = new HashSet<RoomOption>();

    }
}
