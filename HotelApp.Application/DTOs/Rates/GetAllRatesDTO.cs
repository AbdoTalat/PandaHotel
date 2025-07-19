using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HotelApp.Application.DTOs.Rates
{
    public class GetAllRatesDTO
    {
        public int Id { get; set; }
        public string Code { get; set; }
        public int MinChargeDayes { get; set; }
        public bool IsActive { get; set; }
    }
}
