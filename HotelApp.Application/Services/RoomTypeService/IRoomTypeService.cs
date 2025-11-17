using HotelApp.Application.DTOs;
using HotelApp.Application.DTOs.RoomTypes;
using HotelApp.Domain.Entities;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace HotelApp.Application.Services.RoomTypeService
{
    public interface IRoomTypeService
	{
		Task<IEnumerable<GetAllRoomTypesDTO>> GetAllRoomTypesAsync();
		Task<GetRoomTypeByIdDTO?> GetRoomTypeByIdAsync(int Id);
        Task<RoomTypeDTO?> GetRoomTypeToEditByIdAsync(int Id);
        Task<IEnumerable<SelectListItem>> GetRoomTypesDropDownAsync();
		Task<ServiceResponse<RoomTypeDTO>> AddRoomTypeAsync(RoomTypeDTO roomTypeDTO);
		Task<ServiceResponse<RoomTypeDTO>> EditRoomTypeAsync(RoomTypeDTO roomTypeDTO);
		Task<ServiceResponse<RoomType>> DeleteRoomTypeAsync(int Id);	
		Task<IEnumerable<GetRoomTypesForReservationDTO>> GetRoomTypesForReservationAsync(RoomTypeAvailabilityRequestDTO dto);
        Task<int> GetRoomTypeAvailabilityAsync(RoomTypeAvailabilityRequestDTO dto);

    }
}
