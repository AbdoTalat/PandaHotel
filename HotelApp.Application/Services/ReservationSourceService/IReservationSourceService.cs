using HotelApp.Application.DTOs;
using HotelApp.Application.DTOs.ReservationSource;
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

		Task<IEnumerable<ReservationSourceDTO>> GetAllReservationSourcesAsync();
		Task<ServiceResponse<ReservationSourceDTO>> AddReservationSourceAsync(ReservationSourceDTO dto);
		Task<ReservationSourceDTO?> GetReservationSourceToEditByIdAsync(int Id);
		Task<ServiceResponse<ReservationSourceDTO>> EditReservationSourceAsync(ReservationSourceDTO dto);
		Task<ServiceResponse<object>> DeleteReservationSourceAsync(int Id);
	}
}
