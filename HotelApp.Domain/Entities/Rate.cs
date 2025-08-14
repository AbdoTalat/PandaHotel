using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HotelApp.Domain.Common;

namespace HotelApp.Domain.Entities
{
    public class Rate : BaseEntity, IHasBranch
	{
        public int Id { get; set; }
        public string Code { get; set; }
        public DateTime EffectiveDate { get; set; }
        public DateTime EndDate { get; set; }
        public int MinChargeDayes { get; set; }
        public bool IsActive { get; set; }
        public bool SkipHourly { get; set; }
        public bool DisplayOnline { get; set; }
        public int BranchId { get; set; }
        public Branch? Branch { get; set; }
        public ICollection<RoomTypeRate> RoomTypeRates { get; set; } = new HashSet<RoomTypeRate>();
    }
}
