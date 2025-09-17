using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HotelApp.Application.DTOs.Reservation;
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
		Task<ServiceResponse<AddRoomDTO>> AddRoomAsync(AddRoomDTO dto);
		Task<ServiceResponse<AddRoomDTO>> AddManyRoomsAsync(AddRoomDTO dto);
		Task<ServiceResponse<EditRoomDTO>> EditRoomAsync(EditRoomDTO dto);
		Task<ServiceResponse<Room>> DeleteRoomAsync(int Id);
		Task<RoomReportDTO> GetRoomsReportBetweenDatesAsync(DateTime start, DateTime end);
		Task<IEnumerable<GetAvailableRoomsDTO>> GetAvailableRoomsAsync(string? name);
		Task<IEnumerable<GetAllRoomsDTO>> GetFilteredRoomsAsync(RoomFilterDTO dto);
		Task<ServiceResponse<object>> ValidateRoomSelectionsAsync(List<RoomTypeToBookDTO> roomTypeToBookDTOs, List<int> selectedRoomIds);
		Task<int> GetOccupancyPercentAsync();
	}
}
