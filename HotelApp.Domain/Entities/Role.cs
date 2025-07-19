using Microsoft.AspNetCore.Identity;

namespace HotelApp.Domain.Entities
{
    public class Role : IdentityRole<int>
    {
        public bool IsBasic { get; set; } = false;
        public bool IsActive { get; set; }

        public int? CreatedById { get; set; }
        public User? CreatedBy { get; set; }

        public int? LastModifiedById { get; set; }
        public User? LastModifiedBy { get; set; }

        public DateTime? CreatedDate { get; set; }
        public DateTime? LastModifiedDate { get; set; }
    }
}
