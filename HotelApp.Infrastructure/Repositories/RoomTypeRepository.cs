using AutoMapper;
using AutoMapper.QueryableExtensions;
using HotelApp.Helper;
using HotelApp.Application.DTOs.RoomTypes;
using HotelApp.Application.IRepositories;
using HotelApp.Domain;
using HotelApp.Domain.Entities;
using HotelApp.Domain.Enums;
using HotelApp.Infrastructure.DbContext;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HotelApp.Infrastructure.Repositories
{
    public class RoomTypeRepository : IRoomTypeRepository
    {
        private readonly ApplicationDbContext _context;
		private readonly IConfigurationProvider _mapper;

		public RoomTypeRepository(ApplicationDbContext context, IConfigurationProvider mapper)
        {
            _context = context;
			_mapper = mapper;
		}

		public async Task<IEnumerable<RoomType>> GetRoomTypesByIDsAsync(List<int> roomTypeIds)
		{
			var roomTypes = await _context.RoomTypes
				.AsNoTracking()
				.Where(rt => roomTypeIds.Contains(rt.Id))
				.ToListAsync();

			return roomTypes;
		}
		public async Task<IEnumerable<GetRoomTypesForReservationDTO>> GetRoomTypesForReservation()
        {
			var roomTypes = await _context.RoomTypes
				.BranchFilter()
			   .Select(rt => new GetRoomTypesForReservationDTO
			   {
				   Id = rt.Id,
				   Name = rt.Name,
				   NumOfAvailableRooms = rt.Rooms.Count(r => r.RoomStatusId == (int)RoomStatusEnum.Available),
				   MaxNumOfAdults = rt.MaxNumOfAdults,
				   MaxNumOfChildrens = rt.MaxNumOfChildrens
			   })
			   .ToListAsync();

			return roomTypes;
        }
	}
}
