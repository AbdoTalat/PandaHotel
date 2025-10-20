using HotelApp.Application.DTOs.Reservation;
using HotelApp.Domain.Entities;

namespace HotelApp.Application.Services.ReservationService
{
    public interface IReservationService
    {
        Task<IEnumerable<GetAllReservationsDTO>> GetAllReservationAsync();
        Task<ServiceResponse<ReservationDTO>> AddReservation(ReservationDTO dto, int UserId);
        Task<IEnumerable<GetAllReservationsDTO>> GetFilteredReservationsAsync(ReservationFilterDTO dto);
        Task<GetReservationDetailsByIdDTO> GetReservationDetailsByIdAsync(int Id);
        Task<ServiceResponse<object>> ChangeReservationDatesAsync(ChangeReservationDatesDTO dto);

        Task<ReservationDTO?> GetReservationToEditByIdAsync(int Id);
    }
}
