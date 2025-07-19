using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HotelApp.Domain.Entities;

namespace HotelApp.Domain.Common
{
    public abstract class BaseEntity
    {
        public int? CreatedById { get; set; }
        public User? CreatedBy { get; set; }

        public int? LastModifiedById { get; set; }
        public User? LastModifiedBy { get; set; }

        public DateTime? CreatedDate { get; set; }
        public DateTime? LastModifiedDate { get; set; }
    }
}
