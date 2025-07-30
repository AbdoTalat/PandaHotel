using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HotelApp.Application.DTOs.Rooms
{
	public class RoomReportDTO
	{
		public List<RoomsDetailsDTO> roomsDetails = new List<RoomsDetailsDTO>();
		public int NumOfAvailable { get; set; } = 0;
		public int NumOfOccupied { get; set; } = 0;
		public int NumOfMaintainable { get; set; } = 0;
	}
	public class RoomsDetailsDTO
	{
		public string RoomNumber { get; set; } = "";
		public string Description { get; set; } = "";
		public int MaxNumOfAdults { get; set; } = 0;
		public int MaxNumOfChildrens { get; set; } = 0;
		public bool IsActive { get; set; } = false;
	}
}
