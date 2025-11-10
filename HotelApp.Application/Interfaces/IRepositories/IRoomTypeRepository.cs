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
        Task<IEnumerable<GetRoomTypesForReservationDTO>> GetRoomTypesForReservationAsync(RoomTypeAvailabilityRequestDTO dto);
        Task<List<RoomTypeAvailabilityResultDTO>> GetAllRoomTypesAvailabilityAsync(RoomTypeAvailabilityRequestDTO dto, CancellationToken cancellationToken = default);
        Task<int> GetRoomTypeAvailabilityAsync(RoomTypeAvailabilityRequestDTO dto);
    }
}
