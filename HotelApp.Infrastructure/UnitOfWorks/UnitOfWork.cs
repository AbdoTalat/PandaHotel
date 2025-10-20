using AutoMapper;
using HotelApp.Application.Services.CurrentUserService;
using HotelApp.Domain;
using HotelApp.Domain.Entities;
using HotelApp.Infrastructure.DbContext;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using System.Security.Claims;
using HotelApp.Infrastructure.Repositories;
using Microsoft.Extensions.DependencyInjection;
using System.Collections;
using HotelApp.Application.Interfaces;
using HotelApp.Application.Interfaces.IRepositories;
using Microsoft.AspNetCore.Identity;
using HotelApp.Domain.Common;

namespace HotelApp.Infrastructure.UnitOfWorks
{
    public class UnitOfWork : IUnitOfWork, IAsyncDisposable
    {

        private readonly ApplicationDbContext _context;
        private readonly IConfigurationProvider _mapperConfig;
        private readonly ICurrentUserService _currentUserService;
        private readonly Dictionary<Type, object> _repositories = new();
        private IDbContextTransaction? _transaction;


        public UnitOfWork(
                   ApplicationDbContext context,
                   IConfigurationProvider mapperConfig,
                   ICurrentUserService currentUserService,
                   RoleManager<Role> roleManager)
        {
            _context = context;
            _mapperConfig = mapperConfig;
            _currentUserService = currentUserService;

            // Initialize all repositories
            GuestRepository = new GuestRepository(_context, _mapperConfig, currentUserService);
            BranchRepository = new BranchRepository(_context, _mapperConfig);
            UserBranchRepository = new UserBranchRepository(_context, _mapperConfig);
            UserRepository = new UserRepository(_context, _mapperConfig);
            CompanyRepository = new CompanyRepository(_context, _mapperConfig);
            CountryRepository = new CountryRepository(_context, _mapperConfig);
            ReservationSourceRepository = new ReservationSourceRepository(_context, _mapperConfig);
            ReservationRepository = new ReservationRepository(_context, _mapperConfig);
            ReservationRoomRepository = new ReservationRoomRepository(_context, _mapperConfig);
            ReservationRoomTypeRepository = new ReservationRoomTypeRepository(_context, _mapperConfig);
            RoomOptionRepository = new RoomOptionRepository(_context, _mapperConfig);
            RoomTypeRepository = new RoomTypeRepository(_context, _mapperConfig);
            RoomStatusRepository = new RoomStatusRepository(_context, _mapperConfig);
            StateRepository = new StateRepository(_context, _mapperConfig);
            RoleRepository = new RoleRepository(roleManager, _context, _mapperConfig);
            SystemSettingRepository = new SystemSettingRepositroy(_context, _mapperConfig);
            RateRepository = new RateRepository(_context, _mapperConfig);
            CalculationTypeRepository = new CalculationTypeRepository(_context, _mapperConfig);
            FloorRepository = new FloorRepository(_context, _mapperConfig);
            GuestReservationRepository = new GuestReservationRepository(_context, _mapperConfig);
            ProofTypeRepository = new ProofTypeRepository(_context, _mapperConfig);
            RoomRepository = new RoomRepository(_context, _mapperConfig);
            OptionRepository = new OptionRepository(_context, _mapperConfig);
            RoomTypeRateRepository = new RoomTypeRateRepository(_context, _mapperConfig);
            ReservationHistoryRepository = new ReservationHistoryRepository(_context, _mapperConfig);
        }


        #region Repos
        public IGuestRepository GuestRepository { get; private set; }
        public IBranchRepository BranchRepository { get; private set; }
        public IUserBranchRepository UserBranchRepository { get; private set; }
        public IUserRepository UserRepository { get; private set; }
        public ICompanyRepository CompanyRepository { get; private set; }
        public ICountryRepository CountryRepository { get; private set; }
        public IReservationSourceRepository ReservationSourceRepository { get; private set; }
        public IReservationRepository ReservationRepository { get; private set; }
        public IReservationRoomRepository ReservationRoomRepository { get; private set; }
        public IReservationRoomTypeRepository ReservationRoomTypeRepository { get; private set; }
        public IRoomOptionRepository RoomOptionRepository { get; private set; }
        public IRoomTypeRepository RoomTypeRepository { get; private set; }
        public IRoomStatusRepository RoomStatusRepository { get; private set; }
        public IStateRepository StateRepository { get; private set; }
        public IRoleRepository RoleRepository { get; private set; }
        public ISystemSettingRepositroy SystemSettingRepository { get; private set; }
        public IRateRepository RateRepository { get; private set; }
        public ICalculationTypeRepository CalculationTypeRepository { get; private set; }
        public IFloorRepository FloorRepository { get; private set; }
        public IGuestReservationRepository GuestReservationRepository { get; private set; }
        public IProofTypeRepository ProofTypeRepository { get; private set; }
        public IRoomRepository RoomRepository { get; private set; }
        public IOptionRepository OptionRepository { get; private set; }
        public IRoomTypeRateRepository RoomTypeRateRepository { get; private set; }
        public IReservationHistoryRepository ReservationHistoryRepository { get; private set; }
        #endregion



        public async Task<int> CommitAsync(bool skipAuditFields = false, CancellationToken cancellationToken = default)
        {
            if (!skipAuditFields)
                ApplyAuditInformation();
            return await _context.SaveChangesAsync(cancellationToken);
        }
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
        
        #region Audit Logic
        private void ApplyAuditInformation()
        {
            var entries = _context.ChangeTracker
                .Entries<BaseEntity>()
                .Where(e => e.State == EntityState.Added || e.State == EntityState.Modified);

            var now = DateTime.UtcNow;
            var userId = _currentUserService.UserId;

            foreach (var entry in entries)
            {
                if (entry.State == EntityState.Added)
                {
                    entry.Entity.CreatedDate = now;
                    if (userId.HasValue)
                        entry.Entity.CreatedById = userId.Value;
                }
                else if (entry.State == EntityState.Modified)
                {
                    entry.Entity.LastModifiedDate = now;
                    if (userId.HasValue)
                        entry.Entity.LastModifiedById = userId.Value;
                }
            }
        }
        #endregion
    }
}
