using HotelApp.Application.DTOs;
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
        Task<IEnumerable<DropDownDTO<string>>> GetRoomStatusDropDownAsync();
		Task<IEnumerable<DropDownDTO<string>>> GetRoomStatusDropDownWithoutBranchFilterAsync();
		Task<RoomStatusDTO?> GetRoomStatusToEditByIdAsync(int Id);
        Task<ServiceResponse<RoomStatusDTO>> AddRoomStatusAsync(RoomStatusDTO roomStatusDTO);
        Task<ServiceResponse<RoomStatusDTO>> EditRoomStatusAsync(RoomStatusDTO roomStatusDTO);
        Task<ServiceResponse<object>> DeleteRoomStatusByIdAsync(int Id);
    }
}
