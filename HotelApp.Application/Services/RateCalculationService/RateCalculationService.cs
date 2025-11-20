using HotelApp.Application.DTOs.RateCalculation;
using HotelApp.Application.Interfaces;
using HotelApp.Domain;
using HotelApp.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static HotelApp.Domain.Enums.MasterDataItemEnums;

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
            try
            {
                IEnumerable<Payment> payments = new List<Payment>();
                if (request.ReservationId > 0)
                    payments = await _unitOfWork.PaymentRepository.GetAllAsync(p => p.ReservationId == request.ReservationId, includes: r => r.TransactionType);

                var rate = await _unitOfWork.RateRepository.GetByIdAsync(request.RateId);
                if (rate == null)
                    return ServiceResponse<GetRateCalculationDTOResponse>.ResponseFailure("Rate not found.");

                // Get room types and rates
                var roomTypeIds = request.RoomTypeQuantities.Select(rt => rt.RoomTypeId).ToList();
                var roomTypes = await _unitOfWork.RoomTypeRepository.GetAllAsync(rt => roomTypeIds.Contains(rt.Id));
                var roomTypeRates = await _unitOfWork.RoomTypeRateRepository
                    .GetAllAsync(rtr => rtr.RateId == request.RateId && roomTypeIds.Contains(rtr.RoomTypeId));

                // Convert to dictionary 
                var roomTypeDict = roomTypes.ToDictionary(rt => rt.Id);
                var roomTypeRateDict = roomTypeRates.ToDictionary(rtr => rtr.RoomTypeId);

                // Calculate stay duration
                var stayCalculations = StayDurationCalculator(request.CheckIn, request.CheckOut);
                var stayMap = new Dictionary<string, int>
                {
                    ["Monthly"] = stayCalculations.Monthly,
                    ["Weekly"] = stayCalculations.Weekly,
                    ["Daily"] = stayCalculations.Daily,
                    ["Hourly"] = stayCalculations.Hourly
                };

                var chargesSummaries = new List<ChargesSummaryDTO>();
                decimal totalPrice = 0;
                decimal totalPayments = 0;
                decimal balance = 0;

                // --- Calculate room charges ---
                foreach (var rtq in request.RoomTypeQuantities)
                {
                    var roomType = roomTypeDict.TryGetValue(rtq.RoomTypeId, out var rt) ? rt : new RoomType();
                    var roomTypeRate = roomTypeRateDict.TryGetValue(rtq.RoomTypeId, out var rtr) ? rtr : new RoomTypeRate();

                    foreach (var kvp in stayMap)
                    {
                        var stayName = kvp.Key;
                        var stayUnits = kvp.Value;
                        if (stayUnits <= 0)
                            continue;

                        decimal ratePrice = stayName switch
                        {
                            "Monthly" => roomTypeRate.MonthlyPrice,
                            "Weekly" => roomTypeRate.WeeklyPrice,
                            "Daily" => roomTypeRate.DailyPrice,
                            "Hourly" => roomTypeRate.HourlyPrice,
                            _ => 0
                        };

                        decimal total = rtq.Quantity * ratePrice * stayUnits;
                        totalPrice += total;

                        chargesSummaries.Add(new ChargesSummaryDTO
                        {
                            Type = "Rate",
                            Charge = $"{roomType.Name}/{stayName}",
                            Rooms = rtq.Quantity.ToString(),
                            Unit = stayUnits.ToString(),
                            Rate = $"${ratePrice}",
                            Total = $"${total}" 
                        });
                    }
                }

                // Total rate Price
                chargesSummaries.Add(new ChargesSummaryDTO
                {
                    Type = "Rate",
                    Charge = "Total",
                    Total = $"${totalPrice}"
                });

                // --- Process payments ---
                foreach (var payment in payments)
                {
                    var amount = payment.Amount;
                    if (payment.TransactionType?.Value == (int)TransactionType.Refund)
                    {
                        totalPayments -= amount;
                        amount = -amount;
                    }
                    else if (payment.TransactionType?.Value == (int)TransactionType.Payment)
                    {
                        totalPayments += amount;
                    }

                    chargesSummaries.Add(new ChargesSummaryDTO
                    {
                        Type = "Payment",
                        Charge = payment.TransactionType?.Name ?? "",
                        Total = amount >= 0 ? $"+ ${amount}" : $"- ${(amount)}"
                    });
                }

                // Total payments
                chargesSummaries.Add(new ChargesSummaryDTO
                {
                    Type = "Payment",
                    Charge = "Total Payments",
                    Total = $"${totalPayments}"
                });

                // --- Calculate balance ---
                balance = totalPrice - totalPayments;
                chargesSummaries.Add(new ChargesSummaryDTO
                {
                    Type = "Balance",
                    Charge = "Balance",
                    Total = $"${balance}"
                });

                var response = new GetRateCalculationDTOResponse
                {
                    ChargesSummary = chargesSummaries,
                    TotalPrice = totalPrice,
                    TotalPayments = totalPayments,
                    Balance = balance
                };

                return ServiceResponse<GetRateCalculationDTOResponse>.ResponseSuccess(Data: response);
            }
            catch (Exception ex)
            {
                return ServiceResponse<GetRateCalculationDTOResponse>.ResponseFailure(ex.Message);
            }
        }


        //      public async Task<ServiceResponse<GetRateCalculationDTOResponse>> GetRateCalculation(GetRateCalculationDTORequest request)
        //      {
        //	try
        //	{
        //		IEnumerable<Payment> payments = new List<Payment>();
        //		if (request.ReservationId > 0)
        //			payments = await _unitOfWork.PaymentRepository.GetAllAsync(p => p.ReservationId == request.ReservationId, includes: r => r.TransactionType);

        //		var rate = await _unitOfWork.RateRepository.GetByIdAsync(request.RateId);
        //		if (rate == null)
        //			return ServiceResponse<GetRateCalculationDTOResponse>.ResponseFailure("Rate not found.");

        //		var roomTypesIDs = request.RoomTypeQuantities.Select(rt => rt.RoomTypeId).ToList();
        //		var roomTypes = await _unitOfWork.RoomTypeRepository.GetAllAsync(rt => roomTypesIDs.Contains(rt.Id));

        //		var roomTypeRates = await _unitOfWork.RoomTypeRateRepository.GetAllAsync(rtr => rtr.RateId == request.RateId && roomTypesIDs.Contains(rtr.RoomTypeId));

        //		StayDurationCalculatorDTO stayCalculations = StayDurationCalculator(request.CheckIn, request.CheckOut);
        //		string[] stayCalculationNames = ["Monthly", "Weekly", "Daily", "Hourly"];

        //		List<ChargesSummaryDTO> chargesSummaries = new List<ChargesSummaryDTO>();
        //		decimal totalPrice = 0;
        //		decimal totalPayments = 0;
        //		decimal Balance = 0;

        //		GetRateCalculationDTOResponse getRateCalculation = new GetRateCalculationDTOResponse();

        //		foreach (var rtq in request.RoomTypeQuantities)
        //		{
        //			var roomTypeRate = roomTypeRates.FirstOrDefault(rt => rt.RoomTypeId == rtq.RoomTypeId) ?? new RoomTypeRate();
        //			var roomType = roomTypes.FirstOrDefault(rt => rt.Id == rtq.RoomTypeId) ?? new RoomType();

        //			foreach (var stay in stayCalculationNames)
        //			{
        //				ChargesSummaryDTO chargesSummary = new ChargesSummaryDTO();
        //				if (stay == "Monthly" && stayCalculations.Monthly > 0)
        //				{
        //					chargesSummary.Charge = roomType.Name + "/" + stay;
        //					chargesSummary.Rooms = rtq.Quantity.ToString();
        //					chargesSummary.Unit = stayCalculations.Monthly.ToString();
        //					chargesSummary.Rate = "$" + roomTypeRate.MonthlyPrice.ToString();
        //					chargesSummary.Total = "$" + (rtq.Quantity * roomTypeRate.MonthlyPrice * stayCalculations.Monthly).ToString();
        //					totalPrice += rtq.Quantity * roomTypeRate.MonthlyPrice * stayCalculations.Monthly;
        //				}
        //				else if (stay == "Weekly" && stayCalculations.Weekly > 0)
        //				{
        //					chargesSummary.Charge = roomType.Name + "/" + stay;
        //					chargesSummary.Rooms = rtq.Quantity.ToString();
        //					chargesSummary.Unit = stayCalculations.Weekly.ToString();
        //					chargesSummary.Rate = "$" + roomTypeRate.WeeklyPrice.ToString();
        //					chargesSummary.Total = "$" + (rtq.Quantity * roomTypeRate.WeeklyPrice * stayCalculations.Weekly).ToString();
        //					totalPrice += rtq.Quantity * roomTypeRate.WeeklyPrice * stayCalculations.Weekly;
        //				}
        //				else if (stay == "Daily" && stayCalculations.Daily > 0)
        //				{
        //					chargesSummary.Charge = roomType.Name + "/" + stay;
        //					chargesSummary.Rooms = rtq.Quantity.ToString();
        //					chargesSummary.Unit = stayCalculations.Daily.ToString();
        //					chargesSummary.Rate = "$" + roomTypeRate.DailyPrice.ToString();
        //					chargesSummary.Total = "$" + (rtq.Quantity * roomTypeRate.DailyPrice * stayCalculations.Daily).ToString();
        //					totalPrice += rtq.Quantity * roomTypeRate.DailyPrice * stayCalculations.Daily;
        //				}
        //				else if (stay == "Hourly" && stayCalculations.Hourly > 0)
        //				{
        //					chargesSummary.Charge = roomType.Name + "/" + stay;
        //					chargesSummary.Rooms = rtq.Quantity.ToString();
        //					chargesSummary.Unit = stayCalculations.Hourly.ToString();
        //					chargesSummary.Rate = "$" + roomTypeRate.HourlyPrice.ToString();
        //					chargesSummary.Total = "$" + (rtq.Quantity * roomTypeRate.HourlyPrice * stayCalculations.Hourly).ToString();
        //					totalPrice += rtq.Quantity * roomTypeRate.HourlyPrice * stayCalculations.Hourly;
        //				}

        //				if (chargesSummary != null && chargesSummary?.Total != null)
        //				{
        //					chargesSummary.Type = "Rate";
        //					chargesSummaries.Add(chargesSummary);
        //				}
        //			}

        //		}

        //		ChargesSummaryDTO chargesSummaryTotal = new ChargesSummaryDTO();
        //		chargesSummaryTotal.Charge = "Total";
        //		chargesSummaryTotal.Rooms = "";
        //		chargesSummaryTotal.Unit = "";
        //		chargesSummaryTotal.Rate = "";
        //		chargesSummaryTotal.Total = "$" + totalPrice.ToString();
        //		chargesSummaryTotal.Type = "Rate";
        //		chargesSummaries.Add(chargesSummaryTotal);

        //		if (payments != null && payments.Any())
        //		{
        //			foreach (var payment in payments)
        //			{
        //				ChargesSummaryDTO chargesSummaryPaymentDetail = new ChargesSummaryDTO();
        //				chargesSummaryPaymentDetail.Charge = payment?.TransactionType?.Name ?? "";
        //				chargesSummaryPaymentDetail.Rooms = "";
        //				chargesSummaryPaymentDetail.Unit = "";
        //				chargesSummaryPaymentDetail.Rate = "";
        //				if (payment?.TransactionType?.Value == (int)TransactionType.Refund)
        //				{
        //					chargesSummaryPaymentDetail.Total = "- $" + (payment.Amount).ToString();
        //				}
        //				else if (payment?.TransactionType?.Value == (int)TransactionType.Payment)
        //				{
        //					chargesSummaryPaymentDetail.Total = "+ $" + (payment.Amount).ToString();
        //				}
        //				chargesSummaries.Add(chargesSummaryPaymentDetail);

        //				if (payment?.TransactionType?.Value == (int)TransactionType.Refund)
        //				{
        //					totalPayments -= payment.Amount;
        //				}
        //				else if (payment?.TransactionType?.Value == (int)TransactionType.Payment)
        //				{
        //					totalPayments += payment.Amount;
        //				}
        //				chargesSummaryPaymentDetail.Type = "Payment";
        //			}

        //			ChargesSummaryDTO chargesSummaryPayments = new ChargesSummaryDTO();
        //			chargesSummaryPayments.Charge = "Total Payments";
        //			chargesSummaryPayments.Rooms = "";
        //			chargesSummaryPayments.Unit = "";
        //			chargesSummaryPayments.Rate = "";
        //			chargesSummaryPayments.Total = "$" + (totalPayments).ToString();
        //			chargesSummaryPayments.Type = "Payment";

        //			chargesSummaries.Add(chargesSummaryPayments);
        //		}

        //		Balance = totalPrice - totalPayments;
        //		ChargesSummaryDTO chargesSummaryBalance = new ChargesSummaryDTO();
        //		chargesSummaryBalance.Charge = "Balance";
        //		chargesSummaryBalance.Rooms = "";
        //		chargesSummaryBalance.Unit = "";
        //		chargesSummaryBalance.Rate = "";
        //		chargesSummaryBalance.Total = "$" + Balance.ToString();
        //		chargesSummaryBalance.Type = "Balance";
        //		chargesSummaries.Add(chargesSummaryBalance);

        //		getRateCalculation.ChargesSummary = chargesSummaries;
        //		getRateCalculation.TotalPrice = totalPrice;
        //		getRateCalculation.TotalPayments = totalPayments;
        //		getRateCalculation.Balance = Balance;

        //		return ServiceResponse<GetRateCalculationDTOResponse>.ResponseSuccess(Data: getRateCalculation);
        //	}
        //	catch (Exception ex)
        //	{
        //		return ServiceResponse<GetRateCalculationDTOResponse>.ResponseFailure(ex.Message);
        //	}
        //}

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
            if (hours <= 4)
            {
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
