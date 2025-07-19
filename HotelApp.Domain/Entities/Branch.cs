using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HotelApp.Domain.Common;

namespace HotelApp.Domain.Entities
{
    public class Branch : BaseEntity
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string City { get; set; }
        public string Street { get; set; }
        public string Zip_Code { get; set; }
        public string ContactNumber { get; set; }
        public bool IsActive { get; set; }

        public int? CountryId { get; set; }
        public Country? Country { get; set; }
        public int? StateId { get; set; }
        public State? State { get; set; }


        public ICollection<Room> Rooms { get; set; } = new HashSet<Room>();
        public ICollection<UserBranch> userBranches { get; set; } = new HashSet<UserBranch>();
        public ICollection<Floor> Floors { get; set; } = new HashSet<Floor>();
        public ICollection<Reservation> Reservations { get; set; } = new HashSet<Reservation>();
        public ICollection<Guest> Guests { get; set; } = new HashSet<Guest>();
        public ICollection<RoomType> RoomTypes { get; set; } = new HashSet<RoomType>();
        public ICollection<RoomStatus> RoomStatuses { get; set; } = new HashSet<RoomStatus>();
        public ICollection<Rate> Rates { get; set; } = new HashSet<Rate>();
        public ICollection<Option> Options { get; set; } = new HashSet<Option>();   
    }
}
