using HotelApp.Application.DTOs.RoomStatus;
using HotelApp.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HotelApp.Application.Services.RoomStatusService
{
    public interface IRoomStatusService
    {
        Task<IEnumerable<GetAllRoomStatusDTO>> GetAllRoomStatusAsync();
        Task<IEnumerable<GetRoomStatusItemsDTO>> GetRoomStatusItemsAsync();
		Task<IEnumerable<GetRoomStatusItemsDTO>> GetRoomStatusItemsWithoutBranchFilterAsync();
		Task<EditRoomStatusDTO?> GetRoomStatusToEditByIdAsync(int Id);
        Task<ServiceResponse<AddRoomStatusDTO>> AddRoomStatusAsync(AddRoomStatusDTO roomStatusDTO);
        Task<ServiceResponse<EditRoomStatusDTO>> EditRoomStatusAsync(EditRoomStatusDTO roomStatusDTO);
    }
}
