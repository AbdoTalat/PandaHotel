using HotelApp.Domain.Common.Validation;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HotelApp.Application.DTOs.Branches
{
    public class BranchDTO
    {
        public int Id { get; set; }
        [RequiredEx]
        public string Name { get; set; }
        [RequiredEx]
		public int CountryId { get; set; }
        [RequiredEx]
		public int StateId { get; set; }
        [RequiredEx]
        public string City { get; set; }
        [RequiredEx]
		public string Street { get; set; }
        [RequiredEx]
		public string Zip_Code { get; set; }
        [RequiredEx]
		public string ContactNumber { get; set; }
        public bool IsActive { get; set; }
    }
}
