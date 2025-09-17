using HotelApp.Application.DTOs.Rates;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HotelApp.Application.IRepositories
{
    public interface IRateRepository
    {
        Task<RateDTO> GetRateToEditByIdAsync(int Id);
        Task<IEnumerable<GetRatesForReservationResponseDTO>> GetRatesForReservationAsync(RatesForReservationRequestDTO model);

	}
}
