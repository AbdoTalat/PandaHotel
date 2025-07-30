using HotelApp.Application.DTOs;
using HotelApp.Application.DTOs.Reservation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HotelApp.Application.Services.ReservationSourceService
{
    public interface IReservationSourceService
    {
        Task<IEnumerable<DropDownDTO<string>>> GetReservationSourcesDropDownAsync();
    }
}
