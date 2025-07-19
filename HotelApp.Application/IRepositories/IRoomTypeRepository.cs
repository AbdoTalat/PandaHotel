using HotelApp.Application.DTOs.RoomTypes;
using HotelApp.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HotelApp.Application.IRepositories
{
    public interface IRoomTypeRepository
    {
        Task<IEnumerable<RoomType>> GetRoomTypesByIDsAsync(List<int> roomTypeIds);
        Task<IEnumerable<GetRoomTypesForReservationDTO>> GetRoomTypesForReservation();
    }
}
