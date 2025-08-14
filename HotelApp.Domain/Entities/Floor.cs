using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HotelApp.Domain.Common;

namespace HotelApp.Domain.Entities
{
    public class Floor : BaseEntity, IHasBranch
    {
        public int Id { get; set; }
        public int Number { get; set; }
        public bool IsActive { get; set; }

        public int BranchId { get; set; }
        public Branch? Branch { get; set; }

        public ICollection<Room> Rooms { get; set; } = new HashSet<Room>();
    }
}
