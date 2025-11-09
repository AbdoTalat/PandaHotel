using HotelApp.Application.DTOs.RoomTypes;
using HotelApp.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HotelApp.Application.Interfaces.IRepositories
{
    public interface IRoomTypeRepository : IGenericRepository<RoomType>
    {
        Task<IEnumerable<RoomType>> GetRoomTypesByIDsAsync(List<int> roomTypeIds);
        Task<IEnumerable<GetRoomTypesForReservationDTO>> GetRoomTypesForReservationAsync(RoomAvailabilityRequestDTO dto);
        Task<int> GetAvailableRoomCountAsync(int branchId, int roomTypeId, DateTime checkIn, DateTime checkOut, int? reservationId);
    }
}
