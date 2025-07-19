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

namespace HotelApp.Infrastructure.Repositories
{
    public class RoomRepository : IRoomRepository
    {
        private readonly ApplicationDbContext _context;
		private readonly IConfigurationProvider _mapper;

		public RoomRepository(ApplicationDbContext context, IConfigurationProvider mapper)
        {
            _context = context;
			_mapper = mapper;
		}

		public GetRoomsReview GetRoomsReview()
        {
			var available = _context.Rooms
		        .Where(r =>  r.RoomStatusId == (int)RoomStatusEnum.Available)
                .BranchFilter()
		        .Count();

			var Occupied = _context.Rooms
			   .Where(r => r.RoomStatusId == (int)RoomStatusEnum.Occupied)
               .BranchFilter()
			   .Count();

			var Maintainance = _context.Rooms
			   .Where(r => r.RoomStatusId == (int)RoomStatusEnum.Maintenance)
               .BranchFilter()
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
    }
}
