using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HotelApp.Application.DTOs.Company
{
    public class EditCompanyDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Phone { get; set; }
        public string Address { get; set; }

        [MaxLength(100, ErrorMessage = "Max Length is 100")]
        public string? Notes { get; set; }
        [EmailAddress]
        public string Email { get; set; }
        public bool IsActive { get; set; }
    }
}
