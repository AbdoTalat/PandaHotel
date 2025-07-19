using HotelApp.Application.DTOs;
using HotelApp.Application.DTOs.Rooms;
using HotelApp.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HotelApp.Application.IRepositories
{
    public interface IRoomRepository
    {
        GetRoomsReview GetRoomsReview();
        Task<int> CheckRoomAvailabilityAsync(int roomTypeId, int requestedRooms);
    }
}
