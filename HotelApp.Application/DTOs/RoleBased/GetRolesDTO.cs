using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HotelApp.Application.DTOs.RoleBased
{
    public class GetRolesDTO
    {
        public string Id { get; set; }
        public string Name { get; set; }

        public DateTime CreatedDate { get; set; }
		public DateTime LastModifiedDate { get; set; }
        public bool IsActive { get; set; }
		public bool IsBasic { get; set; }
    }
}
