using HotelApp.Domain.Common.Validation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HotelApp.Application.DTOs.Floor
{
    public class FloorDTO
    {
        public int Id { get; set; }
        [RequiredEx]
        public Int16 Number {  get; set; }
        public bool IsActive { get; set; }
    }
}
