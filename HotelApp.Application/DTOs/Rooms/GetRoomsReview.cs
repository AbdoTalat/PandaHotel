using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HotelApp.Application.DTOs.Rooms
{
	public class GetRoomsReview
	{
		public int available {  get; set; }
		public int maintain { get; set; }
		public int Occupied { get; set; }
	}
}
