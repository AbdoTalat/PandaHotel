using HotelApp.Application.DTOs.Reservation;

namespace HotelApp.Application.Services.ReservationService
{
    public interface IReservationService
    {
        Task<IEnumerable<GetAllReservationsDTO>> GetAllReservationAsync();
        Task<ServiceResponse<AddReservationDTO>> AddReservation(AddReservationDTO reservationDTO);
       Task<IEnumerable<GetAllReservationsDTO>> GetFilteredReservationsAsync(ReservationFilterDTO dto);
    }
}
