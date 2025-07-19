using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HotelApp.Application.DTOs.Branches
{
    public class GetBranchByIdDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }
        [Required(ErrorMessage = "Country Is Required")]
        public int CountryId { get; set; }
        [Required(ErrorMessage = "State Is Required")]
        public int StateId { get; set; }
        //public string CountryName { get; set; }
        //public string StateName { get; set; }
        public string City { get; set; }
        public string Street { get; set; }
        public string Zip_Code { get; set; }
        public string ContactNumber { get; set; }
        public bool IsActive { get; set; }
    }
}
