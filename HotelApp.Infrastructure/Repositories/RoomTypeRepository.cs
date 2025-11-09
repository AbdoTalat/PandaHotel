using AutoMapper;
using AutoMapper.QueryableExtensions;
using HotelApp.Application.DTOs.Dashboard;
using HotelApp.Application.DTOs.Reservation;
using HotelApp.Application.DTOs.RoomTypes;
using HotelApp.Application.Interfaces.IRepositories;
using HotelApp.Domain;
using HotelApp.Domain.Entities;
using HotelApp.Domain.Enums;
using HotelApp.Helper;
using HotelApp.Infrastructure.DbContext;
using HotelApp.Infrastructure.UnitOfWorks;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HotelApp.Infrastructure.Repositories
{
    public class RoomTypeRepository : GenericRepository<RoomType>, IRoomTypeRepository
    {
        private readonly ApplicationDbContext _context;
		private readonly IConfigurationProvider _mapperConfig;

		public RoomTypeRepository(ApplicationDbContext context, IConfigurationProvider mapperConfig)
			: base(context, mapperConfig)
        {
            _context = context;
            _mapperConfig = mapperConfig;
		}

		public async Task<IEnumerable<RoomType>> GetRoomTypesByIDsAsync(List<int> roomTypeIds)
		{
			var roomTypes = await _context.RoomTypes
				.AsNoTracking()
				.Where(rt => roomTypeIds.Contains(rt.Id))
				.ToListAsync();

			return roomTypes;
		}
        public async Task<IEnumerable<GetRoomTypesForReservationDTO>> GetRoomTypesForReservationAsync(
            RoomAvailabilityRequestDTO dto)
        {
            var roomTypes = await _context.RoomTypes
                .BranchFilter()
                .Where(rt => rt.IsActive)
                .Select(rt => new GetRoomTypesForReservationDTO
                {
                    Id = rt.Id,
                    Name = rt.Name,
                    MaxNumOfAdults = rt.MaxNumOfAdults,
                    MaxNumOfChildrens = rt.MaxNumOfChildrens
                })
                .ToListAsync();

            var result = await GetAvailableRoomsForAllRoomTypesAsync(dto);

            foreach(var roomType in roomTypes)
            {
                var availability = result.FirstOrDefault(t => t.RoomTypeId == roomType.Id);
                roomType.NumOfAvailableRooms = availability != null &&
                    availability.TotalAvailableRooms > 0 ? availability.TotalAvailableRooms : 0;
            }

            return roomTypes;
        }

        public async Task<List<RoomAvailabilityResultDTO>> GetAvailableRoomsForAllRoomTypesAsync(
            RoomAvailabilityRequestDTO dto, CancellationToken cancellationToken = default)
        {
            var result = new List<RoomAvailabilityResultDTO>();

            await using var connection = _context.Database.GetDbConnection();
            if (connection.State != ConnectionState.Open)
                await connection.OpenAsync(cancellationToken);

            await using var command = connection.CreateCommand();
            command.CommandText = "GetAvailableRoomsForAllRoomTypes";
            command.CommandType = CommandType.StoredProcedure;

            command.Parameters.Add(new SqlParameter("@BranchId", dto.BranchId));
            command.Parameters.Add(new SqlParameter("@ReservationId", (object?)dto.ReservationId ?? DBNull.Value));
            command.Parameters.Add(new SqlParameter("@CheckInDate", dto.CheckInDate));
            command.Parameters.Add(new SqlParameter("@CheckOutDate", dto.CheckOutDate));

            await using var reader = await command.ExecuteReaderAsync(cancellationToken);

            while (await reader.ReadAsync(cancellationToken))
            {
                result.Add(new RoomAvailabilityResultDTO
                {
                    RoomTypeId = reader.GetInt32(reader.GetOrdinal("RoomTypeId")),
                    TotalAvailableRooms = reader.GetInt32(reader.GetOrdinal("TotalAvailable"))
                });
            }

            return result;
        }

        public async Task<int> GetAvailableRoomCountAsync(int branchId, int roomTypeId, DateTime checkInDate, DateTime checkOutDate, int? reservationId)
		{
            var branchParam = new SqlParameter("@BranchId", branchId);
            var roomTypeParam = new SqlParameter("@RoomTypeId", roomTypeId);
            var reservationParam = new SqlParameter("@ReservationId", (object?)reservationId ?? DBNull.Value);
            var quantityParam = new SqlParameter("@Quantity", DBNull.Value);
            var checkInParam = new SqlParameter("@CheckInDate", checkInDate);
            var checkOutParam = new SqlParameter("@CheckOutDate", checkOutDate);
            var totalAvailableParam = new SqlParameter
            {
                ParameterName = "@TotalAvailableRooms",
                SqlDbType = SqlDbType.Int,
                Direction = ParameterDirection.Output
            };

            await _context.Database.ExecuteSqlRawAsync(
                "EXEC GetAvailableRoomsForRoomType @BranchId, @RoomTypeId, @ReservationId, @Quantity, @CheckInDate, @CheckOutDate, @TotalAvailableRooms OUTPUT",
                branchParam, roomTypeParam, reservationParam, quantityParam, checkInParam, checkOutParam, totalAvailableParam);

            return (int)(totalAvailableParam.Value ?? 0);
        }

    }
}
