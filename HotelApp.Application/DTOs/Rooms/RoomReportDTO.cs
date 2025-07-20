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
		public int NumOfAvailable {  get; set; }
		public int NumOfOccupied { get; set; }
		public int NumOfMaintainable { get; set; }
	}
	public class RoomsDetailsDTO
	{
		public string RoomNumber { get; set; }
		public string Description { get; set; }
		public int MaxNumOfAdults { get; set; }
		public int MaxNumOfChildrens { get; set; }
		public bool IsActive { get; set; }
	}
}
