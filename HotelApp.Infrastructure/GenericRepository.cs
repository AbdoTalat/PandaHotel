using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using System.Linq;
using HotelApp.Domain;
using HotelApp.Infrastructure.DbContext;
using AutoMapper.QueryableExtensions;
using AutoMapper;
using HotelApp.Helper;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore.Query;
using HotelApp.Domain.Common;
using System.Security.Claims;
using HotelApp.Application.Services.CurrentUserService;

namespace HotelApp.Infrastructure
{
    public class GenericRepository<T> : IGenericRepository<T> where T : class
    {
        private readonly ApplicationDbContext _context;
        private readonly IConfigurationProvider _mapperConfig;
		private readonly ICurrentUserService _currentUserService;
		private readonly DbSet<T> _dbSet;

		public GenericRepository(ApplicationDbContext context,
            IConfigurationProvider mapperConfig, ICurrentUserService currentUserService)
        {
            _context = context;
            _mapperConfig = mapperConfig;
			_currentUserService = currentUserService;
			_dbSet = _context.Set<T>();
        }

		#region Get Methods

		public IQueryable<T> GetAllIQueryable()
		{
			return _dbSet.AsQueryable();
		}
		public async Task<IEnumerable<T>> GetAllAsync(Expression<Func<T, bool>>? predicate = null, bool SkipBranchFilter = false)
        {
			IQueryable<T> query = _dbSet.AsNoTracking()
				.BranchFilter(SkipBranchFilter);

			if (predicate != null)
				query = query.Where(predicate);

			return await query.ToListAsync();
		}
        public async Task<IEnumerable<TDto>> GetAllAsDtoAsync<TDto>(Expression<Func<T, bool>>? predicate = null, bool SkipBranchFilter = false) where TDto : class
        {
            IQueryable<T> query = _dbSet.AsNoTracking()
				.BranchFilter(SkipBranchFilter);

			if (predicate != null)
            {
                query = query.Where(predicate);
            }

            return await query.ProjectTo<TDto>(_mapperConfig).ToListAsync();
        }
		public async Task<TDto?> GetByIdAsDtoAsync<TDto>(int id, Expression<Func<T, bool>>? predicate = null, bool SkipBranchFilter = false) where TDto : class
		{
			IQueryable<T> query = _dbSet.AsNoTracking()
				.Where(e => EF.Property<int>(e, "Id") == id)
				.BranchFilter(SkipBranchFilter);

			if (predicate != null)
			{
				query = query.Where(predicate);
			}

			return await query
				.ProjectTo<TDto>(_mapperConfig)
				.FirstOrDefaultAsync();
		}
		public async Task<T?> GetByIdAsync(int id, Expression<Func<T, bool>>? predicate = null, bool SkipBranchFilter = false)
		{
            IQueryable<T> query = _dbSet.AsNoTracking()
                .Where(e => EF.Property<int>(e, "Id") == id)
                .BranchFilter(SkipBranchFilter);
                
            if (predicate != null)
			{
				query = query.Where(predicate);
			}

			return await query.FirstOrDefaultAsync();
		}
        public async Task<T?> FirstOrDefaultAsync(Expression<Func<T, bool>>? predicate = null, bool SkipBranchFilter = false)
        {
            IQueryable<T> query = _dbSet
                .AsNoTracking()
                .BranchFilter(SkipBranchFilter); ;

            if (predicate != null)
                query = query.Where(predicate);

            return await query.FirstOrDefaultAsync();
        }
        public async Task<TDto?> FirstOrDefaultAsDtoAsync<TDto>(Expression<Func<T, bool>>? predicate = null, bool skipBranchFilter = false) where TDto : class
        {
            IQueryable<T> query = _dbSet.AsNoTracking()
                .BranchFilter(skipBranchFilter);

            if (predicate != null)
                query = query.Where(predicate);

            return await query
                .ProjectTo<TDto>(_mapperConfig)
                .FirstOrDefaultAsync();
        }
        #endregion

        #region Add & Edit & Delete
        public async Task AddNewAsync(T entity)
        {
            var branchId = BranchContext.CurrentBranchId;

            // If entity has a BranchId property and branchId is not null
            var prop = typeof(T).GetProperty("BranchId");
            if (prop != null && prop.PropertyType == typeof(int) && branchId.HasValue)
            {
                prop.SetValue(entity, branchId.Value);
            }

            await _dbSet.AddAsync(entity);
        }
        public async Task AddRangeAsync(IEnumerable<T> entities)
        {
            var branchId = BranchContext.CurrentBranchId;

            foreach (var entity in entities)
            {
                var prop = typeof(T).GetProperty("BranchId");
                if (prop != null && prop.PropertyType == typeof(int) && branchId.HasValue)
                {
                    prop.SetValue(entity, branchId.Value);
                }
            }

            await _dbSet.AddRangeAsync(entities);
        }
        public void Update(T entity)
            => _dbSet.Update(entity);
        public void UpdateRange(IEnumerable<T> entities)
            => _dbSet.UpdateRange(entities);

		public async Task BulkUpdateAsync(
			Expression<Func<T, bool>> predicate,
			Expression<Func<SetPropertyCalls<T>, SetPropertyCalls<T>>> setProperties,
			bool skipBranchFilter = false,
			bool skipAuditFields = false)
		{
			IQueryable<T> query = _dbSet.BranchFilter(skipBranchFilter);

			if (predicate != null)
				query = query.Where(predicate);

			if (!skipAuditFields)
			{
				setProperties = CombineSetProperties(setProperties, s =>
					s.SetProperty(e => EF.Property<int>(e, "LastModifiedById"), _currentUserService.UserId)
					 .SetProperty(e => EF.Property<DateTime>(e, "LastModifiedDate"), DateTime.UtcNow)
				);
			}

			await query.ExecuteUpdateAsync(setProperties);
		}

		public void Delete(T entity)
            => _dbSet.Remove(entity);
        public void DeleteRange(IEnumerable<T> entities)
            => _dbSet.RemoveRange(entities);

		public async Task DeleteByIdAsync(int id)
		{
			await _dbSet
                .BranchFilter()
				.Where(e => EF.Property<int>(e, "Id") == id)
				.ExecuteDeleteAsync();
		}
		#endregion

		#region Other Methods
		public async Task<bool> IsExistsAsync(Expression<Func<T, bool>> predicate, bool skipBranchFilter = false) 
        {
			IQueryable<T> query = _dbSet.AsNoTracking()
				.BranchFilter(skipBranchFilter);

            return await query.AnyAsync(predicate);
		}
		#endregion


		#region Helper Methods
		private static Expression<Func<SetPropertyCalls<T>, SetPropertyCalls<T>>> CombineSetProperties(
			Expression<Func<SetPropertyCalls<T>, SetPropertyCalls<T>>> first,
			Expression<Func<SetPropertyCalls<T>, SetPropertyCalls<T>>> second)
		{
			var param = Expression.Parameter(typeof(SetPropertyCalls<T>), "p");

			// Invoke first(p)
			var firstBody = Expression.Invoke(first, param);

			// Invoke second(first(p))
			var secondBody = Expression.Invoke(second, firstBody);

			return Expression.Lambda<Func<SetPropertyCalls<T>, SetPropertyCalls<T>>>(secondBody, param);
		}
		#endregion

	}
}
