using AutoMapper;
using HotelApp.Application.Services.CurrentUserService;
using HotelApp.Domain;
using HotelApp.Domain.Entities;
using HotelApp.Infrastructure.DbContext;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using System.Security.Claims;

namespace HotelApp.Infrastructure
{
	public class UnitOfWork : IUnitOfWork, IAsyncDisposable
	{
		private readonly ApplicationDbContext _context;
		private readonly IConfigurationProvider _mapperConfig;
		private readonly ICurrentUserService _currentUserService;
		private IDbContextTransaction? _transaction;
		private readonly Dictionary<Type, object> _repositories = new();

		public UnitOfWork(ApplicationDbContext context,
			IConfigurationProvider mapperConfig,
			ICurrentUserService currentUserService)
		{
			_context = context;
			_mapperConfig = mapperConfig;
			_currentUserService = currentUserService;
		}

		public IGenericRepository<T> Repository<T>() where T : class
		{
			if (_repositories.TryGetValue(typeof(T), out var repo))
				return (IGenericRepository<T>)repo;

			var newRepo = new GenericRepository<T>(_context, _mapperConfig, _currentUserService);
			_repositories[typeof(T)] = newRepo;
			return newRepo;
		}

		public Task<int> CommitAsync(CancellationToken cancellationToken = default)
			=> _context.SaveChangesAsync(cancellationToken);

		public Task<int> CommitAsync(bool skipAuditFields, CancellationToken cancellationToken = default)
			=> _context.SaveChangesAsync(skipAuditFields, cancellationToken);

		public async Task BeginTransactionAsync()
		{
			if (_transaction == null)
				_transaction = await _context.Database.BeginTransactionAsync();
		}

		public async Task CommitTransactionAsync()
		{
			if (_transaction == null)
				throw new InvalidOperationException("No active transaction to commit.");

			try
			{
				await CommitAsync();
				await _transaction.CommitAsync();
			}
			catch
			{
				await RollbackTransactionAsync();
				throw;
			}
			finally
			{
				await DisposeTransactionAsync();
			}
		}

		public async Task RollbackTransactionAsync()
		{
			if (_transaction != null)
			{
				await _transaction.RollbackAsync();
				await DisposeTransactionAsync();
			}
		}

		private async Task DisposeTransactionAsync()
		{
			if (_transaction != null)
			{
				await _transaction.DisposeAsync();
				_transaction = null;
			}
		}

		public async ValueTask DisposeAsync()
		{
			if (_transaction != null)
				await _transaction.DisposeAsync();
			await _context.DisposeAsync();
		}
	}
}
