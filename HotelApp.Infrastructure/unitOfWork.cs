using AutoMapper;
using HotelApp.Application.Services.CurrentUserService;
using HotelApp.Domain;
using HotelApp.Domain.Entities;
using HotelApp.Infrastructure.DbContext;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace HotelApp.Infrastructure
{
    public class unitOfWork : IUnitOfWork
    {
        private readonly ApplicationDbContext _context;
        private readonly IConfigurationProvider _mapperConfig;
		private readonly ICurrentUserService _currentUserService;
		private readonly Dictionary<Type, object> _repositories = new Dictionary<Type, object>();

        public unitOfWork(ApplicationDbContext context,
            IConfigurationProvider mapperConfig, ICurrentUserService currentUserService)
        {
            _context = context;
            _mapperConfig = mapperConfig;
			_currentUserService = currentUserService;
		}

		public IGenericRepository<T> Repository<T>() where T : class
		{
			if (_repositories.ContainsKey(typeof(T)))
				return (IGenericRepository<T>)_repositories[typeof(T)];

			var repo = new GenericRepository<T>(_context, _mapperConfig, _currentUserService);
			_repositories[typeof(T)] = repo;
			return repo;
		}

		public async Task<int> CommitAsync(CancellationToken cancellationToken = default)
		{
			return await _context.SaveChangesAsync(cancellationToken);
		}

		public async Task<int> CommitAsync(bool skipAuditFields, CancellationToken cancellationToken = default)
		{
			return await _context.SaveChangesAsync(skipAuditFields, cancellationToken);
		}
		public void Dispose() { }
    }
}
