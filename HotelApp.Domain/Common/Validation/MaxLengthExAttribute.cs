using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace HotelApp.Domain.Common.Validation
{
    public class MaxLengthExAttribute : MaxLengthAttribute
    {
        public MaxLengthExAttribute(int length) : base(length) 
        {
            ErrorMessage = $"Maximum allowed length is {length} characters.";
        }
    }
}
