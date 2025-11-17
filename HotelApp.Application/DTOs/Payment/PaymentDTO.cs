using HotelApp.Domain.Entities;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HotelApp.Application.DTOs.Payment
{
    public class PaymentDTO
    {
        public int Id { get; set; }
        [Range(0, double.MaxValue, ErrorMessage = "Amount must be greater than zero.")]
        public decimal Amount { get; set; }
        public DateTime PaymentDate { get; set; }
        public string? Notes { get; set; }

        public int GuestId { get; set; }
        public int ReservationId { get; set; }
        public int PaymentMethodId { get; set; }
        public int TransactionTypeId { get; set; }

        public IEnumerable<SelectListItem> TransactionTypes { get; set; } = new List<SelectListItem>();
        public IEnumerable<SelectListItem> PaymentMethods { get; set; } = new List<SelectListItem>();
    }
}
