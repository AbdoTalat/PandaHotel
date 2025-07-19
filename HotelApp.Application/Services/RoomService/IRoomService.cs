using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HotelApp.Application.DTOs.Rooms;
using HotelApp.Domain.Entities;

namespace HotelApp.Application.Services.RoomService
{
    public interface IRoomService
	 {
		Task<IEnumerable<GetAllRoomsDTO>> GetAllRoomsAsync();
		GetRoomsReview GetRoomsReview();
		Task<EditRoomDTO?> GetRoomToEditByIdAsync(int Id);
		Task<GetRoomByIdDTO?> GetRoomByIdAsync(int Id);
		Task<ServiceResponse<AddRoomDTO>> AddRoomAsync(AddRoomDTO room);
		Task<ServiceResponse<EditRoomDTO>> EditRoomAsync(EditRoomDTO room);
		Task<ServiceResponse<Room>> DeleteRoomAsync(int Id);
		Task<ServiceResponse<string>> CheckRoomAvailabilityAsync(int roomTypeId, int numberOfRooms);

     }
}
