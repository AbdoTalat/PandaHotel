using HotelApp.Application.DTOs.Reservation;
using HotelApp.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HotelApp.Application.Interfaces.IRepositories
{
    public interface IReservationRepository : IGenericRepository<Reservation>
    {
        Task<ReservationDTO> GetReservationToEditByIdAsync(int Id);
        Task<Reservation> GetReservationDetailsByIds(int Id);
        Task<IEnumerable<GetAllReservationsDTO>> GetFilteredReservationsAsync(ReservationFilterDTO dto);
        Task<GetReservationDetailsByIdDTO> GetReservationDetailsByIdAsync(int Id);
        Task<string> GenerateReservationNumberAsync(int branchId);
    }
}
