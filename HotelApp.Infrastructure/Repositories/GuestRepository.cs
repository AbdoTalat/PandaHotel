using AutoMapper;
using AutoMapper.QueryableExtensions;
using HotelApp.Application.DTOs.Guests;
using HotelApp.Application.IRepositories;
using HotelApp.Infrastructure.DbContext;
using Microsoft.EntityFrameworkCore;
using HotelApp.Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HotelApp.Infrastructure.Repositories
{
    public class GuestRepository : IGuestRepository
    {
        private readonly ApplicationDbContext _context;
		private readonly IConfigurationProvider _mapperConfig;

		public GuestRepository(ApplicationDbContext context, IConfigurationProvider mapperConfig)
        {
            _context = context;
			_mapperConfig = mapperConfig;
		}
		public async Task<IEnumerable<GetSearchedGuestsDTO>> SerachGuestsByEmailAsync(string email)
		{
			var guest = await _context.Guests
				.AsNoTracking()
				.Where(g => g.Email.ToLower().Contains(email.ToLower()))
				.BranchFilter()
				.ProjectTo<GetSearchedGuestsDTO>(_mapperConfig)
				.ToListAsync();

			return guest;
		}
	}
}
