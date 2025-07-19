using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HotelApp.Domain.Entities.RoleBased
{
    public class Entity
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public ICollection<Permission> Permissions { get; set; } = new HashSet<Permission>();
    }
}
