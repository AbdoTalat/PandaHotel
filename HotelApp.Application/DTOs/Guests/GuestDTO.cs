using HotelApp.Application.Services.SystemSettingService;
using HotelApp.Domain.Common.Validation;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HotelApp.Application.DTOs.Guests
{
    public class GuestDTO 
	{
        public int Id { get; set; }

        [RequiredEx]
        [MaxLengthEx(60)]
        public string FullName { get; set; }
        public bool IsPrimary { get; set; }
        public DateOnly? DateOfBirth { get; set; }
        public string? Address { get; set; }
        public string? Phone { get; set; }

        [EmailAddress]
        public string? Email { get; set; }
		public int? ProofTypeId { get; set; }
		public string? ProofNumber { get; set; }
        public int? BranchId { get; set; }
    }
}
