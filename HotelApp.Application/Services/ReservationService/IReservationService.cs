using HotelApp.Application.DTOs.Reservation;

namespace HotelApp.Application.Services.ReservationService
{
    public interface IReservationService
    {
        Task<IEnumerable<GetAllReservationsDTO>> GetAllReservationAsync();
        Task<ServiceResponse<AddReservationDTO>> AddReservation(AddReservationDTO reservationDTO, int userId);
       // public Task<GetReservationDetailsById> GetReservationDetailsById(int Id);
       Task<IEnumerable<GetAllReservationsDTO>> GetFilteredReservationsAsync(ReservationFilterDTO dto);
    }
}
