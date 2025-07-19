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
        Task<IEnumerable<GetRateDetailsForReservationDTO>> GetRateDetailsForReservation(int rateId);
        Task<EditRateDTO?> GetRateToEditByIdAsync(int Id);
        Task<ServiceResponse<AddRateDTO>> AddRateAsync(AddRateDTO rateDTO);
        Task<ServiceResponse<EditRateDTO>> EditRateAsync(EditRateDTO rateDTO);
    }
}
