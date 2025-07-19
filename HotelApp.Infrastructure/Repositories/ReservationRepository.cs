using AutoMapper;
using AutoMapper.QueryableExtensions;
using HotelApp.Application.DTOs.Reservation;
using HotelApp.Application.IRepositories;
using HotelApp.Infrastructure.DbContext;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HotelApp.Infrastructure.Repositories
{
    public class ReservationRepository : IReservationRepository
    {
        private readonly ApplicationDbContext _context;
        private readonly IConfigurationProvider _mapper;

        public ReservationRepository(ApplicationDbContext context, IConfigurationProvider mapper)
        {
            _context = context;
            _mapper = mapper;
        }


		//public async Task<IEnumerable<GetAllReservationsDTO>> GetAllReservationAsync()
  //      {
  //          var reservations = await
  //      }
	}
}
