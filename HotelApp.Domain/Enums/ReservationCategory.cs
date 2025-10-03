using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HotelApp.Domain.Enums
{
	public enum ReservationCategory
	{
		All = 0,        // default (no filter)
		NewBookings = 1,
		Arrivals = 2,
		Departures = 3,
		StayOvers = 4,
		Cancellations = 5,
		NoShow = 6
	}

}
