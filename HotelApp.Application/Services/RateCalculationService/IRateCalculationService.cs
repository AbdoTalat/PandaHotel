using HotelApp.Application.DTOs.RateCalculation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HotelApp.Application.Services.RateCalculationService
{
	public interface IRateCalculationService
	{
        Task<ServiceResponse<GetRateCalculationDTOResponse>> GetRateCalculation(GetRateCalculationDTORequest request);
    }
}
