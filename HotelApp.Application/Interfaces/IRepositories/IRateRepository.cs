using HotelApp.Application.DTOs.Rates;
using HotelApp.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HotelApp.Application.Interfaces.IRepositories
{
    public interface IRateRepository : IGenericRepository<Rate>
    {
        Task<RateDTO> GetRateToEditByIdAsync(int Id);
        Task<IEnumerable<GetRatesForReservationResponseDTO>> GetRatesForReservationAsync(RatesForReservationRequestDTO model);

    }
}
