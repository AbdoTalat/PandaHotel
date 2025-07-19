using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HotelApp.Domain.Entities.RoleBased
{
    public class Permission
    {
        public int Id { get; set; }
        public string Action { get; set; }

        [ForeignKey("Entity")]
        public int EntityId { get; set; }
        public Entity Entity { get; set; }
    }
}
