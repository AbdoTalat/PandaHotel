using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace HotelApp.Helper.Validation
{
    public class RequiredExAttribute : RequiredAttribute
    {
        public RequiredExAttribute()
        {
            ErrorMessage = "This field is required.";
        }
    }
}
