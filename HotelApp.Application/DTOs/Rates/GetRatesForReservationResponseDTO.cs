using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HotelApp.Application.DTOs.Rates
{
	public class GetRatesForReservationResponseDTO
	{
		public int Id { get; set; }
		public string Name { get; set; }
		public List<RoomTypeRateForReservationDTO> roomTypeRateForReservations { get; set; } = new List<RoomTypeRateForReservationDTO>();
	}
	public class RoomTypeRateForReservationDTO
	{
		public string TypeName { get; set; }
		public decimal DailyPrice { get; set; }
	}
}
