using HotelApp.Domain.Common.Validation;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HotelApp.Application.DTOs.Company
{
    public class ReservationCompanyDTO
    {
        public int Id { get; set; }
        [RequiredEx]
        public string Name { get; set; }
		[RequiredEx]
		public string Phone { get; set; }
		[RequiredEx]
		public string Address { get; set; }
		[RequiredEx]
        [EmailAddress]
		public string Email { get; set; }
        public bool IsActive { get; set; } = true;
        public string? Notes { get; set; }
    }
}
