using AutoMapper;
using AutoMapper.QueryableExtensions;
using HotelApp.Application.DTOs.Guests;
using HotelApp.Infrastructure.DbContext;
using Microsoft.EntityFrameworkCore;
using HotelApp.Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using HotelApp.Domain.Entities;
using HotelApp.Application.Services.CurrentUserService;
using HotelApp.Domain;
using HotelApp.Application.Interfaces.IRepositories;
using HotelApp.Infrastructure.UnitOfWorks;

namespace HotelApp.Infrastructure.Repositories
{
    public class GuestRepository : GenericRepository<Guest> , IGuestRepository
    {
        private readonly ApplicationDbContext _context;
		private readonly IConfigurationProvider _mapperConfig;
        private readonly ICurrentUserService _currentUserService;

        public GuestRepository(ApplicationDbContext context, IConfigurationProvider mapperConfig, ICurrentUserService currentUserService)
            : base(context, mapperConfig)
        {
            _context = context;
			_mapperConfig = mapperConfig;
            _currentUserService = currentUserService;
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

        public async Task<List<GetSearchedGuestsDTO>> SearchGuestsAsync(string input)
        {
            if (string.IsNullOrWhiteSpace(input) || input.Trim().Length < 4)
                return new List<GetSearchedGuestsDTO>();

			input = input.Trim();

            var query = _context.Guests.AsQueryable();

            bool isEmail = input.Contains("@") && input.Contains(".");
            bool isPhone = input.All(char.IsDigit) || input.StartsWith("+");

            if (isEmail)
            {
                query = query.Where(g => g.Email.StartsWith(input));
            }
            else if (isPhone)
            {
                query = query.Where(g => g.Phone.StartsWith(input));
            }
            else
            {
                query = query.Where(g => g.FullName.StartsWith(input));
            }

            var results = await query
                .OrderBy(g => g.FullName)
                .Take(20)
                .Select(g => new GetSearchedGuestsDTO
                {
                    Id = g.Id,
                    DisplayText = isEmail
                        ? g.Email
                        : isPhone
                            ? g.Phone
                            : g.FullName
                })
                .ToListAsync();

            return results;
        }


    }
}
