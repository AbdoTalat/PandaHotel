using HotelApp.Domain.Common;
using HotelApp.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HotelApp.Application.DTOs.Payment
{
    public class GetPaymentDTO
    {
        public int Id { get; set; }
        public decimal Amount { get; set; }
        public DateTime PaymentDate { get; set; }
        public string? Notes { get; set; }

        //public string GuestName { get; set; }
        public string PaymentMethod { get; set; }
        public string TransactionType { get; set; }
    }
}
