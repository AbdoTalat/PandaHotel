using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HotelApp.Application.Validators.Common
{
    public static class DateRangeValidator
    {
        public static IRuleBuilderOptions<T, DateTime> MustBeBefore<T>(
            this IRuleBuilder<T, DateTime> ruleBuilder,
            Func<T, DateTime> otherDateFunc,
            string errorMessage = "Start date must be before end date")
        {
            return ruleBuilder.Must((x, date, context) => date < otherDateFunc(x))
                              .WithMessage(errorMessage);
        }
    }
}
