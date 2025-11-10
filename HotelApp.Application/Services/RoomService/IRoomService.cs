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
		Task<GetRoomByIdDTO?> GetRoomByIdAsync(int Id);
		Task<EditRoomDTO?> GetRoomToEditByIdAsync(int Id);
		GetRoomsReview GetRoomsReview();
		Task<ServiceResponse<AddRoomDTO>> AddRoomAsync(AddRoomDTO dto);
		Task<ServiceResponse<AddRoomDTO>> AddManyRoomsAsync(AddRoomDTO dto);
		Task<ServiceResponse<EditRoomDTO>> EditRoomAsync(EditRoomDTO dto);
		Task<ServiceResponse<Room>> DeleteRoomAsync(int Id);
		Task<RoomReportDTO> GetRoomsReportBetweenDatesAsync(DateTime start, DateTime end);
		Task<IEnumerable<GetAvailableRoomsDTO>> GetAvailableRoomsAsync(string? name, int roomTypeId, DateTime checkInDate, DateTime checkOutDate);
		Task<IEnumerable<GetAllRoomsDTO>> GetFilteredRoomsAsync(RoomFilterDTO dto);
		Task<ServiceResponse<object>> ValidateRoomSelectionsAsync(List<RoomTypeToBookDTO> roomTypeToBookDTOs, List<int> selectedRoomIds);
		Task<int> GetOccupancyPercentAsync();
		Task<IEnumerable<GetRoomsForEditReservationDTO>> GetRoomsForEditReservationByIDs(List<int> Ids);

    }
}
