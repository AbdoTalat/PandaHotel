using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HotelApp.Domain.Entities
{
    public class RoomOption
    {
        public int Id { get; set; }
        public int RoomId { get; set; }
        public Room? Room { get; set; }

        public int OptionId { get; set; }
        public Option? Option { get; set; }
    }
}
