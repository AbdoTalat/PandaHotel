using HotelApp.Infrastructure.DbContext;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HotelApp.Application.DTOs.Rooms;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using HotelApp.Domain.Enums;
using HotelApp.Helper;
using HotelApp.Domain.Entities;
using Microsoft.Identity.Client;
using HotelApp.Application.DTOs.Reservation;
using HotelApp.Application;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;
using HotelApp.Application.Interfaces.IRepositories;
using HotelApp.Infrastructure.UnitOfWorks;
using Microsoft.AspNetCore.Mvc;
using static System.Net.Mime.MediaTypeNames;

namespace HotelApp.Infrastructure.Repositories
{
    public class RoomRepository : GenericRepository<Room>, IRoomRepository
    {
        private readonly ApplicationDbContext _context;
		private readonly IConfigurationProvider _mapperConfig;

		public RoomRepository(ApplicationDbContext context, IConfigurationProvider mapperConfig)
			: base(context, mapperConfig)
        {
            _context = context;
			_mapperConfig = mapperConfig;
		}

		public GetRoomsReview GetRoomsReview()
        {
			var available = _context.Rooms
				.BranchFilter()
		        .Where(r =>  r.RoomStatus.Code == RoomStatusEnum.Available)
		        .Count();

			var Occupied = _context.Rooms
			   .BranchFilter()
			   .Where(r => r.RoomStatus.Code == RoomStatusEnum.Occupied)
			   .Count();

			var Maintainance = _context.Rooms
			   .BranchFilter()
			   .Where(r => r.RoomStatus.Code == RoomStatusEnum.Maintenance)
			   .Count();

            var result = new GetRoomsReview
            {
                available = available,
                Occupied = Occupied,
                maintain = Maintainance
            };
            return result;
		}
        public async Task<RoomReportDTO> GetRoomsReportBetweenDatesAsync(DateTime start, DateTime end)
        {
			var query = _context.Rooms
				.Include(r => r.RoomStatus)
				.Where(r => r.CreatedDate >= start && r.CreatedDate <= end);

			var roomDetails = await query
				.ProjectTo<RoomsDetailsDTO>(_mapperConfig)
				.ToListAsync();

			var statusCounts = await query
				.GroupBy(r => r.RoomStatus.Code)
				.Select(g => new { Status = g.Key, Count = g.Count() })
				.ToListAsync();

			int available = statusCounts.FirstOrDefault(s => s.Status == RoomStatusEnum.Available)?.Count ?? 0;
			int occupied = statusCounts.FirstOrDefault(s => s.Status == RoomStatusEnum.Occupied)?.Count ?? 0;
			int maintenance = statusCounts.FirstOrDefault(s => s.Status == RoomStatusEnum.Maintenance)?.Count ?? 0;
			

			return new RoomReportDTO
			{
				roomsDetails = roomDetails,
				NumOfAvailable = available,
				NumOfOccupied = occupied,
				NumOfMaintainable = maintenance
			};
			
		}
		public async Task<IEnumerable<GetAllRoomsDTO>> GetFilteredRoomsAsync(RoomFilterDTO dto)
		{
			var Query = _context.Rooms
				 .BranchFilter();

			if (!string.IsNullOrEmpty(dto.RoomNumber))
				Query = Query.Where(r => r.RoomNumber.Contains(dto.RoomNumber));

			if (dto.MaxNumOfAdults > 0)
				Query = Query.Where(r => r.MaxNumOfAdults >= dto.MaxNumOfAdults);

			if (dto.MaxNumOfChildrens > 0)
				Query = Query.Where(r => r.MaxNumOfChildrens >= dto.MaxNumOfChildrens);

			if (dto.RoomTypeId >= 1)
				Query = Query.Where(r => r.RoomTypeId == dto.RoomTypeId);

			if (dto.RoomStatusId >= 1)
				Query = Query.Where(r => r.RoomStatusId == dto.RoomStatusId);

			if (dto.IsActive.HasValue)
				Query = Query.Where(r => r.IsActive == dto.IsActive.Value);

			return await Query.ProjectTo<GetAllRoomsDTO>(_mapperConfig).ToListAsync();
		}
        public async Task<ServiceResponse<object>> ValidateRoomSelectionsAsync(List<RoomTypeToBookDTO> roomTypeToBookDTOs, List<int> selectedRoomIds)
        {
            if (roomTypeToBookDTOs == null || !roomTypeToBookDTOs.Any())
                return ServiceResponse<object>.ResponseFailure("No room types selected.");

            if (selectedRoomIds == null || !selectedRoomIds.Any())
            {
                return ServiceResponse<object>.ResponseSuccess("Reservation saved without assigning rooms. Rooms will be assigned later.");
            }
            var selectedRooms = await _context.Rooms
                .AsNoTracking()
                .BranchFilter()
                .Where(r => selectedRoomIds.Contains(r.Id))
                .Select(r => new { r.Id, r.RoomTypeId })
                .ToListAsync();

            if (selectedRooms.Count != selectedRoomIds.Count)
                return ServiceResponse<object>.ResponseFailure("Some selected rooms do not exist.");

            var selectedByType = selectedRooms
                .GroupBy(r => r.RoomTypeId)
                .ToDictionary(g => g.Key, g => g.Count());

            foreach (var typeDto in roomTypeToBookDTOs)
            {
                var requestedCount = typeDto.NumOfRooms;

                if (!selectedByType.TryGetValue(typeDto.RoomTypeId, out var selectedCount))
                {
                    return ServiceResponse<object>.ResponseFailure( $"You must select {requestedCount} room(s) for {GetRoomTypeName(typeDto.RoomTypeId)}.");
                }

                if (requestedCount < selectedCount)
                {
                    return ServiceResponse<object>.ResponseFailure($"For {GetRoomTypeName(typeDto.RoomTypeId)} you selected {selectedCount}, but required {requestedCount}.");
                }
            }

            return ServiceResponse<object>.ResponseSuccess("Room selection is valid.");
        }

		public async Task<int> GetOccupancyPercentAsync()
		{
			var Query = _context.Rooms
				.BranchFilter()
				.AsQueryable();
			//var counts = await Query
			//	.GroupBy(_ => 1)
			//	.Select(g => new
			//	{
			//		Total = g.Count(),
			//		Occupied = g.Count(r => r.RoomStatus != null && !r.RoomStatus.IsReservable)
			//	})
			//	.FirstOrDefaultAsync();


			int totalCount = await Query.CountAsync();
			if (totalCount == 0)
				return 0;

			int occupiedCount = await Query.CountAsync(r => !r.RoomStatus.IsReservable);

			int percent = (int)((double)occupiedCount / totalCount * 100);

			return percent;
		}
        public async Task<IEnumerable<GetAvailableRoomsDTO>> GetAvailableRoomsAsync(string? name, int roomTypeId, DateTime checkInDate, DateTime checkOutDate)
		{
			var Query = _context.Rooms
				.AsNoTracking()
				.Include(r => r.RoomStatus)
				.Include(r => r.RoomType)
				.BranchFilter()
				.Where(r => r.IsActive && r.RoomStatus.IsReservable && r.RoomTypeId == roomTypeId);

			if (!string.IsNullOrWhiteSpace(name))
				Query = Query.Where(r => r.RoomNumber.Contains(name) || (r.RoomType != null && r.RoomType.Name.Contains(name)));

			Query = Query.Where(r => !r.ReservationsRooms.Any(rr => rr.StartDate < checkOutDate && rr.EndDate > checkInDate));

			return await Query
				.ProjectTo<GetAvailableRoomsDTO>(_mapperConfig)
				.ToListAsync();
		}
        private string GetRoomTypeName(int roomTypeId)
        {
            return _context.RoomTypes
                .Where(rt => rt.Id == roomTypeId)
                .Select(rt => rt.Name)
                .FirstOrDefault() ?? $"RoomType {roomTypeId}";
        }



    }
}
