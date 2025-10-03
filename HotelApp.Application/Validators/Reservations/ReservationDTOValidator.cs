using FluentValidation;
using HotelApp.Application.DTOs.Reservation;
using HotelApp.Application.Validators.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HotelApp.Application.Validators.Reservations
{
    public class ReservationDTOValidator : AbstractValidator<ReservationDetailsDTO>
    {
        public ReservationDTOValidator()
        {

            RuleFor(x => x.CheckInDate)
                        .LessThan(x => x.CheckOutDate)
                        .WithMessage("Check-in must be before check-out.");
        }
    }
}
