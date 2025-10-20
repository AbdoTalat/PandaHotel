using HotelApp.Domain.Common;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace HotelApp.Domain.Entities
{
    public class User : IdentityUser<int>
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public bool IsActive { get; set; }

        public int? DefaultBranchId { get; set; }
        public Branch? DefaultBranch { get; set; }

        public int? CreatedById { get; set; }
        public User? CreatedBy { get; set; }
        public int? LastModifiedById { get; set; }
        public User? LastModifiedBy { get; set; }

        public DateTime? CreatedDate { get; set; }
        public DateTime? LastModifiedDate { get; set; }

        public ICollection<UserBranch> userBranches { get; set; } = new HashSet<UserBranch>();
        public ICollection<ReservationHistory> ReservationHistories { get; set; } = new HashSet<ReservationHistory>();        
    }
}
