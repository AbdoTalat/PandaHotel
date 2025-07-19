using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HotelApp.Domain.Entities
{
	public class ReservationRoomType
	{
		public int Id { get; set; }

		public int ReservationId { get; set; }
		public Reservation? Reservation { get; set; }

		public int RoomTypeId { get; set; }
		public RoomType? RoomType { get; set; }

		public int Quantity { get; set; }
	}
}
