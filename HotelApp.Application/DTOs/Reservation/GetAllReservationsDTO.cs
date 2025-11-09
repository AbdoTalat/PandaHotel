using HotelApp.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HotelApp.Application.DTOs.Reservation
{
    public class GetAllReservationsDTO
    {
        public int Id { get; set; }
        public string ReservationNumber { get; set; }
        public string PrimaryGuestName { get; set; }
        public DateTime CheckInDate { get; set; }
        public DateTime CheckOutDate { get; set; }
        public string CreatedBy { get; set; }
        public ReservationStatus Status { get; set; }
        public string ReservationSource { get; set; }
        public int NumberOfNights { get; set; }
        public int NumberOfPeople { get; set; }
        public decimal TotalPrice { get; set; }
    }
}
