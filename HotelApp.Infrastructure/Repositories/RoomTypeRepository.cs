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
using Microsoft.EntityFrameworkCore.Storage;
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
        public async Task<IEnumerable<GetRoomTypesForReservationDTO>> GetRoomTypesForReservationAsync(RoomTypeAvailabilityRequestDTO dto)
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

            var result = await GetAllRoomTypesAvailabilityAsync(dto);

            foreach(var roomType in roomTypes)
            {
                var availability = result.FirstOrDefault(t => t.RoomTypeId == roomType.Id);
                roomType.NumOfAvailableRooms = availability != null &&
                    availability.TotalAvailableRooms > 0 ? availability.TotalAvailableRooms : 0;
            }

            return roomTypes;
        }

        public async Task<List<RoomTypeAvailabilityResultDTO>> GetAllRoomTypesAvailabilityAsync(
            RoomTypeAvailabilityRequestDTO dto, CancellationToken cancellationToken = default)
        {
            var result = new List<RoomTypeAvailabilityResultDTO>();

            try
            {
                var connection = _context.Database.GetDbConnection();
                var transaction = _context.Database.CurrentTransaction?.GetDbTransaction();

                if (connection.State != ConnectionState.Open)
                    await connection.OpenAsync(cancellationToken);

                await using var command = connection.CreateCommand();
                command.CommandText = "GetAvailableRoomsForAllRoomTypes";
                command.CommandType = CommandType.StoredProcedure;

                // ✅ Attach the active transaction if any
                if (transaction != null)
                    command.Transaction = transaction;

                command.Parameters.Add(new SqlParameter("@BranchId", dto.BranchId));
                command.Parameters.Add(new SqlParameter("@ReservationId", (object?)dto.ReservationId ?? DBNull.Value));
                command.Parameters.Add(new SqlParameter("@CheckInDate", dto.CheckInDate));
                command.Parameters.Add(new SqlParameter("@CheckOutDate", dto.CheckOutDate));

                await using var reader = await command.ExecuteReaderAsync(cancellationToken);

                while (await reader.ReadAsync(cancellationToken))
                {
                    result.Add(new RoomTypeAvailabilityResultDTO
                    {
                        RoomTypeId = reader.GetInt32(reader.GetOrdinal("RoomTypeId")),
                        TotalAvailableRooms = reader.GetInt32(reader.GetOrdinal("TotalAvailable"))
                    });
                }

                return result;
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while retrieving room type availability.", ex);
            }
        }


        public async Task<int> GetRoomTypeAvailabilityAsync(RoomTypeAvailabilityRequestDTO dto)
		{
            var branchParam = new SqlParameter("@BranchId", dto.BranchId);
            var roomTypeParam = new SqlParameter("@RoomTypeId", dto.RoomTypeId);
            var reservationParam = new SqlParameter("@ReservationId", (object?)dto.ReservationId ?? DBNull.Value);
            var quantityParam = new SqlParameter("@Quantity", DBNull.Value);
            var checkInParam = new SqlParameter("@CheckInDate", dto.CheckInDate);
            var checkOutParam = new SqlParameter("@CheckOutDate", dto.CheckOutDate);
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
