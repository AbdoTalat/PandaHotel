using HotelApp.Infrastructure.DbContext;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HotelApp.Application.DTOs.Rooms;
using HotelApp.Application.IRepositories;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using HotelApp.Domain.Enums;
using HotelApp.Helper;
using HotelApp.Domain.Entities;
using Microsoft.Identity.Client;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace HotelApp.Infrastructure.Repositories
{
    public class RoomRepository : IRoomRepository
    {
        private readonly ApplicationDbContext _context;
		private readonly IConfigurationProvider _mapperConfig;

		public RoomRepository(ApplicationDbContext context, IConfigurationProvider mapperConfig)
        {
            _context = context;
			_mapperConfig = mapperConfig;
		}

		public GetRoomsReview GetRoomsReview()
        {
			var available = _context.Rooms
				.BranchFilter()
		        .Where(r =>  r.RoomStatus.Name.ToLower() == "available")
		        .Count();

			var Occupied = _context.Rooms
			   .BranchFilter()
			   .Where(r => r.RoomStatus.Name.ToLower() == "occupied")
			   .Count();

			var Maintainance = _context.Rooms
			   .BranchFilter()
			   .Where(r => r.RoomStatus.Name.ToLower() == "maintenance")
			   .Count();

            var result = new GetRoomsReview
            {
                available = available,
                Occupied = Occupied,
                maintain = Maintainance
            };
            return result;
		}
        public async Task<int> CheckRoomAvailabilityAsync(int roomTypeId, int requestedRooms)
        {
            var availableRoomsCount = await _context.Rooms
                .Where(r => r.RoomTypeId == roomTypeId && r.RoomStatus.Name.ToLower() == "available")
                .BranchFilter()
                .CountAsync();

            return availableRoomsCount;
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
				.GroupBy(r => r.RoomStatus.Name)
				.Select(g => new { Status = g.Key, Count = g.Count() })
				.ToListAsync();

			int available = statusCounts.FirstOrDefault(s => s.Status == "Available")?.Count ?? 0;
			int occupied = statusCounts.FirstOrDefault(s => s.Status == "Occupied")?.Count ?? 0;
			int maintenance = statusCounts.FirstOrDefault(s => s.Status == "Maintenance")?.Count ?? 0;
			

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
	}
}
