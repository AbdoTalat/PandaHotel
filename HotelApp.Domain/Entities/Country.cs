using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HotelApp.Domain.Entities
{
    public class Country
    {
        public int Id { get; set; }
        public string ShortName { get; set; }
        public string Name { get; set; }
        public int PhoneCode { get; set; }

        public ICollection<State> States { get; set; } = new HashSet<State>();
        public ICollection<Branch> Branches { get; set; } = new HashSet<Branch>();
    }
}
