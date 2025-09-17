using HotelApp.Application.DTOs.Rates;
using HotelApp.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HotelApp.Application.Services.RateService
{
    public interface IRateService
    {
        Task<IEnumerable<GetAllRatesDTO>> GetAllRatesAsync();
        Task<IEnumerable<GetRatesForReservationResponseDTO>> GetRatesForReservationAsync(RatesForReservationRequestDTO model);
        Task<IEnumerable<GetRateDetailsForReservationDTO>> GetRateDetailsForReservation(int rateId, int BranchId);
        Task<RateDTO?> GetRateToEditByIdAsync(int Id);
        Task<ServiceResponse<RateDTO>> AddRateAsync(RateDTO rateDTO);
        Task<ServiceResponse<RateDTO>> EditRateAsync(RateDTO rateDTO);
    }
}
