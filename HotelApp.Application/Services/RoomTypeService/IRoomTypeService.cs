using HotelApp.Application.DTOs.RoomTypes;
using HotelApp.Domain.Entities;

namespace HotelApp.Application.Services.RoomTypeService
{
    public interface IRoomTypeService
	{
		Task<IEnumerable<GetAllRoomTypesDTO>> GetAllRoomTypesAsync();
		Task<GetRoomTypeByIdDTO?> GetRoomTypeByIdAsync(int Id);
        Task<EditRoomTypeDTO?> GetRoomTypeToEditByIdAsync(int Id);
        Task<IEnumerable<GetRoomTypeItemsDTO>> GetRoomTypeSelectListAsync();
		Task<IEnumerable<GetRoomTypesForReservationDTO>> GetRoomTypesForReservationAsync();
		Task<ServiceResponse<AddRoomTypeDTO>> AddRoomTypeAsync(AddRoomTypeDTO roomTypeDTO);
		Task<ServiceResponse<EditRoomTypeDTO>> EditRoomTypeAsync(EditRoomTypeDTO roomTypeDTO);
		Task<ServiceResponse<RoomType>> DeleteRoomTypeAsync(int Id);
	}
}
