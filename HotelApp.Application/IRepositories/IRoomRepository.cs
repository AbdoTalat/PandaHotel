using HotelApp.Application.DTOs;
using HotelApp.Application.DTOs.Reservation;
using HotelApp.Application.DTOs.Rooms;
using HotelApp.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HotelApp.Application.IRepositories
{
    public interface IRoomRepository
    {
        GetRoomsReview GetRoomsReview();
		Task<RoomReportDTO> GetRoomsReportBetweenDatesAsync(DateTime start, DateTime end);
        Task<IEnumerable<GetAllRoomsDTO>> GetFilteredRoomsAsync(RoomFilterDTO dto);
        Task<ServiceResponse<object>> ValidateRoomSelectionsAsync(List<RoomTypeToBookDTO> roomTypeToBookDTOs, List<int> selectedRoomIds);
		Task<int> GetOccupancyPercentAsync();
	}
}
