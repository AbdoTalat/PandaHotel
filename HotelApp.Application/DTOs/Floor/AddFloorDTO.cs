using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HotelApp.Application.DTOs.Floor
{
    public class AddFloorDTO
    {
        public Int16 Number { get; set; }
        public int BranchId { get; set; }

        public bool IsActive { get; set; }
    }
}
