using HotelApp.Application.DTOs.Reservation;
using HotelApp.Domain.Entities;

namespace HotelApp.Application.Services.ReservationService
{
    public interface IReservationService
    {
        Task<IEnumerable<GetAllReservationsDTO>> GetAllReservationAsync();
        Task<IEnumerable<GetAllReservationsDTO>> GetFilteredReservationsAsync(ReservationFilterDTO dto);
        Task<GetReservationDetailsByIdDTO> GetReservationDetailsByIdAsync(int Id);
        Task<ServiceResponse<object>> ChangeReservationDatesAsync(ChangeReservationDatesDTO dto);
        Task<ReservationDTO> GetReservationToEditByIdAsync(int Id);
        Task<ServiceResponse<ReservationDTO>> SaveReservation(ReservationDTO dto, int BranchId, int UserId);
        Task<ServiceResponse<string>> QuickCheckInByIdAsync(int Id, int UserId);
        Task<CheckOutDTO> GetCheckOutReservationByIdAsync(int Id);
        Task<ServiceResponse<CheckOutDTO>> CheckOutReservationAsync(CheckOutDTO dto, int UserId);
    }
}
