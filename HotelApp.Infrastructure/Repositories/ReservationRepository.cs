using AutoMapper;
using AutoMapper.QueryableExtensions;
using HotelApp.Application.DTOs.Reservation;
using HotelApp.Application.IRepositories;
using HotelApp.Helper;
using HotelApp.Infrastructure.DbContext;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace HotelApp.Infrastructure.Repositories
{
    public class ReservationRepository : IReservationRepository
    {
        private readonly ApplicationDbContext _context;
        private readonly IConfigurationProvider _mapperConfig;

        public ReservationRepository(ApplicationDbContext context, IConfigurationProvider mapperConfig)
        {
            _context = context;
            _mapperConfig = mapperConfig;
        }


        public async Task<IEnumerable<GetAllReservationsDTO>> GetFilteredReservationsAsync(ReservationFilterDTO dto)
        {
            var Query = _context.Reservations
                .BranchFilter()
                .AsQueryable();

            if (dto.CheckInDate.HasValue)
                Query = Query.Where(r => r.CheckInDate >= dto.CheckInDate.Value);

            if (dto.CheckOutDate.HasValue)
                Query = Query.Where(r => r.CheckOutDate <= dto.CheckOutDate.Value);

            if (!string.IsNullOrEmpty(dto.PrimaryGuestName))
            {
                Query = Query.Where(r => r.guestReservations.Any(gr =>
                    gr.IsPrimaryGuest &&
                    gr.Guest.FullName.Contains(dto.PrimaryGuestName)));
            }

            var result = await Query.ProjectTo<GetAllReservationsDTO>(_mapperConfig).ToListAsync();

            return result;
        }

        //public async Task<IEnumerable<GetAllReservationsDTO>> GetAllReservationAsync()
        //      {
        //          var reservations = await
        //      }
    }
}
