using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HotelApp.Application.DTOs.RateCalculation
{
	public class GetRateCalculationDTOResponse
	{
		public decimal TotalPrice { get; set; }
		public decimal TotalPayments { get; set; }
        public decimal Balance { get; set; }

        public List<ChargesSummaryDTO> ChargesSummary { get; set; } = new();
    }
    public class ChargesSummaryDTO
	{
		public string Type { get; set; }
        public string Charge { get; set; }
		public string Rooms { get; set; }
		public string Unit { get; set; }
		public string Rate {  get; set; }
		public string Total { get; set; }
	}
	public class RoomTypeCalculationDTO
	{
		public string RoomTypeName { get; set; }
	}
	public class StayDurationCalculatorDTO
	{
		public int Hourly { get; set; }
		public int Daily { get; set; }
		public int Weekly { get; set; }
		public int Monthly { get; set; }
		public int TotalHours { get; set; }
		public int TotalDays { get; set; }
	}
	public class RoomTypeQuantityDTO
	{
		public int RoomTypeId { get; set; }
		public int Quantity { get; set; }
	}


	public class GetRateCalculationDTORequest
	{
		public int ReservationId { get; set; }
		public bool IsEdit { get; set; }
        public int RateId { get; set; }
		public DateTime CheckIn { get; set; }
		public DateTime CheckOut { get; set; }
        public List<RoomTypeQuantityDTO> RoomTypeQuantities { get; set; } = new();
    }
}
