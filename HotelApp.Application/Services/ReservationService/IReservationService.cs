using HotelApp.Application.DTOs.Reservation;
using HotelApp.Domain.Entities;

namespace HotelApp.Application.Services.ReservationService
{
    public interface IReservationService
    {
        Task<IEnumerable<GetAllReservationsDTO>> GetAllReservationAsync();
        Task<ServiceResponse<ReservationDTO>> AddReservation(ReservationDTO reservationDTO);
        Task<IEnumerable<GetAllReservationsDTO>> GetFilteredReservationsAsync(ReservationFilterDTO dto);
        Task<Reservation> GetReservationDetailsByIdAsync(int Id);
    }
}
