using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HotelApp.Domain.Common;

namespace HotelApp.Domain.Entities
{
    public class Guest : BaseEntity, IHasBranch
    {
        public int Id { get; set; }
        public string FullName { get; set; }
        public DateOnly? DateOfBirth { get; set; }
        public string? Address { get; set; }
        public string? Phone { get; set; }
        public string? Email { get; set; }

        public string? TypeOfProof { get; set; }
        public string? ProofNumber { get; set; }

        public int BranchId { get; set; }
        public Branch? Branch { get; set; }

        public ICollection<GuestReservation> guestReservations { get; set; } = new HashSet<GuestReservation>();
    }

}
