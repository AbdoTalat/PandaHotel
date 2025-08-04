using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HotelApp.Helper.Validation
{
    public class RequiredExAttribute : System.ComponentModel.DataAnnotations.RequiredAttribute
    {
        public RequiredExAttribute()
        {
            ErrorMessage = "This field is required.";
        }
    }
}
