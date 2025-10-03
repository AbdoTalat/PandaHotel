using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HotelApp.Domain.Common;

namespace HotelApp.Domain.Entities
{
    public class SystemSetting : BaseEntity
    {
        public int Id { get; set; }

        public int CheckInStatusId { get; set; }
        public RoomStatus? CheckInStatus { get; set; }

        public int CheckOutStatusId { get; set; }
        public RoomStatus? CheckOutStatus { get; set; }

        public int? CalculationTypeId { get; set; }
        public CalculationType? CalculationType { get; set; }


        public bool IsGuestEmailRequired { get; set; }
        public bool IsGuestDateOfBirthRequired { get; set; }
        public bool IsGuestPhoneRequired { get; set; }
        public bool IsGuestAddressRequired { get; set; }
        public bool IsGuestProofTypeRequired { get; set; }
        public bool IsGuestProofNumberRequired { get; set; }
    }
}
