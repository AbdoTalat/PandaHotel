using AutoMapper;
using HotelApp.Application.Interfaces.IRepositories;
using HotelApp.Domain.Entities;
using HotelApp.Infrastructure.DbContext;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HotelApp.Infrastructure.Repositories
{
    public class BranchRepository : GenericRepository<Branch>, IBranchRepository
    {
        private readonly ApplicationDbContext _context;
        private readonly IConfigurationProvider _mapperConfig;

        public BranchRepository(ApplicationDbContext context, IConfigurationProvider mapperConfig)
            : base(context, mapperConfig)
        {
            _context = context;
            _mapperConfig = mapperConfig;
        }
    }
}
