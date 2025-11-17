using HotelApp.Domain.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HotelApp.Domain.Entities
{
    public class Payment : BaseEntity, IHasBranch
    {
        public int Id { get; set; }
        public decimal Amount { get; set; }
        public DateTime PaymentDate { get; set; }
        public string? Notes { get; set; }

        public int GuestId { get; set; }
        public Guest? Guest { get; set; }

        public int ReservationId { get; set; }
        public Reservation? Reservation { get; set; }

        public int PaymentMethodId { get; set; }
        public MasterDataItem? PaymentMethod { get; set; }

        public int TransactionTypeId { get; set; }
        public MasterDataItem? TransactionType { get; set; }

        public int? BranchId { get; set; }
        public Branch? Branch { get; set; }

    }
}
