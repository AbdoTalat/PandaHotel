using HotelApp.Domain.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HotelApp.Domain.Entities
{
    public class ProofType : BaseEntity
    {
        public int Id { get; set; }
        public string Name { get; set; } 
        public bool IsActive { get; set; }
        public ICollection<Guest> Guests { get; set; } = new HashSet<Guest>();
    }
}
