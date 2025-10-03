using AutoMapper;
using AutoMapper.QueryableExtensions;
using HotelApp.Helper;
using HotelApp.Application.DTOs;
using HotelApp.Domain.Entities;
using HotelApp.Infrastructure.DbContext;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HotelApp.Application.Interfaces.IRepositories;
using HotelApp.Infrastructure.UnitOfWorks;

namespace HotelApp.Infrastructure.Repositories
{
    public class UserRepository : GenericRepository<User>, IUserRepository
	{
		private readonly ApplicationDbContext _context;
		private readonly IConfigurationProvider _mapConfig;

		public UserRepository(ApplicationDbContext context, IConfigurationProvider mapConfig)
			: base(context, mapConfig)
        {
			_context = context;
			_mapConfig = mapConfig;
		}

		public async Task<IEnumerable<UsersDTO>> GetAllUsersAsync()
		{
			var users = await _context.UserBranches
							.BranchFilter()
							.Select(ub => ub.User)
							.Distinct()
							.ProjectTo<UsersDTO>(_mapConfig)
							.ToListAsync();

			return users;
		}


		public async Task<IEnumerable<int>> GetUserBranchesIDsByUserIdAsync(int Id)
		{
			var userBranchesIDs = await _context.UserBranches
				.Where(ub => ub.UserId == Id)
				.Select(ub => ub.BranchId)
				.ToListAsync();

			return userBranchesIDs;
		}
	}
}
