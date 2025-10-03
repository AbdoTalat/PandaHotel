using HotelApp.Application.DTOs.RateCalculation;
using HotelApp.Application.Interfaces;
using HotelApp.Domain;
using HotelApp.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HotelApp.Application.Services.RateCalculationService
{
	public class RateCalculationService : IRateCalculationService
	{
		private readonly IUnitOfWork _unitOfWork;

		public RateCalculationService(IUnitOfWork unitOfWork)
        {
			_unitOfWork = unitOfWork;
		}

        public async Task<ServiceResponse<GetRateCalculationDTOResponse>> GetRateCalculation(GetRateCalculationDTORequest request)
        {
            var rate = await _unitOfWork.RateRepository.GetByIdAsync(request.RateId);
			if (rate == null)
			{
				return ServiceResponse<GetRateCalculationDTOResponse>.ResponseFailure("Rate not found.");
			}
			try
			{
				var roomTypesIDs = request.RoomTypeQuantities.Select(rt => rt.RoomTypeId).ToList();
				var roomTypes = await _unitOfWork.RoomTypeRepository.GetAllAsync(rt => roomTypesIDs.Contains(rt.Id));

				var roomTypeRates = await _unitOfWork.RoomTypeRateRepository.GetAllAsync(rtr => rtr.RateId == request.RateId && roomTypesIDs.Contains(rtr.RoomTypeId));

				StayDurationCalculatorDTO stayCalculations = StayDurationCalculator(request.CheckIn, request.CheckOut);
				string[] stayCalculationNames = ["Monthly", "Weekly", "Daily", "Hourly"];

				List<ChargesSummaryDTO> chargesSummaries = new List<ChargesSummaryDTO>();
				decimal totalPrice = 0;

				GetRateCalculationDTOResponse getRateCalculation = new GetRateCalculationDTOResponse();
				foreach (var rtq in request.RoomTypeQuantities)
				{
					var roomTypeRate = roomTypeRates.FirstOrDefault(rt => rt.RoomTypeId == rtq.RoomTypeId);
					var roomType = roomTypes.FirstOrDefault(rt => rt.Id == rtq.RoomTypeId);

					foreach (var stay in stayCalculationNames)
					{
						ChargesSummaryDTO chargesSummary = new ChargesSummaryDTO();
						if (stay == "Monthly" && stayCalculations.Monthly > 0)
						{
							chargesSummary.Charge = roomType.Name + " " + stay;
							chargesSummary.Unit = rtq.Quantity.ToString();
							chargesSummary.Rate = roomTypeRate.MonthlyPrice.ToString();
							chargesSummary.Total = (rtq.Quantity * roomTypeRate.MonthlyPrice).ToString();
							totalPrice += rtq.Quantity * roomTypeRate.MonthlyPrice;
						}
						else if (stay == "Weekly" && stayCalculations.Weekly > 0)
						{
							chargesSummary.Charge = roomType.Name + " " + stay;
							chargesSummary.Unit = rtq.Quantity.ToString();
							chargesSummary.Rate = roomTypeRate.WeeklyPrice.ToString();
							chargesSummary.Total = (rtq.Quantity * roomTypeRate.WeeklyPrice).ToString();
							totalPrice += rtq.Quantity * roomTypeRate.WeeklyPrice;
						}
						else if (stay == "Daily" && stayCalculations.Daily > 0)
						{
							chargesSummary.Charge = roomType.Name + " " + stay;
							chargesSummary.Unit = rtq.Quantity.ToString();
							chargesSummary.Rate = roomTypeRate.DailyPrice.ToString();
							chargesSummary.Total = (rtq.Quantity * roomTypeRate.DailyPrice).ToString();
							totalPrice += rtq.Quantity * roomTypeRate.DailyPrice;
						}
						else if (stay == "Hourly" && stayCalculations.Hourly > 0)
						{
							chargesSummary.Charge = roomType.Name + " " + stay;
							chargesSummary.Unit = rtq.Quantity.ToString();
							chargesSummary.Rate = roomTypeRate.HourlyPrice.ToString();
							chargesSummary.Total = (rtq.Quantity * roomTypeRate.HourlyPrice).ToString();
							totalPrice += rtq.Quantity * roomTypeRate.HourlyPrice;
						}

						if (chargesSummary != null)
						{
							chargesSummaries.Add(chargesSummary);
						}
					}

				}
				ChargesSummaryDTO chargesSummaryTotal = new ChargesSummaryDTO();
				chargesSummaryTotal.Charge = "Total";
				chargesSummaryTotal.Unit = "";
				chargesSummaryTotal.Rate = "";
				chargesSummaryTotal.Total = totalPrice.ToString();
				chargesSummaries.Add(chargesSummaryTotal);


				getRateCalculation.ChargesSummary = chargesSummaries;
				getRateCalculation.TotalPrice = totalPrice;

				return ServiceResponse<GetRateCalculationDTOResponse>.ResponseSuccess(Data: getRateCalculation);
			}
			catch (Exception ex)
			{
				return ServiceResponse<GetRateCalculationDTOResponse>.ResponseFailure(ex.Message);
			}
		}

		private StayDurationCalculatorDTO StayDurationCalculator(DateTime checkIn, DateTime checkOut)
		{
			var totalHours = (int)(checkOut - checkIn).TotalHours;
			if (totalHours <= 0)
				return new StayDurationCalculatorDTO();

			int months = totalHours / (30 * 24);
			int remainingHours = totalHours % (30 * 24);

			int weeks = remainingHours / (7 * 24);
			remainingHours %= (7 * 24);

			int days = remainingHours / 24;
			int hours = remainingHours % 24;

			if (hours >= 20)
			{
				days += 1;
				hours = 0;
			}

			return new StayDurationCalculatorDTO
			{
				Monthly = months,
				Weekly = weeks,
				Daily = days,
				Hourly = hours,
				TotalHours = totalHours,
				TotalDays = totalHours / 24
			};
		}
	}
}
