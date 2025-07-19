using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HotelApp.Domain.Entities
{
    public class RoomTypeRate
    {
        public int Id { get; set; }
        public decimal HourlyPrice { get; set; }
        public decimal DailyPrice { get; set; }
        public decimal ExtraDailyPrice { get; set; }
        public decimal WeeklyPrice { get; set; }
        public decimal MonthlyPrice { get; set; }

        public int RoomTypeId { get; set; }
        public RoomType? RoomType { get; set; }

        public int RateId { get; set; }
        public Rate? Rate { get; set; }
    }
}
