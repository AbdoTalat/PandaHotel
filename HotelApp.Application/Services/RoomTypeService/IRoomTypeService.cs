using HotelApp.Application.DTOs;
using HotelApp.Application.DTOs.RoomTypes;
using HotelApp.Domain.Entities;

namespace HotelApp.Application.Services.RoomTypeService
{
    public interface IRoomTypeService
	{
		Task<IEnumerable<GetAllRoomTypesDTO>> GetAllRoomTypesAsync();
		Task<GetRoomTypeByIdDTO?> GetRoomTypeByIdAsync(int Id);
        Task<RoomTypeDTO?> GetRoomTypeToEditByIdAsync(int Id);
        Task<IEnumerable<DropDownDTO<string>>> GetRoomTypesDropDownAsync();
		Task<IEnumerable<GetRoomTypesForReservationDTO>> GetRoomTypesForReservationAsync(RoomAvailabilityRequestDTO dto);
		Task<ServiceResponse<RoomTypeDTO>> AddRoomTypeAsync(RoomTypeDTO roomTypeDTO);
		Task<ServiceResponse<RoomTypeDTO>> EditRoomTypeAsync(RoomTypeDTO roomTypeDTO);
		Task<ServiceResponse<RoomType>> DeleteRoomTypeAsync(int Id);
		Task<int> GetAvailableRoomCountAsync(int branchId, int roomTypeId, DateTime checkInDate, DateTime checkOutDate, int? reservationId);

    }
}
