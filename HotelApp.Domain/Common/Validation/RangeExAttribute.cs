using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace HotelApp.Domain.Common.Validation
{
    public class RangeExAttribute : RangeAttribute
    {
        public RangeExAttribute(double min, double max) : base(min, max)
        {
            ErrorMessage = $"Value must be between {min} and {max}.";
        }
	}
}
