using HotelApp.Helper.Validation;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HotelApp.Application.DTOs.Guests
{
    public class AddGuestDTO 
	{
        public int Id { get; set; }

        [RequiredEx]
        public string FullName { get; set; }
        public bool IsPrimary { get; set; }
        public DateOnly? DateOfBirth { get; set; }
        public string? Address { get; set; }
        public string? Phone { get; set; }

        [EmailAddress]
        public string? Email { get; set; }
		public string? TypeOfProof { get; set; }
		public string? ProofNumber { get; set; }
        public int BranchId { get; set; }

    }
}
