using HotelApp.Domain.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HotelApp.Domain.Entities
{
    public class Company : BaseEntity, IHasBranch
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Phone { get; set; }
        public string Address { get; set; }
        public string Email { get; set; }
        public string? Notes { get; set; }
        public bool IsActive { get; set; }

        public int BranchId { get; set; }
        public Branch? Branch { get; set; }

        public ICollection<Reservation> Reservations { get; set; } = new HashSet<Reservation>();
    }
}
