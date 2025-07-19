using Microsoft.AspNetCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HotelApp.Application.DTOs.Users
{
    public class AddUserDTO
    {
        [MinLength(3)]
        [Required(ErrorMessage = "Please your first name")]
        public string FirstName { get; set; }

        [MinLength(3)]
        [Required]
        public string LastName { get; set; }
        [Required]
        [MinLength(3, ErrorMessage = "User Name must be at least 3 characters")]
        public string userName { get; set; }

        [Required]
        [EmailAddress]
        public string email { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string password { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Compare("password")]
        public string confirmPassword { get; set; }
        public int DefaultBranchId { get; set; }
        public bool IsActive { get; set; }

        public int? RoleId { get; set; }
        public List<string> AvailableRoles { get; set; } = new List<string>();

        public List<string?>? SelectedRoles { get; set; }
    }
}
